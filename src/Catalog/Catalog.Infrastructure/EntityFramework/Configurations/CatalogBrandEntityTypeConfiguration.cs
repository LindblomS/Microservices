namespace Catalog.Infrastructure.EntityFramework.Configurations;

using Catalog.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrandEntity>
{
    public void Configure(EntityTypeBuilder<CatalogBrandEntity> builder)
    {
        builder.ToTable("brand", CatalogContext.defaultSchema);
        builder.HasKey(e => e.Id).HasName("pk_brand");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Brand).HasColumnName("brand");
    }
}
