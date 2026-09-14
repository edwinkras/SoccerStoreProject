using Microsoft.EntityFrameworkCore;
using SoccerStore.Api.Models;

namespace SoccerStore.Api.Data;

// This class is the bridge between our C# code and the database.
// EF Core uses this class to know what tables exist and how to talk to them.
public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
    }

    // This line tells EF Core there is a table of items
    public DbSet<Item> Items => Set<Item>();
}
