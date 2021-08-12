namespace Services.User.API.Application.Handlers.CommandHandlers
{
    using MediatR;
    using Services.User.API.Application.Factories;
    using Services.User.API.Application.Models.Commands;
    using Services.User.API.Application.Models.Results;
    using Services.User.Domain.AggregateModels.Role;
    using Services.User.Domain.ValueObjects;
    using Services.User.Infrastructure;
    using Services.User.Infrastructure.Idempotency;
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
            UpdateClaims(request, role);

            if (role.DisplayName != request.DisplayName)
                role.ChangeDisplayName(request.DisplayName);

            await _roleRepository.UpdateAsync(role);
            _ = await _roleRepository.UnitOfWork.SaveChangesAsync();

            if (!(transaction is null))
                await _context.CommitTransactionAsync(transaction);

            return ResultFactory.CreateSuccessResult();
        }

        private void UpdateClaims(UpdateRoleCommand request, Role role)
        {
            var claims = GetClaims(request);

            var claimsToRemove = role.Claims.Except(claims);

            foreach (var claim in claimsToRemove.ToList())
                role.RemoveClaim(claim);

            var claimsToAdd = claims.Except(role.Claims);

            foreach (var claim in claimsToAdd)
                role.AddClaim(claim);
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
