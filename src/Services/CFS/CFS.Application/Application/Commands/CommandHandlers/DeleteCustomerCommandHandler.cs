using CFS.Application.Application.Commands.Commands;
using CFS.Domain.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Application.Application.Commands.CommandHandlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public DeleteCustomerCommandHandler(ICustomerRepository repository, ILogger<CreateCustomerCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("----- Deleting customer - Customer: {customerId}", request.CustomerId);

            try
            {
                await _repository.Delete(request.CustomerId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR deleting customer - CustomerId: {CustomerId} from {AppName}", request.CustomerId, Program.AppName);
                return false;
            }
        }
    }
}
