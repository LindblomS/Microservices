﻿namespace Ordering.API.AutoFac;

using Autofac;
using EventBus.EventBus.Abstractions;
using FluentValidation;
using global::MediatR;
using Ordering.API.MediatR.Behaviours;
using Ordering.Application.Behaviours;
using Ordering.Application.DomainEventHandlers.OrderStarted;
using Ordering.Application.IntegrationEventHandlers;
using Ordering.Application.Repositories;
using Ordering.Application.RequestHandlers.CommandHandlers;
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
        RegisterIntegrationEventHandlers(builder);
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

        builder
            .RegisterType<QueryRepository>()
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
            .RegisterAssemblyTypes(typeof(CreateOrderHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));
    }

    void RegisterDomainEventHandlers(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(AddOrValidateBuyerDomainEventHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(INotificationHandler<>));
    }

    void RegisterIntegrationEventHandlers(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(OrderPaymentFailedIntegrationEventHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
    }

    void RegisterValidation(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(CreateOrderCommandValidator).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IValidator<>));

        builder.RegisterType<ValidationService>().As<IValidationService>().InstancePerLifetimeScope(); ;
    }

    void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<DomainEventPublisher>();
        builder.RegisterType<RequestManager>().As<IRequestManager>();
        builder.RegisterType<IntegrationEventService>().As<IIntegrationEventService>().InstancePerLifetimeScope();
    }

    void RegisterBehaviours(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(ValidationBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
    }
}
