namespace Services.Identity.Infrastructure
{
    using MediatR;
    using Services.Identity.Domain.Domain.SeedWork;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;

    public class DbContext : IUnitOfWork
    {
        private SqlTransaction _transaction;
        private SqlConnection _connection;
        private readonly IConnectionProvider _connectionProvider;
        private readonly IMediator _mediator;
        private readonly List<INotification> _notifications;
        private readonly List<Task<int>> _commandsToExecute;

        public DbContext(IConnectionProvider connectionProvider, IMediator mediator)
        {
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _notifications = new List<INotification>();
            _commandsToExecute = new List<Task<int>>();
        }

        public void Execute<T>(string sql, object parameters, IEnumerable<INotification> notifications)
        {
            _notifications.AddRange(notifications);
            _commandsToExecute.Add(Transaction.Connection.ExecuteAsync(sql, parameters, Transaction));
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters)
        {
            return await Transaction.Connection.QuerySingleOrDefaultAsync<T>(sql, parameters, Transaction);
        }

        public void Dispose()
        {
            _connection?.Close();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var rowsAffected = 0;
            foreach (var command in _commandsToExecute)
                rowsAffected += await command;

            foreach (var notification in _notifications)
                await _mediator.Publish(notification);

            _notifications.Clear();
            await CommitTransactionAsync();
            
            return rowsAffected;
        }

        private async Task CommitTransactionAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollBackTransaction();
                throw;
            }
            finally
            {
                await _connection.CloseAsync();

                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        private async Task RollBackTransaction()
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        private SqlTransaction Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _connection = _connectionProvider.GetConnection();
                    _connection.Open();
                    _transaction = _connection.BeginTransaction();
                }

                return _transaction;
            }
        }
    }
}
