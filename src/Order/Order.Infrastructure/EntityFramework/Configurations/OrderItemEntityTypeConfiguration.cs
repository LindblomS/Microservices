namespace Ordering.Infrastructure.EntityFramework.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.Models;

internal class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.ToTable("order_item", OrderingContext.defaultSchema);
        builder.HasKey(e => e.Id).HasName("pk_order_item");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.OrderId).HasColumnName("order_id");
        builder.Property(e => e.ProductName).HasColumnName("product_name");
        builder.Property(e => e.UnitPrice).HasColumnName("unit_price").HasPrecision(18,2);
        builder.Property(e => e.Units).HasColumnName("units");
    }
}
