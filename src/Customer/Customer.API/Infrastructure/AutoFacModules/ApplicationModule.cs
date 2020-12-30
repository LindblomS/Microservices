namespace Services.Customer.API.Infrastructure.AutoFacModules
{
    using Autofac;
    using Services.Customer.API.Application.Queries;
    using Services.Customer.Domain;
    using Services.Customer.Infrastructure;
    using Services.Customer.Infrastructure.Idempotency;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationModule : Autofac.Module
    {
        public ApplicationModule()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerQueries>()
                .As<ICustomerQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CustomerRepository>()
                .As<ICustomerRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestManager>()
                .As<IRequestManager>()
                .InstancePerLifetimeScope();


        }
    }
}
