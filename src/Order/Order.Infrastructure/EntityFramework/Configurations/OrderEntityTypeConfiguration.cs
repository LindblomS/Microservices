namespace Ordering.Infrastructure.EntityFramework.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.Models;

internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("order", OrderingContext.defaultSchema);
        builder.HasKey(e => e.Id).HasName("pk_order");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.BuyerId).HasColumnName("buyer_id");
        builder.Property(e => e.CardId).HasColumnName("card_id");
        builder.Property(e => e.OrderStatusId).HasColumnName("order_status_id");
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.Created).HasColumnName("created");
        builder.Property(e => e.Street).HasColumnName("street");
        builder.Property(e => e.City).HasColumnName("city");
        builder.Property(e => e.State).HasColumnName("state");
        builder.Property(e => e.Country).HasColumnName("country");
        builder.Property(e => e.ZipCode).HasColumnName("zip_code");

        builder
            .HasOne(e => e.Buyer)
            .WithMany();

        builder
            .HasOne(e => e.Card)
            .WithMany();

        builder
            .HasMany(e => e.OrderItems)
            .WithOne()
            .HasForeignKey(e => e.OrderId)
            .HasConstraintName("fk_order_order_item");
    }
}
