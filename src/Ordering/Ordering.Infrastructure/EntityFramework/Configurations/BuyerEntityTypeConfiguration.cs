namespace Ordering.Infrastructure.EntityFramework.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.Models;

internal class BuyerEntityTypeConfiguration : IEntityTypeConfiguration<BuyerEntity>
{
    public void Configure(EntityTypeBuilder<BuyerEntity> builder)
    {
        builder.ToTable("buyer", OrderingContext.defaultSchema);
        builder.HasKey(e => e.Id).HasName("pk_buyer");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");

        builder.HasMany(e => e.Cards).WithOne().HasForeignKey(e => e.BuyerId).HasConstraintName("fk_buyer_card");
    }
}
