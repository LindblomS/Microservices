namespace Ordering.Infrastructure.EntityFramework.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.Models;

internal class CardTypeEntityTypeConfiguration : IEntityTypeConfiguration<CardTypeEntity>
{
    public void Configure(EntityTypeBuilder<CardTypeEntity> builder)
    {
        builder.ToTable("card_type", OrderingContext.defaultSchema);
        builder.HasKey(x => x.Id).HasName("pk_card_type");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");
    }
}
