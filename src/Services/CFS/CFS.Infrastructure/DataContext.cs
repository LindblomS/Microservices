using CFS.Domain.SeedWork;
using MediatR;
using System;
using System.Data;
using Dapper;
using CFS.Domain.Aggregates.CustomerAggregate;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Common;

namespace CFS.Infrastructure
{
    public class DataContext : IUnitOfWork
    {
        private IDbTransaction _currentTransaction;
        private readonly IConnectionFactory _connectionFactory;
        private bool _disposed;

        public DataContext(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<int> ExecuteAsync(string sql)
        {
            if (_currentTransaction == null) throw new InvalidOperationException($"Transaction is null");

            var affectedRows = await _currentTransaction.Connection.ExecuteAsync(sql);
            return affectedRows;
        }

        public async Task<List<T>> ListQueryAsync<T>(string sql)
        {
            try
            {
                using (var connection = _connectionFactory.GetConnection())
                {
                    List<T> result = (List<T>)await connection.QueryAsync<List<T>>(sql);
                    return result;
                }
            }
            catch (Exception)
            {
                // Log exception
                return new List<T>();
            }
        }

        public async Task<T> QueryAsync<T>(string sql)
        {
            try
            {
                using (var connection = _connectionFactory.GetConnection())
                {
                    T result = (T)await connection.QueryAsync<T>(sql);
                    return result;
                }
            }
            catch (Exception)
            {
                // Log exception
                return default(T);
            }
        }

        public bool HasActiveTransaction() => _currentTransaction != null;

        public IDbTransaction BeginTransaction()
        {
            if (_currentTransaction != null) return null;
            _currentTransaction = _connectionFactory.GetConnection().BeginTransaction();
            return _currentTransaction;
        }

        public async Task<bool> CommitTransaction(IDbTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction is not current");
            bool success;

            try
            {
                await ((DbTransaction)transaction).CommitAsync();
                success = true;
            }
            catch
            {
                RollbackTransaction();
                success = false;
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

            return success;
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
