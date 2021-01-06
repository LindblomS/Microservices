namespace Services.Order.API.Infrastructure.AutoFacModules
{
    using Autofac;
    using EventBus.EventBus.Abstractions;
    using Services.Order.API.Application.IntegrationEvents.Handlers;
    using Services.Order.API.Application.Queries;
    using Services.Order.Domain;
    using Services.Order.Infrastructure;
    using System.Reflection;

    public class ApplicationModule : Autofac.Module
    {
        public ApplicationModule()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OrderQueries>()
                .As<IOrderQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderRepository>()
                .As<IOrderRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(CustomerDeletedIntegrationEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
