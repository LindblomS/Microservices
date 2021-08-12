namespace Services.User.API.Application.Behaviours
{
    using MediatR;
    using Services.User.API.Application.Factories;
    using Services.User.API.Application.Models.Results;
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
