namespace Ordering.Application.Behaviours;

using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Services;
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    readonly IUnitOfWork unitOfWork;
    readonly ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> logger;
    readonly IIntegrationEventService integrationEventService;

    public UnitOfWorkBehaviour(IUnitOfWork unitOfWork, ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> logger, IIntegrationEventService integrationEventService)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (unitOfWork.Active)
            return await next();

        await unitOfWork.BeginAsync();
        using (LogContext.PushProperty("UnitOfWork", unitOfWork.TransactionId))
        {
            logger.LogInformation("Beginning UnitOfWork {UnitOfWorkId}", unitOfWork.TransactionId);

            var response = await next();

            logger.LogInformation("Commiting UnitOfWork {UnitOfWorkId}", unitOfWork.TransactionId);
            var transactionId = unitOfWork.TransactionId;
            await unitOfWork.CommitAsync(unitOfWork);
            await integrationEventService.PublishEventsThroughEventBusAsync(transactionId);
            return response;
        }
    }
}
