using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Ordering.API.Application.Commands.CreateOrderCommand;

namespace Ordering.API.Application.Commands.CommandHandlers
{
    public class CreateOrderDraftCommandHandler : IRequestHandler<CreateOrderDraftCommand, OrderDraftDTO>
    {
        public CreateOrderDraftCommandHandler()
        {

        }

        public Task<OrderDraftDTO> Handle(CreateOrderDraftCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class OrderDraftDTO
    {
        public IEnumerable<OrderItemDTO> OrderItems { get; set; }
        public decimal Total { get; set; }

        public static OrderDraftDTO FromOrder(Order order)
        {
            return new OrderDraftDTO()
            {
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    Discount = oi.GetCurrentDiscount(),
                    ProductId = oi.ProductId,
                    UnitPrice = oi.GetUnitPrice(),
                    PictureUrl = oi.GetPictureUri(),
                    Units = oi.GetUnits(),
                    ProductName = oi.GetProductName()
                }),
                Total = order.GetTotal()
            };
        }

    }
}
