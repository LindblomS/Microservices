namespace Identity.API.Application.Handlers.CommandHandlers
{
    using Identity.API.Application.Factories;
    using Identity.Contracts.Commands;
    using MediatR;
    using Services.Identity.Contracts.Results;
    using Services.Identity.Domain.AggregateModels.Role;
    using Services.Identity.Domain.AggregateModels.User;
    using Services.Identity.Domain.ValueObjects;
    using Services.Identity.Infrastructure.Idempotency;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<CommandResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.Username, request.PasswordHash);

            foreach (var claim in request.Claims)
                user.AddClaim(new Claim(claim.Type, claim.Value));

            var roles = await _roleRepository.GetAsync(request.Roles);
            foreach (var role in roles)
                user.AddRole(role);

            _userRepository.Create(user);

            return ResultFactory.CreateSuccessResult();
        }
    }

    public class IdentifiedCreateUserCommandHandler : IdentifiedCommandHandler<CreateUserCommand, CommandResult>
    {
        public IdentifiedCreateUserCommandHandler(IRequestManager requestManager, IMediator mediator) : base(requestManager, mediator)
        {
        }

        protected override CommandResult CreateResultForDuplicateRequest()
        {
            return ResultFactory.CreateFailureResult<CommandResult>("Duplicate command");
        }
    }
}
