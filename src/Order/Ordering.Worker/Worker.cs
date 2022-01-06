namespace Ordering.Worker;

using EventBus.EventBus.Abstractions;
using Microsoft.Extensions.Options;
using Ordering.Contracts.IntegrationEvents;
using System.Data.SqlClient;
using Dapper;

public class Worker : BackgroundService
{
    readonly ILogger<Worker> logger;
    readonly IEventBus eventBus;
    readonly WorkerOptions options;

    public Worker(ILogger<Worker> logger, IEventBus eventBus, IOptions<WorkerOptions> options)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            foreach (var order in await GetOrders())
            {
                await Task.Delay(1000);
                PublishIntegrationEvent(order);
            }

            await Task.Delay(options.CheckUpdateTimeSeconds * 1000, stoppingToken);
        }
    }

    void PublishIntegrationEvent(Guid order)
    {
        var integrationEvent = new GracePeriodConfirmedIntegrationEvent(order);
        logger.LogInformation("Publishing integration event: {IntegrationEventId} from Ordering.Worder", integrationEvent.Id);
        eventBus.Publish(integrationEvent);
    }

    async Task<IEnumerable<Guid>> GetOrders()
    {
        try
        {
            using var connection = new SqlConnection(options.ConnectionString);
            return (await connection.QueryAsync<string>("select [id] from [ordering].[order] where [order_status_id] = 1")).Select(x => Guid.Parse(x));
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Something went wrong while retrieving orders");
            return new List<Guid>();
        }
    }
}
