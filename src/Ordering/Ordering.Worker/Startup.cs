namespace Ordering.Worker;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System;
using EventBus.EventBus;
using EventBus.EventBus.Abstractions;
using EventBus.EventBusRabbitMQ;
using RabbitMQ.Client;

internal class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; set; }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddEventBus(Configuration);

        services.Configure<WorkerOptions>(options =>
        {
            options.ConnectionString = Configuration["ConnectionString"];
            options.CheckUpdateTimeSeconds = Configuration.GetValue<int>("CheckUpdateTimeSeconds");
        });

        services.AddHostedService<Worker>();
        var container = new ContainerBuilder();

        container.Populate(services);
        return new AutofacServiceProvider(container.Build());
    }

    public void Configure(IApplicationBuilder app)
    {
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
}
