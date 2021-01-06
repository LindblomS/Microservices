namespace Services.Order.API.Application.Behaviours
{
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ValidationBehaviour<TRequest, TRespose> : IPipelineBehavior<TRequest, TRespose>
    {
        private readonly ILogger<ValidationBehaviour<TRequest, TRespose>> _logger;
        private readonly IValidatorFactory _validatorFactory;

        public ValidationBehaviour(ILogger<ValidationBehaviour<TRequest, TRespose>> logger, IValidatorFactory validatorFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validatorFactory = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
        }

        public async Task<TRespose> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TRespose> next)
        {
            _logger.LogInformation("----- Validating command {CommandType}", typeof(TRequest).Name);

            var validator = _validatorFactory.GetValidator<TRequest>();

            var failures = validator.Validate(request);

            if (!failures.IsValid)
            {
                _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeof(TRequest).Name, request, failures);

                throw new Exception(
                    $"Command Validation Errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures.Errors));
            }

            return await next();
        }
    }
}
