namespace Ordering.Infrastructure.EntityFramework.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.Models;
using System;

internal class CardEntityTypeConfiguration : IEntityTypeConfiguration<CardEntity>
{
    public void Configure(EntityTypeBuilder<CardEntity> builder)
    {
        throw new NotImplementedException();
    }
}
