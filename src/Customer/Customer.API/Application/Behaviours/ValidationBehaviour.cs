namespace Services.Customer.API.Application.Behaviours
{
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ValidationBehaviour<TRequest, TRespose> : IPipelineBehavior<TRequest, TRespose>
    {
        private readonly ILogger<ValidationBehaviour<TRequest, TRespose>> _logger;
        private readonly IValidator<TRequest>[] _validators;

        public ValidationBehaviour(ILogger<ValidationBehaviour<TRequest, TRespose>> logger, IValidator<TRequest>[] validators)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TRespose> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TRespose> next)
        {
            _logger.LogInformation("----- Validating command {CommandType}", typeof(TRequest).Name);

            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeof(TRequest).Name, request, failures);

                throw new Exception(
                    $"Command Validation Errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures));
            }

            return await next();
        }
    }
}
