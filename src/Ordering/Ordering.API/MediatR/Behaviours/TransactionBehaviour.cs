namespace Ordering.API.MediatR.Behaviours;

using global::MediatR;
using Ordering.Application.Services;
using Ordering.Infrastructure.EntityFramework;
using Serilog.Context;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    readonly OrderingContext context;
    readonly IIntegrationEventService integrationEventService;
    readonly ILogger<TransactionBehaviour<TRequest, TResponse>> logger;

    public TransactionBehaviour(
        OrderingContext context,
        IIntegrationEventService integrationEventService,
        ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var response = default(TResponse);
        var typeName = request.GetGenericTypeName();

        try
        {
            if (context.HasActiveTransaction)
                return await next();

            var strategy = context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                Guid transactionId;

                using var transaction = await context.BeginTransactionAsync();
                using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                {
                    logger.LogInformation("Begin transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);
                    var response = await next();
                    logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);
                    await context.CommitTransactionAsync(transaction);
                    transactionId = transaction.TransactionId;
                }

                await integrationEventService.PublishEventsThroughEventBusAsync(transactionId);
            });

            return response;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error handling transaction for {CommandName}", typeName);
            throw;
        }
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
