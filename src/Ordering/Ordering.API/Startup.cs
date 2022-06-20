namespace Ordering.API;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Basket.Contracts.IntegrationEvents;
using Catalog.Contracts.IntegrationEvents;
using EventBus.EventBus;
using EventBus.EventBus.Abstractions;
using EventBus.EventBus.Events;
using EventBus.EventBusRabbitMQ;
using EventBus.IntegrationEventLogEF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.API.AutoFac;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Infrastructure.EntityFramework;
using Ordering.Infrastructure.Options;
using Payment.Contracts.IntegrationEvents;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Data.Common;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; set; }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddCustomDbContext(Configuration);
        services.AddEventBus(Configuration);
        services.AddControllers();
        services.Configure<CatalogApiOptions>(Configuration.GetSection(nameof(CatalogApiOptions)));
        var container = new ContainerBuilder();

        container.Populate(services);
        container.RegisterModule(new ApplicationModule());
        return new AutofacServiceProvider(container.Build());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSerilogRequestLogging();
        app.UseRouting();
        app.UseEndpoints(e => e.MapControllers());
        ConfigureEventBus(app);
    }

    private void ConfigureEventBus(IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<GracePeriodConfirmedIntegrationEvent, IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>>();
        eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>>();
        eventBus.Subscribe<OrderPaymentSucceededIntegrationEvent, IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>>();
        eventBus.Subscribe<OrderStockConfirmedIntegrationEvent, IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>>();
        eventBus.Subscribe<OrderStockRejectedIntegrationEvent, IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>>();
        eventBus.Subscribe<UserCheckoutAcceptedIntegrationEvent, IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>>();
    }
}

static class CustomExtensionsMethods
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderingContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionString"], sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            });

        }, ServiceLifetime.Scoped);

        services.AddDbContext<IntegrationEventLogContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionString"],
                                 sqlServerOptionsAction: sqlOptions =>
                                 {
                                     sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                 });
        }, ServiceLifetime.Scoped);

        return services;
    }

    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(sp => (DbConnection c) =>
        {
            var eventTypes = typeof(OrderStartedIntegrationEvent).Assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();

            return new IntegrationEventLogService(c, eventTypes);
        });

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
}

