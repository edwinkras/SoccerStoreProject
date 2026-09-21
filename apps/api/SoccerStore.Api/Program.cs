using Microsoft.EntityFrameworkCore;
using Npgsql;
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
    options.UseNpgsql(connectionString)
           // The real tables use snake_case column names (category_id, stock_quantity).
           // This makes EF Core map to those automatically instead of expecting CategoryId.
           .UseSnakeCaseNamingConvention());

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
// You can filter by categoryId, size, or search by name using query parameters
// Example: /api/items?categoryId=8&size=5
// Example: /api/items?search=nike
app.MapGet("/api/items", async (int? categoryId, int? size, string? search, StoreContext db) =>
{
    // Start with all items, including their category name, then narrow it down
    IQueryable<Item> query = db.Items.Include(item => item.Category);

    if (categoryId != null)
    {
        query = query.Where(item => item.CategoryId == categoryId);
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

// GET every category, so the frontend knows what options exist for filtering
// and for the dropdown when adding a new item.
app.MapGet("/api/categories", async (StoreContext db) =>
{
    List<Category> categories = await db.Categories.ToListAsync();
    return Results.Ok(categories);
});

// GET a single item by its id
app.MapGet("/api/items/{id}", async (int id, StoreContext db) =>
{
    Item? item = await db.Items.Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == id);

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

    newItem.CreatedAt = DateTime.UtcNow;
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
    existingItem.CategoryId = updatedItem.CategoryId;
    existingItem.Price = updatedItem.Price;
    existingItem.StockQuantity = updatedItem.StockQuantity;
    existingItem.Size = updatedItem.Size;

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

// POST to sell an item. This calls the sell_item() database function instead
// of just editing StockQuantity directly, so a real sale record gets created
// too, not just a silently changed number.
app.MapPost("/api/items/{id}/sell", async (int id, SellRequest request, StoreContext db) =>
{
    try
    {
        var saleId = await db.Database
            .SqlQueryRaw<int>("SELECT sell_item({0}, {1})", id, request.Quantity)
            .FirstAsync();

        Item? updatedItem = await db.Items.Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == id);

        return Results.Ok(new { saleId, item = updatedItem });
    }
    catch (PostgresException ex) when (ex.MessageText.Contains("Not enough stock"))
    {
        return Results.BadRequest(ex.MessageText);
    }
    catch (PostgresException ex) when (ex.MessageText.Contains("does not exist"))
    {
        return Results.NotFound(ex.MessageText);
    }
});

app.Run();
