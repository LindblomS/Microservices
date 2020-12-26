namespace Services.Customer.Infrastructure
{
    using MediatR;
    using Services.Customer.Domain.SeedWork;
    using System.Linq;
    using System.Threading.Tasks;

    public static class MediatorExtensions
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, CustomerContext context)
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
