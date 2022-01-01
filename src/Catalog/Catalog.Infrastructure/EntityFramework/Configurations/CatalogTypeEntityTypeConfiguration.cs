namespace Catalog.Infrastructure.EntityFramework.Configurations;

using Catalog.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CatalogTypeEntityTypeConfiguration : IEntityTypeConfiguration<CatalogTypeEntity>
{
    public void Configure(EntityTypeBuilder<CatalogTypeEntity> builder)
    {
        builder.ToTable("type", CatalogContext.defaultSchema);
        builder.HasKey(e => e.Id).HasName("pk_id");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Type).HasColumnName("type");
    }
}
