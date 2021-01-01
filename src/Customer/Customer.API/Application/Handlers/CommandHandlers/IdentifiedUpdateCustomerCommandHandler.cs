namespace Services.Customer.API.Application.Handlers.CommandHandlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Customer.API.Application.Commands;
    using Services.Customer.Infrastructure.Idempotency;

    public class IdentifiedUpdateCustomerCommandHandler : IdentifiedCommandHandler<UpdateCustomerCommand, bool>
    {
        public IdentifiedUpdateCustomerCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<UpdateCustomerCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
