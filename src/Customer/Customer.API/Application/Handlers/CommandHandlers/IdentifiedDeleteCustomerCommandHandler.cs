namespace Services.Customer.API.Application.Handlers.CommandHandlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Customer.API.Application.Commands;
    using Services.Customer.Infrastructure.Idempotency;

    public class IdentifiedDeleteCustomerCommandHandler : IdentifiedCommandHandler<DeleteCustomerCommand, bool>
    {
        public IdentifiedDeleteCustomerCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<DeleteCustomerCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
