using Autofac;
using CFS.Application.Application.Queries;
using CFS.Domain.Aggregates.CustomerAggregate;
using CFS.Domain.Aggregates.FacilityAggregate;
using CFS.Domain.Aggregates.ServiceAggregate;
using CFS.Infrastructure;
using CFS.Infrastructure.Repositories;

namespace CFS.Application.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string queriesConnectionString)
        {
            QueriesConnectionString = queriesConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ConnectionFactory(QueriesConnectionString)).As<IConnectionFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Queries>().As<IQueries>().InstancePerLifetimeScope();
            builder.RegisterType<DataContext>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FacilityRepository>().As<IFacilityRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ServiceRepository>().As<IServiceRepository>().InstancePerLifetimeScope();
        }
    }
}
