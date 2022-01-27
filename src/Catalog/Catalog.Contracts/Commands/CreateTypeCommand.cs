namespace Catalog.Contracts.Commands;

using MediatR;

public record CreateTypeCommand(string Type) : IRequest<bool>;
