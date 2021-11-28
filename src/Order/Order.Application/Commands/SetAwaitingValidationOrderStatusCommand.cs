namespace Order.Application.Commands;

using MediatR;

internal class SetAwaitingValidationOrderStatusCommand : IRequest<bool>
{
}
