using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Idempotency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Extensions;

namespace Ordering.API.Application.Commands.CommandHandlers
{
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R> where T : IRequest<R>
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

        /// <summary>
        /// Creates the result value to return if a previous request was found
        /// </summary>
        protected virtual R CreateResultForDuplicateRequest()
        {
            return default(R);
        }

        /// <summary>
        /// This method handles the command. It just ensures that no other request exists with the same ID, and if this is the case
        /// just enqueues the original inner command.
        /// </summary>
        /// <param name="message">IdentifiedCommand which contains both original command & request ID</param>
        /// <returns>Return value of inner command or default value if request same ID was found</returns>
        public async Task<R> Handle(IdentifiedCommand<T, R> request, CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistsAsync(request.Id);
            if (alreadyExists)
                return CreateResultForDuplicateRequest();

            else
            {
                await _requestManager.CreateRequestforCommandAsync<T>(request.Id);
                try
                {
                    var command = request.Command;
                    var commandName = command.GetGenericTypeName();
                    var idProperty = string.Empty;
                    var commandId = string.Empty;

                    switch (command)
                    {
                        case CreateOrderCommand createOrderCommand:
                            idProperty = nameof(createOrderCommand.UserId);
                            break;

                        case CancelOrderCommand cancelOrderCommand:
                            idProperty = nameof(cancelOrderCommand.OrderNumber);
                            commandId = $"{cancelOrderCommand.OrderNumber}";
                            break;

                        case ShipOrderCommand shipOrderCommand:
                            idProperty = nameof(shipOrderCommand.OrderNumber);
                            commandId = $"{shipOrderCommand.OrderNumber}";
                            break;

                        default:
                            idProperty = "Id?";
                            commandId = "n/a";
                            break;
                    }

                    _logger.LogInformation($"----- Sending command: {commandName} - {idProperty}: {commandId} ({command})");

                    // Send the embeded business command to mediator so it runs its related CommandHandler 
                    var result = await _mediator.Send(command, cancellationToken);

                    _logger.LogInformation($"----- Command result: {result} - {commandName} - {idProperty}: {commandId} ({command})");

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
