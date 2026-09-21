namespace SoccerStore.Api.Models;

// This class represents one row in the items table.
// CategoryId is a foreign key pointing at the categories table,
// instead of storing the category as a plain enum value on the item itself.
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int CategoryId { get; set; }

    // Filled in automatically by EF Core when we ask for it,
    // lets us read item.Category.Name without a separate query.
    public Category? Category { get; set; }

    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    // Size only applies to balls, for example 3, 4, or 5.
    // It stays empty for every other category, so it can be null.
    public int? Size { get; set; }

    public DateTime CreatedAt { get; set; }
}
