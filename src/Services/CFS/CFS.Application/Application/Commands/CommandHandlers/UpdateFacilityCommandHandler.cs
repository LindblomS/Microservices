using CFS.Application.Application.Commands.Commands;
using CFS.Domain.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Application.Application.Commands.CommandHandlers
{
    public class UpdateFacilityCommandHandler : IRequestHandler<UpdateFacilityCommand, bool>
    {
        private readonly IFacilityRepository _repository;
        private readonly ILogger<UpdateFacilityCommandHandler> _logger;
        private readonly IMediator _mediator;

        public UpdateFacilityCommandHandler(IFacilityRepository repository, ILogger<UpdateFacilityCommandHandler> logger, IMediator mediator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(UpdateFacilityCommand request, CancellationToken cancellationToken)
        {
            var address = new Address(
                request.Address.Street,
                request.Address.City,
                request.Address.State,
                request.Address.Country,
                request.Address.ZipCode);

            var facility = new Facility(request.FacilityId, request.CustomerId, request.FacilityName, address);

            _logger.LogInformation("----- Updating facility - FacilityId: {facilityId}", facility.Id);

            try
            {
                await _repository.Update(facility);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR updating facility - FacilityId: {facilityId} from {AppName}", Program.AppName, facility.Id);
                return false;
            }
        }
    }
}
