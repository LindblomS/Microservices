using CFS.Domain.Aggregates;
using CFS.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace CFS.Infrastructure
{
    public class RepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RepositoryFactory> _logger;

        public RepositoryFactory(IServiceProvider serviceProvider, ILogger<RepositoryFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public T GetRepository<T>(IDbTransaction transaction)
        {
            var logger = (ILogger<DbWithTransaction>)_serviceProvider.GetService(typeof(ILogger<DbWithTransaction>));
            var db = new DbWithTransaction(transaction, logger);

            if (typeof(T) is ICustomerRepository)

            switch (typeof(T))
            {
                case ICustomerRepository type:
                    return new CustomerRepository(db);
                case IFacilityRepository type:
                    return new FacilityRepository(db) as T;
                case IServiceRepository type:
                    return new ServiceRepository(db) as T;
                default:
                    throw new ArgumentException("Type is not known");
            }
        }
    }
}
