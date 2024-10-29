using HSCodeDatabase.DataAccess.Entities;
using HSCodeDatabase.DataAccess.EntityConfigurations; // Ensure you import the configurations
using Microsoft.EntityFrameworkCore;

namespace HSCodeDatabase.DataAccess;

public class MainContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    //public DbSet<Root> Roots { get; set; }
    //public DbSet<Product> Products { get; set; }
    //public DbSet<SubCategory> SubCategories { get; set; }

    public MainContext(DbContextOptions<MainContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Register entity configurations
        //modelBuilder.ApplyConfiguration(new SubCategoryConfiguration());
        //modelBuilder.ApplyConfiguration(new RootConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}
