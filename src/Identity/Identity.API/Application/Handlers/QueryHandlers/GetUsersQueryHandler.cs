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
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Identity.API.Application.Factories;

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, QueryResult<IEnumerable<User>>>
    {
        private readonly IConnectionProvider _connectionProvider;

        public GetUsersQueryHandler(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public async Task<QueryResult<IEnumerable<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connectionProvider.GetConnection();
            await connection.OpenAsync();
            var userClaims = await GetUserClaims(connection);
            var userRoles = await GetUserRoles(connection);
            var users = new List<User>();

            foreach (var userClaim in userClaims)
            {
                var user = users.SingleOrDefault(x => x.Id == userClaim.UserId);

                if (user is null)
                    user = new(userClaim.UserId, userClaim.Username, new List<Claim>(), new List<Role>());

                user.Claims.Add(new(userClaim.ClaimType, userClaim.ClaimValue));
            }

            foreach (var user in users)
            {
                foreach (var userRole in userRoles.Where(x => x.UserId == user.Id))
                    user.Roles.Add(new(userRole.RoleId, userRole.DisplayName));
            }

            return ResultFactory.CreateSuccessResult<IEnumerable<User>>(users);
        }

        private async Task<IEnumerable<UserClaim>> GetUserClaims(SqlConnection connection)
        {
            var sql = $@"
                select 
                    user.id as {nameof(UserClaim.UserId)}, 
                    user.username as {nameof(UserClaim.Username)}, 
                    user_claim.claim_type as {nameof(UserClaim.ClaimType)}, 
                    user_claim.claim_value as {nameof(UserClaim.ClaimValue)} 
                from user
                left join user_claim on user_claim.user_id = user.id";

            return await connection.QueryAsync<UserClaim>(sql);
        }

        private async Task<IEnumerable<UserRole>> GetUserRoles(SqlConnection connection)
        {
            var sql = $@"
                select 
                    user_role.user_id as {nameof(UserRole.UserId)}, 
                    user_role.role_id as {nameof(UserRole.RoleId)},
                    role.display_name as {nameof(UserRole.DisplayName)}
                from user_role
                join role on role.id = user_role.role_id";
            return await connection.QueryAsync<UserRole>(sql);
        }

        private record UserClaim(Guid UserId, string Username, string ClaimType, string ClaimValue);
        private record UserRole(Guid UserId, string RoleId, string DisplayName);
    }
}
