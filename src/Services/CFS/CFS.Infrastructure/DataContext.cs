using CFS.Domain.SeedWork;
using MediatR;
using System;
using System.Data;
using Dapper;
using CFS.Domain.Aggregates.CustomerAggregate;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace CFS.Infrastructure
{
    public class DataContext
    {
        private readonly IConnectionFactory _connectionFactory;

        public DataContext(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<int> ExecuteNonQueryAsync(string sql)
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
                    var result = (await connection.QueryAsync<T>(sql)).ToList();
                    return result;
                }
            }
            catch
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
                    T result = (await connection.QueryAsync<T>(sql)).FirstOrDefault();
                    return result;
                }
            }
            catch
            {
                // Log exception
                return default(T);
            }
        }
    }
}
