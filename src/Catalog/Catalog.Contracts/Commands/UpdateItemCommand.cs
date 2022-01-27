namespace Catalog.Contracts.Commands;

using MediatR;

public record UpdateItemCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Type,
    string Brand,
    int AvailableStock) : IRequest<bool>;