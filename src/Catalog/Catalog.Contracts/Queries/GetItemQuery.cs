namespace Catalog.Contracts.Queries;

using MediatR;

public record GetItemQuery(Guid id) : IRequest<Item>;


