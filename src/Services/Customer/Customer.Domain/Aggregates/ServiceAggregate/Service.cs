using CFS.Domain.SeedWork;

namespace CFS.Domain.Aggregates.ServiceAggregate
{
    public class Service : Entity, IAggregateRoot
    {
        public Service(int id)
        {
            Id = id;
        }
    }
}
