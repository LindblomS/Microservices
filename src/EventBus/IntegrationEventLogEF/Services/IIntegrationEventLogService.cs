namespace EventBus.IntegrationEventLogEF
{
    using Microsoft.EntityFrameworkCore.Storage;
    using global::EventBus.EventBus.Events;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IIntegrationEventLogService
    {
        Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId);
        Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction);
        Task MarkEventAsPublishedAsync(string eventId);
        Task MarkEventAsInProgressAsync(string eventId);
        Task MarkEventAsFailedAsync(string eventId);
    }
}
