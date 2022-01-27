namespace Catalog.Contracts.Commands;

using MediatR;
using System;

public record DeleteCatalogItemCommand(Guid id) : IRequest<bool>;
