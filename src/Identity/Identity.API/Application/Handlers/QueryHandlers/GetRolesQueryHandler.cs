namespace Identity.API.Application.Handlers.QueryHandlers
{
    using MediatR;
    using Services.Identity.Contracts.Models;
    using Services.Identity.Contracts.Queries;
    using Services.Identity.Contracts.Models.Results;
    using Services.Identity.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using System.Linq;
    using Identity.API.Application.Factories;

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, QueryResult<IEnumerable<RoleReadModel>>>
    {
        private readonly IConnectionProvider _connectionProvider;

        public GetRolesQueryHandler(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public async Task<QueryResult<IEnumerable<RoleReadModel>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connectionProvider.GetConnection();
            var roles = await GetRoles(connection);
            var roleClaims = await GetRoleClaims(connection);

            foreach (var role in roles)
                foreach (var claim in roleClaims.Where(x => x.RoleId == role.Id))
                    role.Claims.Add(new(claim.ClaimType, claim.ClaimValue));

            return ResultFactory.CreateSuccessResult(roles);
        }

        private async Task<IEnumerable<RoleReadModel>> GetRoles(SqlConnection connection)
        {
            var sql = $@"select id as {nameof(Role.Id)}, display_name as {nameof(Role.DisplayName)} from role";
            var dtos = await connection.QueryAsync<Role>(sql);
            var roles = new List<RoleReadModel>();

            foreach (var role in dtos)
                roles.Add(new RoleReadModel(role.Id, role.DisplayName, new List<ClaimReadModel>()));

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
