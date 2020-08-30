using CFS.Application.Application.Commands.Commands;
using CFS.Domain.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Application.Application.Commands.CommandHandlers
{
    public class DeleteFacilityCommandHandler : IRequestHandler<DeleteFacilityCommand, bool>
    {
        private readonly IFacilityRepository _repository;
        private readonly ILogger<DeleteFacilityCommandHandler> _logger;

        public DeleteFacilityCommandHandler(IFacilityRepository repository, ILogger<DeleteFacilityCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("----- Deleting facility - FacilityId: {facilityId}", request.FacilityId);

            try
            {
                await _repository.Delete(request.FacilityId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR deleting facility - FacilityId: {FacilityId} from {AppName}", request.FacilityId, Program.AppName);
                return false;
            }
        }

    }
}
