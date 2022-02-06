namespace Catalog.Contracts.Commands;

using MediatR;
using System;

public record DeleteItemCommand(Guid id) : IRequest<bool>;
