using CFS.Domain.SeedWork;
using MediatR;
using System;
using System.Data;
using Dapper;
using CFS.Domain.Aggregates.CustomerAggregate;

namespace CFS.Infrastructure
{
    public class DataContext : IUnitOfWork
    {
        private readonly IMediator _mediator;
        private IDbTransaction _currentTransaction;
        private readonly IConnectionFactory _connectionFactory;
        private bool _disposed;

        public DataContext(IMediator mediator, IConnectionFactory connectionFactory)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public int Execute(string sql)
        {
            var affectedRows =_currentTransaction.Connection.Execute(sql);
            return affectedRows;
        }

        public T Query<T>(string sql)
        {
            T result = default(T);

            try
            {
                using (var connection = _connectionFactory.GetConnection())
                {
                    result = (T)connection.Query<T>(sql);

                }
            }
            catch (Exception)
            {
                // Log exception
            }

            return result;
        }

        public IDbTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction() => _currentTransaction != null;

        public IDbTransaction BeginTransaction()
        {
            if (_currentTransaction != null) return null;
            _currentTransaction = _connectionFactory.GetConnection().BeginTransaction();
            return _currentTransaction;
        }

        public void CommitTransaction(IDbTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction is not current");

            try
            {
                transaction.Commit();
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _currentTransaction?.Dispose();
            }

            _disposed = true;
        }
    }
}
