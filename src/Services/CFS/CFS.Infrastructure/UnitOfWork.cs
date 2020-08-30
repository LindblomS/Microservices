using CFS.Domain.Aggregates;
using CFS.Domain.SeedWork;
using System;
using System.Data;

namespace CFS.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbTransaction _transaction;
        private IDbConnection _connection;

        private ICustomerRepository _customerRepository;
        private IFacilityRepository _facilityRepository;
        private IServiceRepository _serviceRepository;

        private readonly IConnectionFactory _connectionFactory;
        private readonly RepositoryFactory _repositoryFactory;
        private bool _disposed;

        public UnitOfWork(IConnectionFactory connectionFactory, RepositoryFactory repositoryFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        }

        public ICustomerRepository CustomerRepository 
        {
            get
            {
                if (_transaction == null)
                    throw new InvalidOperationException("Transaction is null");

                if (_customerRepository == null)
                    _customerRepository = _repositoryFactory.GetRepository<ICustomerRepository>(_transaction);

                return _customerRepository;
            }
        }

        public IFacilityRepository FacilityRepository
        {
            get
            {
                if (_transaction == null)
                    throw new InvalidOperationException("Transaction is null");

                if (_facilityRepository == null)
                    _facilityRepository = _repositoryFactory.GetRepository<IFacilityRepository>(_transaction);

                return _facilityRepository;
            }
        }

        public IServiceRepository ServiceRepository
        {
            get
            {
                if (_transaction == null)
                    throw new InvalidOperationException("Transaction is null");

                if (_serviceRepository == null)
                    _serviceRepository = _repositoryFactory.GetRepository<IServiceRepository>(_transaction);

                return _serviceRepository;
            }
        }

        public void BeginTransaction()
        {
            _connection = _connectionFactory.GetConnection();
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        private void ResetRepositories()
        {
            _customerRepository = null;
            _facilityRepository = null;
            _serviceRepository = null;
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
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        UnitOfWork()
        {
            dispose(false);
        }
    }
}
