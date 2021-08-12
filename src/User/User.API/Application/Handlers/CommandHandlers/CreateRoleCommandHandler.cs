﻿namespace Services.User.API.Application.Handlers.CommandHandlers
{
    using MediatR;
    using Services.User.API.Application.Factories;
    using Services.User.API.Application.Models.Commands;
    using Services.User.API.Application.Models.Results;
    using Services.User.Domain.AggregateModels.Role;
    using Services.User.Infrastructure;
    using Services.User.Infrastructure.Idempotency;
    using System;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CommandResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly CustomContext _context;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, CustomContext context)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CommandResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            SqlTransaction transaction = null;

            if (!_context.HasActiveTransaction())
                transaction = await _context.BeginTransactionAsync();

            var role = new Role(request.Id, request.DisplayName);
            foreach (var claim in request.Claims)
                role.AddClaim(new(claim.Type, claim.Value));

            _roleRepository.Create(role);
            _ = await _roleRepository.UnitOfWork.SaveChangesAsync();

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