namespace Ordering.Application.Behaviours;

using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Services;
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    readonly IUnitOfWork unitOfWork;
    readonly ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> logger;

    public UnitOfWorkBehaviour(IUnitOfWork unitOfWork, ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> logger)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            if (unitOfWork.Active)
                return await next();

            using (LogContext.PushProperty("UnitOfWork", unitOfWork.Id))
            {
                logger.LogInformation("Beginning UnitOfWork {UnitOfWorkId}", unitOfWork.Id);
                await unitOfWork.BeginAsync();

                var response = await next();

                logger.LogInformation("Commiting UnitOfWork {UnitOfWorkId}", unitOfWork.Id);
                await unitOfWork.CommitAsync(unitOfWork);
                return response;
            }
        }
        finally
        {
            unitOfWork?.Dispose();
        }
        
    }
}
