using Autofac;
using CFS.Application.Application.Behaviours;
using CFS.Application.Application.Commands.Commands;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Reflection;

namespace CFS.Application.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // Resgiter all command handlers
            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register all command validators
            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly).Where(t => t.IsClosedTypeOf(typeof(IValidator<>))).AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
