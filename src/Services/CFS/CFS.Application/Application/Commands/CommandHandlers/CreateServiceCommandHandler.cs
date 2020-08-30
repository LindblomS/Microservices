using CFS.Application.Application.Commands.Commands;
using CFS.Domain.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Application.Application.Commands.CommandHandlers
{
    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, bool>
    {
        private readonly IServiceRepository _repository;
        private readonly ILogger<CreateServiceCommandHandler> _logger;

        public CreateServiceCommandHandler(IServiceRepository repository, ILogger<CreateServiceCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = new Service(-1, request.FacilityId, request.StartDate, request.StopDate);

            _logger.LogInformation("----- Creating service - Service: {service}", service);

            try
            {
                await _repository.Add(service);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR creating service: {AppName} - ({@Service})", Program.AppName, service);
                return false;
            }
        }
    }
}
