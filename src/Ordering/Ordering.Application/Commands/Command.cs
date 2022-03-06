namespace Ordering.Application.Commands;

using MediatR;

public abstract record Command<TResponse> : IRequest<TResponse>; 