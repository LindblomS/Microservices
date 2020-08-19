using CFS.Application.Application.Commands.Commands;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CFS.Application.Application.Commands.Validations
{
    public class DeleteFacilityCommandValidator : AbstractValidator<DeleteFacilityCommand>
    {
        public DeleteFacilityCommandValidator(ILogger<DeleteFacilityCommandValidator> logger)
        {
            RuleFor(command => command.FacilityId).GreaterThan(0);

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
