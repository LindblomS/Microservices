namespace Ordering.Infrastructure.EntityFramework.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.Models;

internal class ClientRequestEntityTypeConfiguration : IEntityTypeConfiguration<ClientRequestEntity>
{
    public void Configure(EntityTypeBuilder<ClientRequestEntity> builder)
    {
        builder.ToTable("client_request", OrderingContext.defaultSchema);
        builder.HasKey(e => e.Id).HasName("pk_client_request");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Time).HasColumnName("time");
    }
}
