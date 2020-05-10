using CFS.Application.Application.Commands.Commands;
using CFS.Domain.Aggregates.CustomerAggregate;
using CFS.Domain.Aggregates.SharedValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Application.Application.Commands.CommandHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(ICustomerRepository repository, IMediator mediator, ILogger<CreateCustomerCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var address = new Address(
                request.Address.Street, 
                request.Address.City, 
                request.Address.State, 
                request.Address.Country, 
                request.Address.ZipCode);

            var customer = new Customer(-1, request.FirstName, request.LastName, request.PhoneNumber, request.Email, address);

            _logger.LogInformation("----- Creating Order - Order: {order}", customer);

            IDbTransaction transaction = null;

            while (!_repository.UnitOfWork.HasActiveTransaction())
            {
                 transaction = _repository.UnitOfWork.BeginTransaction();
            }

            await _repository.Add(customer);
            return await _repository.UnitOfWork.CommitTransaction(transaction);
        }
    }
}
