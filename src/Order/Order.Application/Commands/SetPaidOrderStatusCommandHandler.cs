namespace Ordering.Application.Commands;

using MediatR;
using System.Threading;
using System.Threading.Tasks;

internal class SetPaidOrderStatusCommandHandler : IRequestHandler<SetPaidOrderStatusCommand, bool>
{
    public Task<bool> Handle(SetPaidOrderStatusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
