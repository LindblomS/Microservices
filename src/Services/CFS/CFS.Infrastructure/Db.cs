using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using CFS.Domain.SeedWork;
using System.Data.Common;

namespace CFS.Infrastructure
{
    public class Db : IDb
    {
        private DbTransaction _currentTransaction;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IMediator _mediator;
        private List<INotification> _domainEvents;
        private readonly ILogger<Db> _logger;
        private bool _disposed;

        public Db(IConnectionFactory connectionFactory, IMediator mediator, ILogger<Db> logger)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domainEvents = new List<INotification>();
        }

        public DbTransaction GetCurrentTransaction => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task<bool> CommitTransactionAsync(DbTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction is not current");
        
            var success = false;
            try
            {
                _domainEvents.ForEach(e => _mediator.Publish(e));
                await _currentTransaction.CommitAsync();
                _domainEvents.Clear();
                success = true;
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync();
                _logger.LogError(ex, "");
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }

            return success;
        }
 
        public async Task<DbTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;
            var connection = (DbConnection)_connectionFactory.GetConnection();
            await connection.OpenAsync();
            _currentTransaction = await connection.BeginTransactionAsync();
            return _currentTransaction;
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                _domainEvents.Clear();
                await _currentTransaction?.RollbackAsync();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task<T> CommandAsync<T>(Func<DbConnection, DbTransaction, Task<T>> command)
        {
            try
            {
                var result = await command(_currentTransaction.Connection, _currentTransaction);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR");
                await RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters, Entity entity)
        {
            return await CommandAsync(async (connection, transaction) =>
            {
                var result = await connection.ExecuteAsync(sql, parameters, transaction);
                if (entity.DomainEvents?.Any() == true)
                    _domainEvents.AddRange(entity.DomainEvents);
                return result;
            });
        }

        public async Task<T> GetAsync<T>(string sql, object parameters)
        {
            return await CommandAsync(async (connection, transaction) =>
            {
                return await connection.QuerySingleAsync<T>(sql, parameters, transaction);
            });
        }

        public async Task<IList<T>> SelectAsync<T>(string sql, object parameters)
        {
            return await CommandAsync(async (connection, transaction) =>
            {
                return (await connection.QueryAsync<T>(sql, parameters, transaction)).ToList();
            });
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_currentTransaction != null)
                    {
                        _currentTransaction.Dispose();
                        _currentTransaction = null;
                    }
                }
                _disposed = true;
            }
        }
    }
}
