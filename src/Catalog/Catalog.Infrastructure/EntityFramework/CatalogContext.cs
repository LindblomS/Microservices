namespace Catalog.Infrastructure.EntityFramework;

using Catalog.Infrastructure.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

public class CatalogContext : DbContext
{
    internal const string defaultSchema = "catalog";

    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
    }
}
