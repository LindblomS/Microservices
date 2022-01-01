namespace Catalog.Infrastructure.EntityFramework;

using Catalog.Infrastructure.EntityFramework.Configurations;
using Catalog.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

public class CatalogContext : DbContext
{
    internal const string defaultSchema = "catalog";

    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {

    }

    public DbSet<CatalogItemEntity> Items { get; private set; }
    public DbSet<CatalogBrandEntity> Brands { get; private set; }
    public DbSet<CatalogTypeEntity> Types { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
    }
}
