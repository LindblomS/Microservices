namespace Services.Order.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class OrderEntityConfiguration : IEntityTypeConfiguration<Domain.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Order> builder)
        {
            builder.ToTable("order", OrderContext.DEFAULT_SCHEMA);
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder
                .Property(e => e.CustomerId)
                .HasField("_customerId")
                .HasColumnName("customer_id")
                .IsRequired();
        }
    }
}
