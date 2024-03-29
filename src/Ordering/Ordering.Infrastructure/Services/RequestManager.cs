﻿namespace Ordering.Infrastructure.Services;

using Ordering.Application.Services;
using Ordering.Infrastructure.EntityFramework;
using Ordering.Infrastructure.Exceptions;
using Ordering.Infrastructure.Models;
using System;
using System.Threading.Tasks;

public class RequestManager : IRequestManager
{
    readonly OrderingContext context;

    public RequestManager(OrderingContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CreateRequestAsync<T>(Guid id)
    {
        var exists = await ExistsAsync(id);

        if (exists)
            throw new RequestAlreadyExistsException(id);

        var request = new ClientRequestEntity(id.ToString(), typeof(T).Name, DateTime.UtcNow);

        context.Add(request);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var request = await context.FindAsync<ClientRequestEntity>(id.ToString());
        return request is not null;
    }
}
