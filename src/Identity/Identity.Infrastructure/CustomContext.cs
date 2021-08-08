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
    using System.Linq;
    using Services.Identity.Infrastructure.Models;

    public interface IDbContext
    {
        Task<SqlTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(SqlTransaction transaction);
        bool HasActiveTransaction();

        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters);
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters);
        void Execute(Command command);
    }

    public class CustomContext : IUnitOfWork, IDbContext
    {
        private SqlTransaction _transaction;
        private SqlConnection _connection;
        private readonly IConnectionProvider _connectionProvider;
        private readonly IMediator _mediator;
        private readonly List<Command> _commands;

        public CustomContext(IConnectionProvider connectionProvider, IMediator mediator)
        {
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _commands = new List<Command>();
        }

        public void Execute(Command command)
        {
            _commands.Add(command);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters)
        {
            if (_transaction is null)
            {
                using (var connection = _connectionProvider.GetConnection())
                {
                    await connection.OpenAsync();
                    return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
                }
            }

            return await _transaction.Connection.QuerySingleOrDefaultAsync<T>(sql, parameters, _transaction);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters)
        {
            if (_transaction is null)
            {
                using (var connection = _connectionProvider.GetConnection())
                {
                    await connection.OpenAsync();
                    return await connection.QueryAsync<T>(sql, parameters);
                }
            }

            return await _transaction.Connection.QueryAsync<T>(sql, parameters, _transaction);
        }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var commands = _commands.ToList();
            _commands.Clear();

            if (_transaction == null)
            {
                var transaction = await BeginTransactionAsync();
                var affectedRows = await SaveChangesAsync(transaction, commands, cancellationToken);
                await CommitTransactionAsync(transaction);
                return affectedRows;
            }

            return await SaveChangesAsync(_transaction, commands, cancellationToken);
        }

        private async Task<int> SaveChangesAsync(SqlTransaction transaction, IEnumerable<Command> commands, CancellationToken token)
        {
            var connection = transaction.Connection;
            var rowsAffected = 0;

            foreach (var command in commands)
                rowsAffected += await connection.ExecuteAsync(command.Sql, command.Parameters, transaction);

            //foreach (var notification in commands.Select(x => x.Notifications))
            //    await _mediator.Publish(notification);

            return rowsAffected;
        }

        public bool HasActiveTransaction()
        {
            return _transaction != null;
        }

        public async Task<SqlTransaction> BeginTransactionAsync()
        {
            if (_transaction != null)
                throw new InvalidOperationException("a transaction is already active");
            else
            {
                _connection = _connectionProvider.GetConnection();
                await _connection.OpenAsync();
                _transaction = (SqlTransaction)await _connection.BeginTransactionAsync();
            }

            return _transaction;
        }

        public async Task CommitTransactionAsync(SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (transaction != _transaction)
                throw new InvalidOperationException("transaction is not current");

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
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }

                await _connection.CloseAsync();
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

        public void Dispose()
        {
            _connection?.Close();
        }
    }
}
