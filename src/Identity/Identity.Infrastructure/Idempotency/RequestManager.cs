namespace Services.Identity.Infrastructure.Idempotency
{
    using System;
    using System.Threading.Tasks;
    using Dapper;

    public class RequestManager : IRequestManager
    {
        private readonly IConnectionProvider _connectionProvider;

        public RequestManager(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        }
        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            if (exists)
                throw new InvalidOperationException($"Request with {id} already exists");

            var request = new ClientRequest(id, typeof(T).Name, DateTime.UtcNow);

            using var connection = _connectionProvider.GetConnection();
            await connection.OpenAsync();

            var sql = "insert into client_request (id, name, time) values (@id, @name, @time)";
            _ = await connection.ExecuteAsync(sql, new { id = request.Id, name = request.Name, time = request.Time });

        }

        public async Task<bool> ExistAsync(Guid id)
        {
            using var connection = _connectionProvider.GetConnection();
            await connection.OpenAsync();

            var sql = "select count(id) from client_request where id = @id";
            var count = await connection.QuerySingleAsync<int>(sql, new { id = id });
            return count == 1;
        }
    }
}
