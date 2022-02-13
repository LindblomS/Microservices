namespace Catalog.Contracts.Commands;

using MediatR;

public record CreateBrandCommand(string Brand) : IRequest<bool>;
