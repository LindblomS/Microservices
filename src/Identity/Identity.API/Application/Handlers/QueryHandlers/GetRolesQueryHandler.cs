namespace Identity.API.Application.Handlers.QueryHandlers
{
    using MediatR;
    using Services.Identity.Contracts.Models;
    using Services.Identity.Contracts.Queries;
    using Services.Identity.Contracts.Results;
    using Services.Identity.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, QueryResult<IEnumerable<Role>>>
    {
        private readonly IConnectionProvider _connectionProvider;

        public GetRolesQueryHandler(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public async Task<QueryResult<IEnumerable<Services.Identity.Contracts.Models.Role>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connectionProvider.GetConnection();

        }

        private async Task<IEnumerable<Services.Identity.Contracts.Models.Role>> GetRoles(SqlConnection connection)
        {
            var sql = $@"select id as {nameof(Role.Id)}, display_name as {nameof(Role.DisplayName)} from role";
            var dtos = await connection.QueryAsync<Role>(sql);
            var roles = new List<Services.Identity.Contracts.Models.Role>();

            foreach (var role in dtos)
                roles.Add(new Services.Identity.Contracts.Models.Role(role.Id, role.DisplayName, new List<Claim>()));

            return roles;
        }

        private async Task<IEnumerable<RoleClaim>> GetRoleClaims(SqlConnection connection)
        {
            var sql = $@"select [role_id] as {nameof(RoleClaim.RoleId)}, [claim_type] as {nameof(RoleClaim.ClaimType)}, [claim_value] as {nameof(RoleClaim.ClaimValue)} from role_claim";
            return await connection.QueryAsync<RoleClaim>(sql);
        }

        private record Role(string Id, string DisplayName);
        private record RoleClaim(string RoleId, string ClaimType, string ClaimValue);
    }
}
