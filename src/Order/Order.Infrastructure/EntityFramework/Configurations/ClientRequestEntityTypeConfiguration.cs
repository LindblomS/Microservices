namespace Ordering.Infrastructure.EntityFramework.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.Models;
using System;

internal class ClientRequestEntityTypeConfiguration : IEntityTypeConfiguration<ClientRequestEntity>
{
    public void Configure(EntityTypeBuilder<ClientRequestEntity> builder)
    {
        throw new NotImplementedException();
    }
}
