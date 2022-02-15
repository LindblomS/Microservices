namespace Catalog.Contracts.Queries;

using MediatR;
using System.Collections.Generic;

public record GetTypesQuery() : IRequest<IEnumerable<string>>;


