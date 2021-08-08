namespace Identity.API.Application.Handlers.CommandHandlers
{
    using Identity.API.Application.Factories;
    using MediatR;
    using Services.Identity.Contracts.Commands;
    using Services.Identity.Contracts.Results;
    using Services.Identity.Domain.AggregateModels.Role;
    using Services.Identity.Domain.AggregateModels.User;
    using Services.Identity.Domain.ValueObjects;
    using Services.Identity.Infrastructure;
    using Services.Identity.Infrastructure.Idempotency;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, CommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly CustomContext _context;

        public UpdateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, CustomContext context)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<CommandResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            SqlTransaction transaction = null;
            if (!_context.HasActiveTransaction())
                transaction = await _context.BeginTransactionAsync();

            var user = await _userRepository.GetAsync(request.Id);

            UpdateClaims(request, user);
            await UpdateRoles(request, user);

            await _userRepository.UpdateAsync(user);
            _ = await _userRepository.UnitOfWork.SaveChangesAsync();

            if (!(transaction is null))
                await _context.CommitTransactionAsync(transaction);

            return ResultFactory.CreateSuccessResult();
        }

        private async Task UpdateRoles(UpdateUserCommand request, User user)
        {
            var roles = await _roleRepository.GetAsync(request.Roles);

            var rolesToRemove = user.Roles.Where(userRole => !roles.Any(r => r.Id == userRole.Id));

            foreach (var role in rolesToRemove.ToList())
                user.RemoveRole(role);

            var rolesToAdd = roles.Where(r => !user.Roles.Any(userRole => userRole.Id == r.Id));

            foreach (var role in rolesToAdd)
                user.AddRole(role);
        }

        private void UpdateClaims(UpdateUserCommand request, User user)
        {
            var claims = GetClaims(request);

            var claimsToRemove = user.Claims.Except(claims);

            foreach (var claim in claimsToRemove.ToList())
                user.RemoveClaim(claim);

            var claimsToAdd = claims.Except(user.Claims);

            foreach (var claim in claimsToAdd)
                user.AddClaim(claim);
        }

        private IEnumerable<Claim> GetClaims(UpdateUserCommand request)
        {
            var claims = new List<Claim>();

            foreach (var claim in request.Claims)
                claims.Add(new(claim.Type, claim.Value));

            return claims;
        }
    }

    public class IdentifiedUpdateUserCommandHandler : IdentifiedCommandHandler<UpdateUserCommand, CommandResult>
    {
        public IdentifiedUpdateUserCommandHandler(IRequestManager requestManager, IMediator mediator) : base(requestManager, mediator)
        {
        }

        protected override CommandResult CreateResultForDuplicateRequest()
        {
            return ResultFactory.CreateFailureResult<CommandResult>("Duplicate command");
        }
    }
}
