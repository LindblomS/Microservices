﻿namespace Services.User.Infrastructure.Repositories
{
    using MediatR;
    using Services.User.Domain.AggregateModels.Role;
    using Services.User.Domain.AggregateModels.User;
    using Services.User.Domain.SeedWork;
    using Services.User.Domain.ValueObjects;
    using Services.User.Infrastructure.Mappers;
    using Services.User.Infrastructure.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserRepository : IUserRepository
    {
        private readonly CustomContext _context;

        public UserRepository(CustomContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Create(User user)
        {
            var commands = new List<Command>();
            var sql = $@"insert into [user] (id, username, password_hash) values (@id, @username, @password)";
            commands.Add(new(sql, new { id = user.Id, username = user.Username, password = user.PasswordHash }, new List<INotification>()));

            foreach (var command in GetAddClaimCommands(user.Claims, user.Id))
                commands.Add(command);

            foreach (var command in GetAddRoleCommands(user.Roles, user.Id))
                commands.Add(command);

            foreach (var command in commands)
                _context.Execute(command);
        }

        public async Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync(Guid id)
        {
            var sql = $"select id as {nameof(UserDto.id)}, username as {nameof(UserDto.username)}, password_hash as {nameof(UserDto.passwordHash)} from [user] where id = @id";
            var user = UserMapper.Map(await _context.QuerySingleOrDefaultAsync<UserDto>(sql, new { id = id }));

            var claims = await GetUserClaims(id);
            foreach (var claim in claims)
                user.AddClaim(claim);

            var roles = await GetUserRoles(id);
            foreach (var role in roles)
                user.AddRole(role);

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var claims = await GetUserClaims(user.Id);
            var roles = await GetUserRoles(user.Id);

            var claimsToAdd = user.Claims.Where(a => !claims.Any(b => b.Type == a.Type && b.Value == a.Value));
            var claimsToRemove = claims.Where(a => !user.Claims.Any(b => b.Type == a.Type && b.Value == a.Value));

            var rolesToAdd = user.Roles.Where(a => !roles.Any(b => b.Id == a.Id));
            var rolesToRemove = roles.Where(a => !user.Roles.Any(b => b.Id == a.Id));

            var commands = new List<Command>();

            foreach (var command in GetAddClaimCommands(claimsToAdd, user.Id))
                commands.Add(command);

            foreach (var command in GetRemoveClaimCommands(claimsToRemove, user.Id))
                commands.Add(command);

            foreach (var command in GetAddRoleCommands(rolesToAdd, user.Id))
                commands.Add(command);

            foreach (var command in GetRemoveRoleCommands(rolesToRemove, user.Id))
                commands.Add(command);

            foreach (var command in commands)
                _context.Execute(command);
        }

        private List<Command> GetAddClaimCommands(IEnumerable<Claim> claims, Guid userId)
        {
            var commands = new List<Command>();

            foreach (var claim in claims)
            {
                var sql = $@"
                    insert into user_claim (claim_type, claim_value, user_id) 
                    values (@claim_type, @claim_value, @user_id)";
                commands.Add(new(sql, new { claim_type = claim.Type, claim_value = claim.Value, user_id = userId }, new List<INotification>()));
            }

            return commands;
        }

        private List<Command> GetRemoveClaimCommands(IEnumerable<Claim> claims, Guid userId)
        {
            var commands = new List<Command>();

            foreach (var claim in claims)
            {
                var sql = $@"
                    delete from user_claim 
                    where claim_type = @claim_type and claim_value = @claim_value and user_id = @user_id";
                commands.Add(new(sql, new { claim_type = claim.Type, claim_value = claim.Value, user_id = userId }, new List<INotification>()));
            }

            return commands;
        }

        private List<Command> GetAddRoleCommands(IEnumerable<Role> roles, Guid userid)
        {
            var commands = new List<Command>();

            foreach (var role in roles)
            {
                var text = $@"insert into user_role (user_id, role_id) values (@user_id, @role_id)";
                commands.Add(new(text, new { user_id = userid, role_id = role.Id }, new List<INotification>()));
            }

            return commands;
        }

        private List<Command> GetRemoveRoleCommands(IEnumerable<Role> roles, Guid userId)
        {
            var commands = new List<Command>();

            foreach (var role in roles)
            {
                var text = $@"delete from user_role where user_id = @user_id and role_id = @role_id";
                commands.Add(new(text, new { user_id = userId, role_id = role.Id }, new List<INotification>()));
            }

            return commands;
        }

        private async Task<IEnumerable<Claim>> GetUserClaims(Guid userId)
        {
            var sql = $"select claim_type as {nameof(ClaimDto.type)}, claim_value as {nameof(ClaimDto.value)} from user_claim where user_id = @user_id";
            var dtos = await _context.QueryAsync<ClaimDto>(sql, new { user_id = userId });
            var claims = new List<Claim>();

            foreach (var claim in dtos)
                claims.Add(ClaimMapper.Map(claim));

            return claims;
        }

        private async Task<IEnumerable<Role>> GetUserRoles(Guid userId)
        {
            var sql = $@"
                select 
                    role_id as {nameof(RoleDto.id)}, 
                    display_name as {nameof(RoleDto.displayName)} 
                from user_role 
                join [role] on [role].id = user_role.role_id
                where user_id = @user_id";

            var dtos = await _context.QueryAsync<RoleDto>(sql, new { user_id = userId });
            var roles = new List<Role>();

            foreach (var role in dtos)
                roles.Add(RoleMapper.Map(role));

            return roles;
        }
    }
}