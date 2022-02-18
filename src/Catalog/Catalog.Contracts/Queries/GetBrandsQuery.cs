namespace Catalog.Contracts.Queries;

using MediatR;
using System.Collections.Generic;

public record GetBrandsQuery() : IRequest<IEnumerable<string>>;


