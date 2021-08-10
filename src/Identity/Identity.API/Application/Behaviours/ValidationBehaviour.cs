namespace Identity.API.Application.Behaviours
{
    using FluentValidation;
    using Identity.API.Application.Factories;
    using MediatR;
    using Services.Identity.Contracts.Models.Results;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ValidationBehaviour<T, R> : IPipelineBehavior<T, R> where R : Result
    {
        private readonly IValidator<T>[] _validators;

        public ValidationBehaviour(IValidator<T>[] validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<R> Handle(T request, CancellationToken cancellationToken, RequestHandlerDelegate<R> next)
        {
            var result = _validators.Select(x => x.Validate(request)).Where(x => !x.IsValid);

            if (result.Any())
                return ResultFactory.CreateFailureResult<R>(string.Join(Environment.NewLine, result));

            return await next();
        }
    }
}
