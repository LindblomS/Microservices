using Application.Infrastructure;
using Autofac;
using CFS.Application.Application.Queries;
using CFS.Domain.Aggregates;
using CFS.Domain.SeedWork;
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
            builder.RegisterType<RepositoryFactory>().InstancePerDependency();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();
            builder.RegisterType<Queries>().As<IQueries>().InstancePerLifetimeScope();
            builder.RegisterType<DbQueries>().As<IDbQueries>().InstancePerLifetimeScope();
            builder.RegisterType<DbWithTransaction>().As<IDbWithTransaction>().InstancePerLifetimeScope();
        }
    }
}
