using CFS.Application.Application.Commands.Commands;
using CFS.Domain.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Application.Application.Commands.CommandHandlers
{
    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, bool>
    {
        private readonly IServiceRepository _repository;
        private readonly ILogger<DeleteServiceCommandHandler> _logger;

        public DeleteServiceCommandHandler(IServiceRepository repository, ILogger<DeleteServiceCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("----- Deleting service - ServiceId: {ServiceId}", request.ServiceId);

            try
            {
                var service = await _repository.GetService(request.ServiceId);
                await _repository.Delete(service);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR deleting service - ServiceId: {ServiceId} from {AppName}", request.ServiceId, Program.AppName);
                return false;
            }
        }
    }
}
