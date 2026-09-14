using Microsoft.EntityFrameworkCore;
using SoccerStore.Api.Data;
using SoccerStore.Api.Models;

// This is the entry point of the API.
// It sets up the server, then defines each endpoint below.

var builder = WebApplication.CreateBuilder(args);

// Register Swagger so we can test the API in a browser
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Read the connection string from appsettings.json
// Local dev points at the Podman Postgres container; production reads this
// from the ConnectionStrings__StoreDatabase environment variable instead.
string connectionString = builder.Configuration.GetConnectionString("StoreDatabase")
    ?? throw new InvalidOperationException(
        "No 'ConnectionStrings:StoreDatabase' was found. Set it in appsettings.json, " +
        "appsettings.Development.json, or the ConnectionStrings__StoreDatabase environment variable.");

builder.Services.AddDbContext<StoreContext>(options =>
    options.UseNpgsql(connectionString));

// Allow a frontend (Angular, React, etc.) to call this API from a different port
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowFrontend");

// Quick connectivity check - hit this first to confirm the DB is reachable.
app.MapGet("/db-check", async (StoreContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return Results.Ok(new
    {
        connected = canConnect,
        provider = db.Database.ProviderName
    });
})
.WithName("DbCheck");

// GET all items in the inventory
// You can filter by category, size, or search by name using query parameters
// Example: /api/items?category=Balls&size=5
// Example: /api/items?search=nike
app.MapGet("/api/items", async (Category? category, int? size, string? search, StoreContext db) =>
{
    // Start with all items, then narrow it down based on what was passed in
    IQueryable<Item> query = db.Items;

    if (category != null)
    {
        query = query.Where(item => item.Category == category);
    }

    if (size != null)
    {
        query = query.Where(item => item.Size == size);
    }

    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(item => item.Name.ToLower().Contains(search.ToLower()));
    }

    List<Item> items = await query.ToListAsync();
    return Results.Ok(items);
});

// GET a single item by its id
app.MapGet("/api/items/{id}", async (int id, StoreContext db) =>
{
    Item? item = await db.Items.FindAsync(id);

    if (item == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(item);
});

// POST a new item into the inventory
app.MapPost("/api/items", async (Item newItem, StoreContext db) =>
{
    if (string.IsNullOrEmpty(newItem.Name))
    {
        return Results.BadRequest("Item name is required.");
    }

    db.Items.Add(newItem);
    await db.SaveChangesAsync();

    return Results.Created("/api/items/" + newItem.Id, newItem);
});

// PUT to update an existing item's details
app.MapPut("/api/items/{id}", async (int id, Item updatedItem, StoreContext db) =>
{
    Item? existingItem = await db.Items.FindAsync(id);

    if (existingItem == null)
    {
        return Results.NotFound();
    }

    existingItem.Name = updatedItem.Name;
    existingItem.Category = updatedItem.Category;
    existingItem.Price = updatedItem.Price;
    existingItem.StockQuantity = updatedItem.StockQuantity;

    await db.SaveChangesAsync();

    return Results.Ok(existingItem);
});

// DELETE an item from the inventory
app.MapDelete("/api/items/{id}", async (int id, StoreContext db) =>
{
    Item? item = await db.Items.FindAsync(id);

    if (item == null)
    {
        return Results.NotFound();
    }

    db.Items.Remove(item);
    await db.SaveChangesAsync();

    return Results.Ok();
});

// POST to sell an item, this changes stock and is separate from a normal edit
app.MapPost("/api/items/{id}/sell", async (int id, SellRequest request, StoreContext db) =>
{
    Item? item = await db.Items.FindAsync(id);

    if (item == null)
    {
        return Results.NotFound();
    }

    if (item.StockQuantity < request.Quantity)
    {
        return Results.BadRequest("Not enough stock to complete this sale.");
    }

    item.StockQuantity = item.StockQuantity - request.Quantity;
    await db.SaveChangesAsync();

    return Results.Ok(item);
});

app.Run();
