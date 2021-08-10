namespace Identity.API.Application.Behaviours
{
    using Identity.API.Application.Factories;
    using MediatR;
    using Services.Identity.Contracts.Models.Results;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ResultBehaviours<T, R> : IPipelineBehavior<T, R> where T : IRequest<R> where R : Result
    {
        public async Task<R> Handle(T request, CancellationToken cancellationToken, RequestHandlerDelegate<R> next)
        {
            try
            {
                return await next();
            }
            catch (Exception exception)
            {
                return ResultFactory.CreateExceptionResult<R>(exception);
            }
        }
    }
}
