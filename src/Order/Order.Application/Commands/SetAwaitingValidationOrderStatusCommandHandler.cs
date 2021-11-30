namespace Ordering.Application.Commands;

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

internal class SetAwaitingValidationOrderStatusCommandHandler : IRequestHandler<SetAwaitingValidationOrderStatusCommand, bool>
{
    public Task<bool> Handle(SetAwaitingValidationOrderStatusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
