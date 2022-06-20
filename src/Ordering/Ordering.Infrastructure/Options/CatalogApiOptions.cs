namespace Ordering.Infrastructure.Options;

using System;

public class CatalogApiOptions
{
    public string BaseAddress { get; set; }

    public string GetItemUri(Guid productId) => $"{BaseAddress}/{productId}";

}
