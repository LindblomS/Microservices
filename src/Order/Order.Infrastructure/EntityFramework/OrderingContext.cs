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
    IDbContextTransaction currentTransaction;

    public OrderingContext(DbContextOptions<OrderingContext> options) : base(options)
    {
        Id = Guid.NewGuid();
    }

    public DbSet<OrderEntity> Orders { get; private set; }
    public DbSet<OrderItemEntity> OrderItems { get; private set; }
    public DbSet<BuyerEntity> Buyers { get; private set; }
    public DbSet<CardEntity> Cards { get; private set; }

    public Guid Id { get; }
    public bool Active { get => currentTransaction is not null; }

    public async Task BeginAsync()
    {
        if (Active)
            throw new InvalidOperationException("Unit of work is active");

        currentTransaction = await Database.BeginTransactionAsync();
    }

    public async Task CommitAsync(IUnitOfWork unitOfWork)
    {
        if (unitOfWork?.Id != Id)
            throw new InvalidOperationException("Unit of work is not current");

        try
        {
            await currentTransaction.CommitAsync();
        }
        catch (Exception exception)
        {
            throw new UnitOfWorkException(Id, "Something went wrong commiting unit of work", exception);
        }
        finally
        {
            await currentTransaction?.RollbackAsync();

            if (currentTransaction is not null)
            {
                await currentTransaction.DisposeAsync();
                currentTransaction = null;
            }
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
}
