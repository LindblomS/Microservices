using System;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace CFS.Infrastructure
{
    public class DataContext
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<DataContext> _logger;

        public DataContext(IConnectionFactory connectionFactory, ILogger<DataContext> logger)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> ExecuteNonQueryAsync(string sql)
        {
            try
            {
                var affectedRows = await _connectionFactory.GetConnection().ExecuteAsync(sql);
                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR");
                return 0;
            }
            
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
