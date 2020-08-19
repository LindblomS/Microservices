using CFS.Application.Application.Commands.Commands;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CFS.Application.Application.Commands.Validations
{
    public class CreateFacilityCommandValidator : AbstractValidator<CreateFacilityCommand>
    {
        public CreateFacilityCommandValidator(ILogger<CreateFacilityCommandValidator> logger)
        {
            RuleFor(command => command.CustomerId).GreaterThan(0);

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
