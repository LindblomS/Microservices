namespace Order.Application.Commands;

using MediatR;

internal class CancelOrderCommand : IRequest<bool>
{
}