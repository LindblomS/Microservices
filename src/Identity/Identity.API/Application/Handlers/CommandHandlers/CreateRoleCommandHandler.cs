using Identity.API.Application.Factories;
using MediatR;
using Services.Identity.Contracts.Commands;
using Services.Identity.Contracts.Results;
using Services.Identity.Domain.AggregateModels.Role;
using Services.Identity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Handlers.CommandHandlers
{
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
}
