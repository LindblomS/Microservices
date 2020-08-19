using CFS.Application.Application.Commands.Commands;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CFS.Application.Application.Commands.Validations
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator(ILogger<CreateCustomerCommandValidator> logger)
        {
            RuleFor(command => command.FirstName).MaximumLength(20);
            RuleFor(command => command.LastName).MaximumLength(20);
            RuleFor(command => command.Email).Must(EmailValidator.IsValidEmail).WithMessage("Email is not valid");

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
