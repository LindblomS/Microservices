namespace Order.Application.Commands;

using MediatR;
using System.Threading;
using System.Threading.Tasks;

internal class SetStockRejectedOrderStatusCommandHandler : IRequestHandler<SetStockRejectedOrderStatusCommand, bool>
{
    public Task<bool> Handle(SetStockRejectedOrderStatusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
