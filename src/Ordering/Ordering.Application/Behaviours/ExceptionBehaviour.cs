namespace Ordering.Application.Behaviours;

using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    readonly ILogger<ExceptionBehaviour<TRequest, TResponse>> logger;

    public ExceptionBehaviour(ILogger<ExceptionBehaviour<TRequest, TResponse>> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
            throw;
        }
    }
}
