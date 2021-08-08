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
            var claims = GetClaims(request);
            var roles = await _roleRepository.GetAsync(request.Roles);

            foreach (var claim in user.Claims.Where(userClaim => claims.Any(c => c.Type != userClaim.Type && c.Value != userClaim.Value)))
                user.RemoveClaim(claim);

            foreach (var claim in claims.Where(c => user.Claims.Any(userClaim => userClaim.Type != c.Type && userClaim.Value != c.Value)))
                user.AddClaim(claim);

            foreach (var role in user.Roles.Where(userRole => roles.Any(r => r.Id != userRole.Id)))
                user.RemoveRole(role);

            foreach (var role in roles.Where(r => user.Roles.Any(userRole => userRole.Id != r.Id)))
                user.AddRole(role);

            await _userRepository.UpdateAsync(user);
            _ = await _userRepository.UnitOfWork.SaveChangesAsync();

            if (!(transaction is null))
                await _context.CommitTransactionAsync(transaction);

            return ResultFactory.CreateSuccessResult();
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
