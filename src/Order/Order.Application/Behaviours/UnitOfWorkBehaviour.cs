namespace Ordering.Application.Behaviours;

using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Exceptions;
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
        if (unitOfWork.Active)
            return await next();

        using (LogContext.PushProperty("UnitOfWork", unitOfWork.Id))
        {
            try
            {
                logger.LogInformation("Beginning UnitOfWork {UnitOfWorkId}", unitOfWork.Id);
                unitOfWork.Begin();

                var response = await next();

                logger.LogInformation("Commiting UnitOfWork {UnitOfWorkId}", unitOfWork.Id);
                await unitOfWork.Commit(unitOfWork.Id);
                return response;

            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error commiting UnitOfWork {UnitOfWork}", unitOfWork.Id);
                throw new UnitOfWorkException(unitOfWork.Id, "Something went wrong when commiting the UnitOfWork", exception);
            }
        }
    }
}
