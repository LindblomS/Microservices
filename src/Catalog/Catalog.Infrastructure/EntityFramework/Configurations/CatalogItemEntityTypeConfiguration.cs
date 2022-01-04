namespace Catalog.Infrastructure.EntityFramework.Configurations;

using Catalog.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItemEntity>
{
    public void Configure(EntityTypeBuilder<CatalogItemEntity> builder)
    {
        builder.ToTable("item", CatalogContext.defaultSchema);
        builder.HasKey(e => e.Id).HasName("pk_item");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.Price).HasColumnName("price").HasPrecision(18,2);
        builder.Property(e => e.CatalogTypeId).HasColumnName("type_id");
        builder.Property(e => e.CatalogBrandId).HasColumnName("brand_id");
        builder.Property(e => e.AvailableStock).HasColumnName("available_stock");

        builder.HasOne(e => e.CatalogType).WithMany();
        builder.HasOne(e => e.CatalogBrand).WithMany();
    }
}
