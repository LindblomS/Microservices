namespace Catalog.Infrastructure.Services;

using Catalog.Application.Services;
using Catalog.Infrastructure.EntityFramework;
using System;

public class ValidationService : IValidationService
{
    readonly CatalogContext context;

    public ValidationService(CatalogContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public bool BrandExists(string brand)
    {
        return context.Brands.Find(brand) is not null;
    }

    public bool ItemExists(Guid id)
    {
        return context.Items.Find(id.ToString()) is not null;
    }

    public bool TypeExists(string type)
    {
        return context.Types.Find(type) is not null;
    }
}
