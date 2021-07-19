using MediatR;
using Services.Identity.Domain.AggregateModels;
using Services.Identity.Domain.AggregateModels.Role;
using Services.Identity.Domain.Domain.SeedWork;
using Services.Identity.Infrastructure.Mappers;
using Services.Identity.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Identity.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbContext _context;

        public RoleRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Create(Role role)
        {
            var sql = "insert into role (id, display_name) values (@id, @display_name)";
            _context.Execute(sql, new { id = role.Id, display_name = role.DisplayName }, new List<INotification>());
        }

        public Task DeleteAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public async Task<Role> GetAsync(string id)
        {
            var sql = $"select id as {nameof(RoleDto.Id)}, display_name as {nameof(RoleDto.DisplayName)} from role where id = @id";
            var role = RoleMapper.Map(await _context.QuerySingleOrDefaultAsync<RoleDto>(sql, new { id = id }));

            var claims = await GetRoleClaims(id);
            foreach (var claim in claims)
                role.AddClaim(claim);

            return role;
        }

        public async Task UpdateAsync(Role role)
        {
            var claims = await GetRoleClaims(role.Id);

            var claimsToAdd = role.Claims.Where(a => !claims.Any(b => b.Type == a.Type && b.Value == a.Value));
            var claimsToRemove = claims.Where(a => !role.Claims.Any(b => b.Type == a.Type && b.Value == a.Value));

            var commands = new Dictionary<string, object>();

            foreach (var command in GetAddClaimCommands(claimsToAdd, role.Id))
                commands.Add(command.Key, command.Value);

            foreach (var command in GetRemoveClaimCommands(claimsToRemove, role.Id))
                commands.Add(command.Key, command.Value);

            foreach (var command in commands)
                _context.Execute(command.Key, command.Value, new List<INotification>());

            var sql = "update role set display_name = @display_name where id = @id";
            _context.Execute(sql, new { id = role.Id, display_name = role.DisplayName }, new List<INotification>());
        }

        private async Task<IEnumerable<Claim>> GetRoleClaims(string roleId)
        {
            var sql = $"select claim_type as {nameof(ClaimDto.Type)}, claim_value as {nameof(ClaimDto.Value)} where role_id = @role_id";
            var dtos = await _context.QueryAsync<ClaimDto>(sql, new { role_id = roleId });
            var claims = new List<Claim>();

            foreach (var claim in dtos)
                claims.Add(ClaimMapper.Map(claim));

            return claims;
        }

        private IDictionary<string, object> GetAddClaimCommands(IEnumerable<Claim> claims, string roleId)
        {
            var commands = new Dictionary<string, object>();

            foreach (var claim in claims)
            {
                var text = $@"
                    insert into role_claim (claim_type, claim_value, role_id) 
                    values (@claim_type, @claim_value, @role_id)";
                commands.Add(text, new { claim_type = claim.Type, claim_value = claim.Value, role_id = roleId });
            }

            return commands;
        }

        private IDictionary<string, object> GetRemoveClaimCommands(IEnumerable<Claim> claims, string roleId)
        {
            var commands = new Dictionary<string, object>();

            foreach (var claim in claims)
            {
                var text = $@"
                    delete from role_claim 
                    where claim_type = @claim_type and claim_value = @claim_value and user_id = @role_id";
                commands.Add(text, new { claim_type = claim.Type, claim_value = claim.Value, role_id = roleId });
            }

            return commands;
        }
    }
}
