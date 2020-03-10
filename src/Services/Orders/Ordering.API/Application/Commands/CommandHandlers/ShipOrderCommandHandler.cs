using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Infrastructure.Idempotency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands.CommandHandlers
{
    public class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public ShipOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<bool> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetAsync(request.OrderNumber);
            if (orderToUpdate == null)
                return false;

            else
            {
                orderToUpdate.SetShippedStatus();
                return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
        }
    }

    public class ShipOrderIdentifiendCommandHandler : IdentifiedCommandHandler<ShipOrderCommand, bool>
    {
        public ShipOrderIdentifiendCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<ShipOrderCommand, bool>> logger) : base(mediator, requestManager, logger)
        {
        }

        // Ignore duplicate requests for processing order.
        protected override bool CreateResultForDuplicateRequest() => true;        
    }
}
