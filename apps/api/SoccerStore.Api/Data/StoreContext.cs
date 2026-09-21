using Microsoft.EntityFrameworkCore;
using SoccerStore.Api.Models;

namespace SoccerStore.Api.Data;

// This class is the bridge between our C# code and the database.
// The actual tables are created by the SQL scripts in infra/podman/initdb,
// not by EF Core migrations, so this class just needs to describe the
// schema accurately, it doesn't own or create it.
public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
    }

    public DbSet<Item> Items => Set<Item>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tell EF Core how Item and Category relate: each item belongs to
        // exactly one category, found through CategoryId.
        modelBuilder.Entity<Item>()
            .HasOne(i => i.Category)
            .WithMany()
            .HasForeignKey(i => i.CategoryId);
    }
}
