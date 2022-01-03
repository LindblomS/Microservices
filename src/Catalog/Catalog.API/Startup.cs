namespace Catalog.API;

using Autofac;
using Catalog.API.Filters;
using Catalog.API.IntegrationHandlers;
using Catalog.API.Repositories;
using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.EntityFramework;
using Catalog.Infrastructure.Repositories;
using EventBus.EventBus;
using EventBus.EventBus.Abstractions;
using EventBus.EventBusRabbitMQ;
using EventBus.IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Ordering.Contracts.IntegrationEvents;
using RabbitMQ.Client;
using Serilog;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        });

        services.AddEventBus(Configuration);
        services.AddTransient<IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>, OrderStatusChangedToPaidIntegrationEventHandler>();
        services.AddTransient<IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
        services.AddTransient<ICatalogRepository, CatalogRepository>();
        services.AddTransient<ICatalogQueryRepository, CatalogQueryRepository>();
        services.AddCustomDbContext(Configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.ConfigureEventBus();
    }
}

static class Extensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

            var factory = new ConnectionFactory()
            {
                HostName = configuration["EventBus:Connection"],
                DispatchConsumersAsync = true
            };

            return new DefaultRabbitMQPersistentConnection(factory, logger, 5);
        });

        services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
        {
            var subscriptionClientName = configuration["EventBus:SubscriptionClientName"];
            var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
            var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
            var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
            var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

            return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, 5);
        });

        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        return services;
    }

    public static IApplicationBuilder ConfigureEventBus(this IApplicationBuilder app)
    {
        var bus = app.ApplicationServices.GetRequiredService<IEventBus>();
        bus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>>();
        bus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>>();
        return app;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionString"], sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            });

        }, ServiceLifetime.Scoped);

        return services;
    }
}
