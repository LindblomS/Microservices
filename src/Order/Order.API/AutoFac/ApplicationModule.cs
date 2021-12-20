namespace Ordering.API.AutoFac;

using Autofac;
using FluentValidation;
using MediatR;
using Ordering.Application.Behaviours;
using Ordering.Application.Commands;
using Ordering.Application.DomainEventHandlers.OrderStarted;
using Ordering.Application.Services;
using Ordering.Application.Validation;
using Ordering.Domain.AggregateModels.Buyer;
using Ordering.Domain.AggregateModels.Order;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Services;
using System.Reflection;

public class ApplicationModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterRepositories(builder);
        RegisterMediator(builder);
        RegisterCommandHandlers(builder);
        RegisterDomainEventHandlers(builder);
        RegisterValidation(builder);
        RegisterServices(builder);
        RegisterBehaviours(builder);
    }

    void RegisterRepositories(ContainerBuilder builder)
    {
        builder
            .RegisterType<OrderRepository>()
            .As<IOrderRepository>()
            .InstancePerLifetimeScope();

        builder
            .RegisterType<BuyerRepository>()
            .As<IBuyerRepository>()
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
            .RegisterAssemblyTypes(typeof(CreateOrderCommandHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));
    }

    void RegisterDomainEventHandlers(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(AddOrValidateBuyerDomainEventHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(INotificationHandler<>));
    }

    void RegisterValidation(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(CreateOrderCommandValidator).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IValidator<>));
    }

    void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<DomainEventPublisher>();
        builder.RegisterType<RequestManager>().As<IRequestManager>();
        builder.RegisterType<IntegrationEventService>().As<IIntegrationEventService>();
    }

    void RegisterBehaviours(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(ValidationBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(UnitOfWorkBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
    }
}
