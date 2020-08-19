using CFS.Application.Application.Commands.Commands;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CFS.Application.Application.Commands.Validations
{
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator(ILogger<DeleteCustomerCommandValidator> logger)
        {
            RuleFor(command => command.CustomerId).GreaterThan(0);

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
