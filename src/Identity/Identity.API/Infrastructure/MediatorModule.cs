namespace Identity.API.Infrastructure
{
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using FluentValidation;
    using Identity.API.Application.Behaviours;
    using Identity.API.Application.Handlers.CommandHandlers;
    using Identity.API.Application.Validators;
    using MediatR;
    using Services.Identity.Domain.AggregateModels.Role;
    using Services.Identity.Domain.AggregateModels.User;
    using Services.Identity.Infrastructure;
    using Services.Identity.Infrastructure.Idempotency;
    using Services.Identity.Infrastructure.Repositories;

    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestManager>()
                .As<IRequestManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CustomContext>()
                .As<IDbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>();

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            builder.RegisterAssemblyTypes(typeof(CreateUserCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register the Command's Validators (Validators based on FluentValidation library)
            builder
                .RegisterAssemblyTypes(typeof(CreateUserCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidationBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
