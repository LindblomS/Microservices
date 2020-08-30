using CFS.Application.Application.Commands.Commands;
using CFS.Domain.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Application.Application.Commands.CommandHandlers
{
    public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, bool>
    {
        private readonly IServiceRepository _repository;
        private readonly ILogger<UpdateServiceCommandHandler> _logger;
        private readonly IMediator _mediator;

        public UpdateServiceCommandHandler(IServiceRepository repository, ILogger<UpdateServiceCommandHandler> logger, IMediator mediator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = new Service(request.ServiceId, request.FacilityId, request.StartDate, request.StopDate);

            _logger.LogInformation("----- Updating service - ServiceId: {serviceId}", service.Id);

            try
            {
                await _repository.Update(service);
                // For implementation of integrationevents
                // _mediator.Publish(integrationevent) 
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR updating service - ServiceId: {serviceId} from {AppName}", Program.AppName, service.Id);
                return false;
            }
        }
    }
}
