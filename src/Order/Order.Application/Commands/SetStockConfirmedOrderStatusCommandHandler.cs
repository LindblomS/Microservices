namespace Order.Application.Commands;

using MediatR;
using System.Threading;
using System.Threading.Tasks;

internal class SetStockConfirmedOrderStatusCommandHandler : IRequestHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    public Task<bool> Handle(SetStockConfirmedOrderStatusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
