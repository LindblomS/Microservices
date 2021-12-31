namespace Basket.API;

using Autofac;
using Basket.API.Inteceptors;
using Basket.API.IntegrationEventHandlers;
using Basket.API.Services;
using Basket.Domain.AggregateModels;
using Basket.Infrastructure.Repositories;
using Catalog.Contracts.IntegrationEvents;
using EventBus.EventBus;
using EventBus.EventBus.Abstractions;
using EventBus.EventBusRabbitMQ;
using Ordering.Contracts.IntegrationEvents;
using RabbitMQ.Client;
using Serilog;
using StackExchange.Redis;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEventBus(Configuration);
        services
            .AddTransient<IIntegrationEventHandler<OrderStartedIntegrationEvent>, OrderStartedIntegrationEventHandler>()
            .AddTransient<IIntegrationEventHandler<ProductPriceChangedIntegrationEvent>, ProductPriceChangedIntegrationEventHandler>();

        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ExceptionInterceptor>();
        });

        services.AddTransient<IBasketRepository, RedisBasketRepository>();

        services.AddSingleton(services =>
        {
            var configuration = ConfigurationOptions.Parse(Configuration["RedisConnectionString"]);
            return ConnectionMultiplexer.Connect(configuration);
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<GrpcBasketService>();
        });

        app.ConfigureEventBus();
    }
}

internal static class Extensions
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
        bus.Subscribe<OrderStartedIntegrationEvent, IIntegrationEventHandler<OrderStartedIntegrationEvent>>();
        bus.Subscribe<ProductPriceChangedIntegrationEvent, IIntegrationEventHandler<ProductPriceChangedIntegrationEvent>>();

        return app;
    }
}
