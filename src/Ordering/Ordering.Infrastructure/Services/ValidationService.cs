namespace Ordering.Infrastructure.Services;

using Ordering.Application.Services;
using Ordering.Infrastructure.EntityFramework;
using System;

public class ValidationService : IValidationService
{
    readonly OrderingContext context;

    public ValidationService(OrderingContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public bool OrderExists(Guid id)
    {
        return context.Orders.Find(id.ToString()) is not null;
    }
}
