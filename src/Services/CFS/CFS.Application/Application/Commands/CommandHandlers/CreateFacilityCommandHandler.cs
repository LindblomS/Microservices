using CFS.Application.Application.Commands.Commands;
using CFS.Domain.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Application.Application.Commands.CommandHandlers
{
    public class CreateFacilityCommandHandler : IRequestHandler<CreateFacilityCommand, bool>
    {
        private readonly IFacilityRepository _repository;
        private readonly ILogger<CreateFacilityCommandHandler> _logger;

        public CreateFacilityCommandHandler(IFacilityRepository repository, ILogger<CreateFacilityCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateFacilityCommand request, CancellationToken cancellationToken)
        {
            var address = new Address(
                request.Address.Street,
                request.Address.City,
                request.Address.State,
                request.Address.Country,
                request.Address.ZipCode);

            var facility = new Facility(-1, request.CustomerId, request.FacilityName, address);

            _logger.LogInformation("----- Creating facility - Facility: {facility}", facility);

            try
            {
                await _repository.Add(facility);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR creating facility: {AppName} - ({@Facility})", Program.AppName, facility);
                return false;
            }
        }
    }
}
