using CFS.Application.Application.Queries;
using CFS.Infrastructure;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure
{
    public class Queries : IQueries
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<Queries> _logger;

        public Queries(IConnectionFactory connectionFactory,  ILogger<Queries> logger)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CustomerViewModel> GetCustomer(int id)
        {
            var sql = "SELECT * FROM Customers WHERE customerId = @id";
            using (var connection = (SqlConnection)_connectionFactory.GetConnection())
            {
                try
                {
                    connection.Open();
                    return await connection.QuerySingleAsync<CustomerViewModel>(sql, id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error");
                    throw;
                }
            }
        }

        public async Task<IList<CustomerViewModel>> GetCustomers()
        {
            var sql = "SELECT * FROM Customers";
            using (var connection = (SqlConnection)_connectionFactory.GetConnection())
            {
                try
                {
                    connection.Open();
                    return (await connection.QueryAsync<CustomerViewModel>(sql)).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error");
                    throw;
                }
            }
        }

        public async Task<IList<FacilityViewModel>> GetFacilities()
        {
            var sql = "SELECT * FROM Facilities";
            using (var connection = (SqlConnection)_connectionFactory.GetConnection())
            {
                try
                {
                    connection.Open();
                    return (await connection.QueryAsync<FacilityViewModel>(sql)).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error");
                    throw;
                }
            }
        }

        public async Task<IList<FacilityViewModel>> GetFacilitiesOnCustomer(int id)
        {
            var sql = "SELECT * FROM Facilities WHERE customerId = @id";
            using (var connection = (SqlConnection)_connectionFactory.GetConnection())
            {
                try
                {
                    connection.Open();
                    return (await connection.QueryAsync<FacilityViewModel>(sql, id)).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error");
                    throw;
                }
            }
        }

        public async Task<FacilityViewModel> GetFacility(int id)
        {
            var sql = "SELECT * FROM Facilities WHERE facilityId = @id";
            using (var connection = (SqlConnection)_connectionFactory.GetConnection())
            {
                try
                {
                    connection.Open();
                    return await connection.QuerySingleAsync<FacilityViewModel>(sql, id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error");
                    throw;
                }
            }
        }

        public async Task<ServiceViewModel> GetService(int id)
        {
            var sql = "SELECT * FROM Services WHERE serviceId = @id";
            using (var connection = (SqlConnection)_connectionFactory.GetConnection())
            {
                try
                {
                    connection.Open();
                    return await connection.QuerySingleAsync<ServiceViewModel>(sql, id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error");
                    throw;
                }
            }
        }

        public async Task<IList<ServiceViewModel>> GetServices()
        {
            var sql = $"SELECT * FROM Services";
            using (var connection = (SqlConnection)_connectionFactory.GetConnection())
            {
                try
                {
                    connection.Open();
                    return (await connection.QueryAsync<ServiceViewModel>(sql)).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error");
                    throw;
                }
            }
        }

        public async Task<IList<ServiceViewModel>> GetServicesOnCustomer(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT * FROM Services s");
            sql.AppendLine("JOIN Facilities f ON f.facilityId = s.facilityId");
            sql.AppendLine("WHERE f.customerId = @id");

            using (var connection = (SqlConnection)_connectionFactory.GetConnection())
            {
                try
                {
                    connection.Open();
                    return (await connection.QueryAsync<ServiceViewModel>(sql.ToString(), id)).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error");
                    throw;
                }
            }
        }
    }
}
