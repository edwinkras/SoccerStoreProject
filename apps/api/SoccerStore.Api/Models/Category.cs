namespace SoccerStore.Api.Models;

// This class represents one row in the categories table.
// The actual allowed category names (Cleats, Shirts, etc.) are enforced
// by a CHECK constraint in the database itself, not in this class.
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
