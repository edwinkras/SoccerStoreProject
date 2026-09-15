namespace SoccerStore.Api.Models;

// This class represents one row in the items table.
// It is a plain data model, it does not do any logic on its own.
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    // Size only applies to balls, for example 3, 4, or 5.
    // It stays empty for every other category, so it can be null.
    public int? Size { get; set; }
}

// This is the fixed list of categories a soccer store item can belong to.
// Using an enum instead of free text means every item must pick one of these,
// there is no risk of typos like "Cleat" vs "cleats" vs "Cleets".
public enum Category
{
    Cleats,
    Shirts,
    Socks,
    Bags,
    Jackets,
    Shorts,
    Pants,
    Balls
}
