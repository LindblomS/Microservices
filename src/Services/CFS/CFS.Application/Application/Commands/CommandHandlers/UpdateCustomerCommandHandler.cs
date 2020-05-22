using CFS.Application.Application.Commands.Commands;
using CFS.Domain.Aggregates.CustomerAggregate;
using CFS.Domain.Aggregates.SharedValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Application.Application.Commands.CommandHandlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<UpdateCustomerCommandHandler> _logger;
        private readonly IMediator _mediator;

        public UpdateCustomerCommandHandler(ICustomerRepository repository, ILogger<UpdateCustomerCommandHandler> logger, IMediator mediator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var address = new Address(
                request.Address.Street,
                request.Address.City,
                request.Address.State,
                request.Address.Country,
                request.Address.ZipCode);

            var customer = new Customer(request.CustomerId, request.FirstName, request.LastName, request.PhoneNumber, request.Email, address);

            _logger.LogInformation("----- Updating customer - CustomerId: {customerId}", customer.Id);

            try
            {
                await _repository.Update(customer);
                // For implementation of integrationevents
                // _mediator.Publish(integrationevent) 
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR updating customer - CustomerId: {customerId} from {AppName}", Program.AppName, customer.Id);
                return false;
            }
        }
    }
}
