using CFS.Application.Application.Commands.Commands;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CFS.Application.Application.Commands.Validations
{
    public class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
    {
        public UpdateServiceCommandValidator(ILogger<UpdateServiceCommandValidator> logger)
        {
            RuleFor(command => command.ServiceId).GreaterThan(0);
            RuleFor(command => command.FacilityId).GreaterThan(0);
            RuleFor(command => command.StartDate).NotEmpty();

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
