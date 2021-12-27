namespace Ordering.Application.Commands;

using Domain.AggregateModels.Order;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class CreateOrderDraftCommandHandler : IRequestHandler<CreateOrderDraftCommand, CreateOrderDraftCommand.Draft>
{
    public Task<CreateOrderDraftCommand.Draft> Handle(CreateOrderDraftCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CreateOrderDraftCommand.Draft(
            request.OrderItems, 
            Order.GetDraftTotal(request.OrderItems.Select(i => Map(i)))));
    }

    OrderItem Map(CreateOrderDraftCommand.OrderItem item)
    {
        return new(item.ProductId, new(item.ProductName), new(item.UnitPrice), new(item.Units));
    }
}
