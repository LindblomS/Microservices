namespace Ordering.Application.Services;

using System.Threading.Tasks;

public interface IUnitOfWork
{
    Task CommitAsync(IUnitOfWork unitOfWork);
    Guid Id { get; }
    bool Active { get; }
    Task BeginAsync();
}
