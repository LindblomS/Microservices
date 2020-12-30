namespace Services.Customer.API.Application.Handlers.CommandHandlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Customer.API.Application.Commands;
    using Services.Customer.Infrastructure.Idempotency;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
        where T : IRequest<R>
    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;
        private readonly ILogger<IdentifiedCommandHandler<T, R>> _logger;

        public IdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<T, R>> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected virtual R CreateResultForDuplicateRequest()
        {
            return default(R);
        }

        public async Task<R> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistAsync(message.Id);
            if (alreadyExists)
            {
                return CreateResultForDuplicateRequest();
            }
            else
            {
                await _requestManager.CreateRequestForCommandAsync<T>(message.Id);
                try
                {
                    var command = message.Command;
                    var commandName = typeof(CreateCustomerCommand).Name;
                    var idProperty = string.Empty;
                    var commandId = string.Empty;

                    switch (command)
                    {
                        case CreateCustomerCommand createCustomerCommand:
                            idProperty = nameof(createCustomerCommand.Name);
                            commandId = createCustomerCommand.Name;
                            break;

                        default:
                            idProperty = "Id?";
                            commandId = "n/a";
                            break;
                    }

                    _logger.LogInformation(
                        "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        commandName,
                        idProperty,
                        commandId,
                        command);

                    var result = await _mediator.Send(command, cancellationToken);

                    _logger.LogInformation(
                        "----- Command result: {@Result} - {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        result,
                        commandName,
                        idProperty,
                        commandId,
                        command);

                    return result;
                }
                catch
                {
                    return default(R);
                }
            }
        }
    }
}
