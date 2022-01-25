namespace Ordering.Application.Services;

using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync(IUnitOfWork unitOfWork);
    Guid TransactionId { get; }
    bool Active { get; }
    Task BeginAsync();
}
