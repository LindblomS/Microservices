namespace Identity.API.Application.Handlers.CommandHandlers
{
    using Identity.API.Application.Factories;
    using MediatR;
    using Services.Identity.Contracts.Commands;
    using Services.Identity.Contracts.Results;
    using Services.Identity.Domain.AggregateModels.Role;
    using Services.Identity.Domain.ValueObjects;
    using Services.Identity.Infrastructure;
    using Services.Identity.Infrastructure.Idempotency;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, CommandResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly CustomContext _context;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository, CustomContext context)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<CommandResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            SqlTransaction transaction = null;
            if (!_context.HasActiveTransaction())
                transaction = await _context.BeginTransactionAsync();

            var role = await _roleRepository.GetAsync(request.Id);
            var claims = GetClaims(request);

            foreach (var claim in role.Claims.Where(roleClaim => claims.Any(c => c.Type != roleClaim.Type && c.Value != roleClaim.Value)).ToList())
                role.RemoveClaim(claim);

            foreach (var claim in claims.Where(c => !(role.Claims.Any(roleClaim => roleClaim.Type == c.Type && roleClaim.Value == c.Value))))
                role.AddClaim(claim);

            if (role.DisplayName != request.DisplayName)
                role.ChangeDisplayName(request.DisplayName);

            await _roleRepository.UpdateAsync(role);
            _ = await _roleRepository.UnitOfWork.SaveChangesAsync();

            if (!(transaction is null))
                await _context.CommitTransactionAsync(transaction);

            return ResultFactory.CreateSuccessResult();
        }

        private IEnumerable<Claim> GetClaims(UpdateRoleCommand request)
        {
            var claims = new List<Claim>();
            foreach (var claim in request.Claims)
                claims.Add(new(claim.Type, claim.Value));

            return claims;
        }
    }

    public class IdentifiedUpdateRoleCommandHandler : IdentifiedCommandHandler<UpdateRoleCommand, CommandResult>
    {
        public IdentifiedUpdateRoleCommandHandler(IRequestManager requestManager, IMediator mediator) : base(requestManager, mediator)
        {
        }

        protected override CommandResult CreateResultForDuplicateRequest()
        {
            return ResultFactory.CreateFailureResult<CommandResult>("Duplicate command");
        }
    }
}
