namespace Ordering.Application.Behaviours;

using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    readonly ILogger<LoggingBehaviour<TRequest, TResponse>> logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var commandName = request.GetGenericTypeName();
        logger.LogInformation("Handling command {CommandName} ({@Command})", commandName, request);
        var response = await next();
        logger.LogInformation("Command {CommandName} handled - response: {@Response}", commandName, response);
        return response;
    }
}


public static class GenericTypeExtensions
{
    public static string GetGenericTypeName(this Type type)
    {
        var typeName = string.Empty;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }

    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }

}
