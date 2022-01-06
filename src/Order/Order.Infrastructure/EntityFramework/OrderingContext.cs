namespace Ordering.Infrastructure.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Application.Exceptions;
using Ordering.Application.Services;
using Ordering.Infrastructure.EntityFramework.Configurations;
using Ordering.Infrastructure.Models;
using System;
using System.Threading.Tasks;

public class OrderingContext : DbContext, IUnitOfWork
{
    internal const string defaultSchema = "ordering";
    bool disposed;
    IDbContextTransaction currentTransaction;

    public OrderingContext(DbContextOptions<OrderingContext> options) : base(options)
    {
    }

    public DbSet<OrderEntity> Orders { get; private set; }
    public DbSet<OrderItemEntity> OrderItems { get; private set; }
    public DbSet<BuyerEntity> Buyers { get; private set; }
    public DbSet<CardEntity> Cards { get; private set; }

    public Guid TransactionId { get; private set; }
    public bool Active { get => currentTransaction is not null; }

    public async Task BeginAsync()
    {
        if (Active)
            throw new InvalidOperationException("Unit of work is active");

        currentTransaction = await Database.BeginTransactionAsync();
        TransactionId = currentTransaction.TransactionId;
    }

    public async Task CommitAsync(IUnitOfWork unitOfWork)
    {
        if (unitOfWork?.TransactionId != TransactionId)
            throw new InvalidOperationException("Unit of work is not current");

        try
        {
            await currentTransaction.CommitAsync();
            TransactionId = Guid.Empty;
        }
        catch (Exception exception)
        {
            throw new UnitOfWorkException(TransactionId, "Something went wrong commiting unit of work", exception);
        }
    }

    public IDbContextTransaction GetCurrentTransaction() => currentTransaction;

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

    public override void Dispose()
    {
        if (disposed)
            return;

        currentTransaction?.Dispose();
        base.Dispose();
        disposed = true;
    }
}
