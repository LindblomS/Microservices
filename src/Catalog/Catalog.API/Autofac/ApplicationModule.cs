namespace Catalog.API.Autofac;

using Catalog.Application.Behaviours;
using Catalog.Application.CommandHandlers;
using Catalog.Application.DomainEventHandlers.CatalogItemPriceChanged;
using Catalog.Application.IntegrationEventHandlers;
using Catalog.Application.Repositories;
using Catalog.Application.Services;
using Catalog.Application.Validation;
using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Services;
using EventBus.EventBus.Abstractions;
using FluentValidation;
using global::Autofac;
using MediatR;
using System.Reflection;
using Module = global::Autofac.Module;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterRepositories(builder);
        RegisterMediator(builder);
        RegisterCommandHandlers(builder);
        RegisterDomainEventHandlers(builder);
        RegisterIntegrationEventHandlers(builder);
        RegisterValidation(builder);
        RegisterServices(builder);
        RegisterBehaviours(builder);
    }

    void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<CatalogRepository>()
            .As<ICatalogRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<QueryRepository>()
            .As<IQueryRepository>()
            .InstancePerLifetimeScope();
    }

    void RegisterMediator(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        builder.Register<ServiceFactory>(context =>
        {
            var componentContext = context.Resolve<IComponentContext>();
            return t =>
            {
                object o;
                return componentContext.TryResolve(t, out o)
                    ? o
                    : null;
            };
        });
    }

    void RegisterCommandHandlers(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(CreateItemCommandHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));
    }

    void RegisterDomainEventHandlers(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(PublishIntegrationEventDomainEventHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(INotificationHandler<>));
    }

    void RegisterIntegrationEventHandlers(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(OrderStatusChangedToAwaitingValidationIntegrationEventHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
    }

    void RegisterValidation(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(CreateItemCommandValidator).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IValidator<>));
    }

    void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<DomainEventPublisher>();
        builder.RegisterType<ValidationService>().As<IValidationService>();
    }

    void RegisterBehaviours(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(ValidationBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
    }
}
