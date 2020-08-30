using CFS.Application.Application.Commands.Commands;
using CFS.Domain.Aggregates;
using CFS.Domain.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Application.Application.Commands.CommandHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCustomerCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var address = new Address(
                request.Address.Street, 
                request.Address.City, 
                request.Address.State, 
                request.Address.Country, 
                request.Address.ZipCode);

            var customer = new Customer(-1, request.FirstName, request.LastName, request.PhoneNumber, request.Email, address);

            _logger.LogInformation("----- Creating customer - Customer: {customer}", customer);

            try
            {
                _unitOfWork.BeginTransaction();
                await _repository.Add(customer);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR creating customer: {AppName} - ({@Customer})", Program.AppName, customer);
                return false;
            }
        }
    }
}
