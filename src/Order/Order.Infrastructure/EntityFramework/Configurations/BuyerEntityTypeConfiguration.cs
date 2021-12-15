namespace Ordering.Infrastructure.EntityFramework.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.Models;
using System;

internal class BuyerEntityTypeConfiguration : IEntityTypeConfiguration<BuyerEntity>
{
    public void Configure(EntityTypeBuilder<BuyerEntity> builder)
    {
        throw new NotImplementedException();
    }
}
