namespace Ordering.Infrastructure.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OrderingContext : DbContext, IUnitOfWork
{
    public Guid Id => throw new NotImplementedException();

    public bool Active => throw new NotImplementedException();

    public void Begin()
    {
        throw new NotImplementedException();
    }

    public Task Commit(Guid unitOfWorkId)
    {
        throw new NotImplementedException();
    }

    public IDbContextTransaction GetCurrentTransaction() => throw new NotImplementedException();
}
