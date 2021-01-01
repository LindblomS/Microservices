namespace Services.Customer.API.Application.Handlers.CommandHandlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Customer.API.Application.Commands;
    using Services.Customer.Domain;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ILogger<DeleteCustomerCommandHandler> _logger;
        private readonly ICustomerRepository _repository;

        public DeleteCustomerCommandHandler(ILogger<DeleteCustomerCommandHandler> logger, ICustomerRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("----- Deleting Customer - CustomerId: {@CustomerId}", request.Id);
            var customer = await _repository.GetAsync(request.Id);

            if (customer != null)
            {
                customer.Delete();
                await _repository.DeleteAsync(customer);
            }
            else
            {
                _logger.LogWarning("----- Customer - CustomerId: {@CustomerID} was not found");
                return false;
            }
            return await _repository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
