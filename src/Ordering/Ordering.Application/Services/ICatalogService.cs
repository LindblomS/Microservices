namespace Ordering.Application.Services;
using Catalog.Contracts.Queries;
using System;
using System.Collections.Generic;

public interface ICatalogService
{
    Task<IReadOnlyCollection<Item>> GetAsync(IEnumerable<Guid> productIds);
}
