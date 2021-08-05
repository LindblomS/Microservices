namespace Identity.API.Application.Handlers.CommandHandlers
{
    using Identity.API.Application.Factories;
    using MediatR;
    using Services.Identity.Contracts.Commands;
    using Services.Identity.Contracts.Results;
    using Services.Identity.Domain.AggregateModels.Role;
    using Services.Identity.Infrastructure;
    using Services.Identity.Infrastructure.Idempotency;
    using System;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateRoleCommandHandler : IPipelineBehavior<CreateRoleCommand, CommandResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly CustomContext _context;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, CustomContext context)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CommandResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<CommandResult> next)
        {
            SqlTransaction transaction = null;

            if (!_context.HasActiveTransaction())
                transaction = await _context.BeginTransactionAsync();

            var role = new Role(request.Id, request.DisplayName);
            foreach (var claim in request.Claims)
                role.AddClaim(new(claim.Type, claim.Value));

            _roleRepository.Create(role);

            if (!(transaction is null))
                await _context.CommitTransactionAsync(transaction);

            return ResultFactory.CreateSuccessResult();
        }
    }

    public class IdentifiedCreateRoleCommandHandler : IdentifiedCommandHandler<CreateRoleCommand, CommandResult>
    {
        public IdentifiedCreateRoleCommandHandler(IRequestManager requestManager, IMediator mediator) : base(requestManager, mediator)
        {
        }

        protected override CommandResult CreateResultForDuplicateRequest()
        {
            return ResultFactory.CreateFailureResult<CommandResult>("Duplicate command");
        }
    }
}
