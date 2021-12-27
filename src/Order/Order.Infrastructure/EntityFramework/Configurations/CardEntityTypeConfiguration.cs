namespace Ordering.Infrastructure.EntityFramework.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.Models;

internal class CardEntityTypeConfiguration : IEntityTypeConfiguration<CardEntity>
{
    public void Configure(EntityTypeBuilder<CardEntity> builder)
    {
        builder.ToTable("card", OrderingContext.defaultSchema);
        builder.HasKey(e => e.Id).HasName("pk_card");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.BuyerId).HasColumnName("buyer_id");
        builder.Property(e => e.Type).HasColumnName("type");
        builder.Property(e => e.Number).HasColumnName("number");
        builder.Property(e => e.SecurityNumber).HasColumnName("security_number");
        builder.Property(e => e.HolderName).HasColumnName("holder_name");
        builder.Property(e => e.Expiration).HasColumnName("expiration");
    }
}
