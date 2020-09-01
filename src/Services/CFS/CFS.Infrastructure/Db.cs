using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using CFS.Domain.SeedWork;

namespace CFS.Infrastructure
{
    public class Db : IDb
    {
        private IDbTransaction _currentTransaction;
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
        }

        public IDbTransaction GetCurrentTransaction => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public void SaveChanges()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException(nameof(_currentTransaction));
            try
            {
                _domainEvents.ForEach(e => _mediator.Publish(e));
                _currentTransaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public IDbTransaction BeginTransaction()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = _connectionFactory.GetConnection().BeginTransaction();

            return _currentTransaction;
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task<T> CommandAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> command)
        {
            try
            {
                var result = await command(_currentTransaction.Connection, _currentTransaction);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR");
                throw;
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters, Entity entity)
        {
            return await CommandAsync(async (connection, transaction) =>
            {
                var result = await connection.ExecuteAsync(sql, parameters, transaction);
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
