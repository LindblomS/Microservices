namespace Services.Order.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Services.Order.Domain;

    public class OrderItemEntityConfiguration : IEntityTypeConfiguration<Domain.OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items", OrderContext.DEFAULT_SCHEMA);
            builder.Ignore(e => e.DomainEvents);
            builder.Ignore(e => e.Id);
            builder.HasKey(e => new { e.OrderId, e.Name });

            builder
                .Property(e => e.OrderId)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("order_id")
                .IsRequired();

            builder
                .Property(e => e.Name)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("name")
                .IsRequired();

            builder
                .Property(e => e.Quantity)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("quantity")
                .IsRequired();
        }
    }
}
