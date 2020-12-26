namespace Services.Customer.API.Application.Handlers.CommandHandlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Customer.API.Application.Commands;
    using Services.Customer.Domain;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(
            ICustomerRepository repository, 
            ILogger<CreateCustomerCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer(customerId, request.Name);

            _logger.LogInformation("----- Creating Customer - Customer: {@Customer}", customer);

            await _repository.CreateAsync(customer);
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return customerId;
        }
    }
}
