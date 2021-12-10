namespace Ordering.Application.Services;

using System.Threading.Tasks;

public interface IUnitOfWork
{
    Task Commit(Guid unitOfWorkId);
    Guid Id { get; }
    bool Active { get; }
    void Begin();
}
