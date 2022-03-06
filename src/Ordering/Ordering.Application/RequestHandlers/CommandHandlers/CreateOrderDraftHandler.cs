namespace Ordering.Application.RequestHandlers.CommandHandlers;

using Domain.AggregateModels.Order;
using MediatR;
using Ordering.Application.Commands;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class CreateOrderDraftHandler : IRequestHandler<CreateOrderDraft, CreateOrderDraft.Draft>
{
    public Task<CreateOrderDraft.Draft> Handle(CreateOrderDraft request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CreateOrderDraft.Draft(
            request.OrderItems, 
            Order.GetDraftTotal(request.OrderItems.Select(i => Map(i)))));
    }

    OrderItem Map(CreateOrderDraft.OrderItem item)
    {
        return new(item.ProductId, new(item.ProductName), new(item.UnitPrice), new(item.Units));
    }
}
