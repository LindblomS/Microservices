namespace Catalog.Contracts.Commands;

using MediatR;

public record CreateItemCommand(
    string Name,
    string Description,
    decimal Price,
    int Type,
    int Brand,
    int AvailableStock) : IRequest<bool>;
