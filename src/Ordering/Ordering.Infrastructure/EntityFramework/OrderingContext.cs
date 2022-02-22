namespace Ordering.Infrastructure.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Infrastructure.EntityFramework.Configurations;
using Ordering.Infrastructure.Models;
using System;
using System.Data;
using System.Threading.Tasks;

public class OrderingContext : DbContext
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

    public IDbContextTransaction GetCurrentTransaction() => currentTransaction;
    public bool HasActiveTransaction { get => currentTransaction != null; }


    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (currentTransaction != null) return null;

        currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (currentTransaction != null)
            {
                currentTransaction.Dispose();
                currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            currentTransaction?.Rollback();
        }
        finally
        {
            if (currentTransaction != null)
            {
                currentTransaction.Dispose();
                currentTransaction = null;
            }
        }
    }

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
