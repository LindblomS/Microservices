namespace Services.Customer.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CustomerEntityConfiguration : IEntityTypeConfiguration<Domain.Customer>
    {
        public void Configure(EntityTypeBuilder<Domain.Customer> builder)
        {
            builder.ToTable("customer", CustomerContext.DEFAULT_SCHEMA);
            builder.HasKey(e => e.Id);
            builder.Ignore(e => e.DomainEvents);
            builder
                .Property<string>("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("name")
                .IsRequired();
        }
    }
}
