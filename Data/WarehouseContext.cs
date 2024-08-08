using Microsoft.EntityFrameworkCore;

public class WarehouseContext : DbContext
{
    public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options) { }

    public DbSet<Gudang> Gudangs { get; set; }
    public DbSet<Barang> Barangs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gudang>().ToTable("Table_Gudang");
        modelBuilder.Entity<Barang>().ToTable("Table_Barang");
    }
}

/**
 This class defines a database context for a warehouse management system, 
 inheriting from the DbContext class in Entity Framework Core.


- Constructor WarehouseContext(DbContextOptions<WarehouseContext> options): 
    Initializes a new instance of the WarehouseContext class, passing in database connection options.
- Properties Gudangs and Barangs: 
    Expose database sets for Gudang and Barang entities, allowing for CRUD operations.
- OnModelCreating(ModelBuilder modelBuilder): 
    Configures the database schema by mapping Gudang and Barang entities to specific database tables (Table_Gudang and Table_Barang, respectively).
**/