namespace Catalog.Application.Behaviours;

using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Catalog.Application.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    readonly IValidator<TRequest>[] validators;
    readonly ILogger<ValidationBehaviour<TRequest, TResponse>> logger;

    public ValidationBehaviour(IValidator<TRequest>[] validators, ILogger<ValidationBehaviour<TRequest, TResponse>> logger)
    {
        this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var typeName = request.GetType().Name;
        logger.LogInformation("Validating command {CommandType}", typeName);

        var errors = validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(e => e is not null);

        if (errors.Any())
        {
            logger.LogWarning("Validation errors - {CommandType} - Command {@Command} - Errors: {@ValidatonErrors}",
                typeName,
                request,
                errors);

            throw new CommandValidationException(errors.Select(e => e.ErrorMessage));
        }

        return await next();
    }
}
