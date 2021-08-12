﻿namespace Services.User.API.Application.Handlers.CommandHandlers
{
    using MediatR;
    using Services.User.API.Application.Factories;
    using Services.User.API.Application.Models.Commands;
    using Services.User.API.Application.Models.Results;
    using Services.User.Domain.AggregateModels.Role;
    using Services.User.Domain.AggregateModels.User;
    using Services.User.Domain.ValueObjects;
    using Services.User.Infrastructure;
    using Services.User.Infrastructure.Idempotency;
    using System;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly CustomContext _context;

        public CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, CustomContext context)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CommandResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            SqlTransaction transaction = null;
            if (!_context.HasActiveTransaction())
                transaction = await _context.BeginTransactionAsync();

            var user = new User(request.Username, request.PasswordHash);

            foreach (var claim in request.Claims)
                user.AddClaim(new Claim(claim.Type, claim.Value));

            var roles = await _roleRepository.GetAsync(request.Roles);
            foreach (var role in roles)
                user.AddRole(role);

            _userRepository.Create(user);
            _ = await _userRepository.UnitOfWork.SaveChangesAsync();

            if (!(transaction is null))
                await _context.CommitTransactionAsync(transaction);

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