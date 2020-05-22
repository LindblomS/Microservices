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
    public class DataContext
    {
        private readonly IConnectionFactory _connectionFactory;
        private bool _disposed;

        public DataContext(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<int> ExecuteAsync(string sql)
        {
            var affectedRows = await _connectionFactory.GetConnection().ExecuteAsync(sql);
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
    }
}
