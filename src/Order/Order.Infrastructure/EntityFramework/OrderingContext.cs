namespace Ordering.Infrastructure.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Application.Services;
using Ordering.Infrastructure.EntityFramework.Configurations;
using Ordering.Infrastructure.Models;
using System;
using System.Threading.Tasks;

public class OrderingContext : DbContext, IUnitOfWork
{
    internal const string defaultSchema = "ordering";

    public OrderingContext(DbContextOptions<OrderingContext> options) : base(options)
    {
    }

    public DbSet<OrderEntity> Orders { get; private set; }
    public DbSet<OrderItemEntity> OrderItems { get; private set; }
    public DbSet<BuyerEntity> Buyers { get; private set; }
    public DbSet<CardEntity> Cards { get; private set; }

    public Guid Id => throw new NotImplementedException();
    public bool Active => throw new NotImplementedException();

    public void Begin()
    {
        throw new NotImplementedException();
    }

    public Task Commit(Guid unitOfWorkId)
    {
        throw new NotImplementedException();
    }

    public IDbContextTransaction GetCurrentTransaction() => throw new NotImplementedException();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderStatusEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CardEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CardTypeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
    }
}
