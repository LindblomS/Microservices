namespace Services.Customer.API.Application.Handlers.CommandHandlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Customer.API.Application.Commands;
    using Services.Customer.Domain;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ILogger<UpdateCustomerCommandHandler> _logger;
        private readonly ICustomerRepository _repository;

        public UpdateCustomerCommandHandler(ILogger<UpdateCustomerCommandHandler> logger, ICustomerRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("----- Updating Customer - CustomerId: {@CustomerId}", request.Id);
            var customer = await _repository.GetAsync(request.Id);

            if (customer != null)
            {
                customer.Name = request.Name;
                await _repository.UpdateAsync(customer);
            }
            else
            {
                throw new InvalidOperationException($"Customer was not found. CustomerId: {request.Id}");
            }
  
            return await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
