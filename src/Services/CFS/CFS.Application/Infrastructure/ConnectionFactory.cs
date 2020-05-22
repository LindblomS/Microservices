using CFS.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace CFS.Application.Infrastructure
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionstring;

        public ConnectionFactory(string connectionString)
        {
            _connectionstring = !string.IsNullOrEmpty(connectionString) 
                ? connectionString 
                : throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionstring);
        }
    }
}
