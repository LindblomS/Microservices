namespace Order.Application.Services;

using System;
using System.Threading.Tasks;

public interface IRequestManager
{
    Task<bool> ExistsAsync(Guid id);
    Task CreateRequestAsync(Guid guid);
}
