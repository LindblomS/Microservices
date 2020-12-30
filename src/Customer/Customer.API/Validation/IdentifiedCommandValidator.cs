namespace Services.Customer.API.Validation
{
    using FluentValidation;
    using Microsoft.Extensions.Logging;
    using Services.Customer.API.Application.Commands;
    using System;

    public class IdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<CreateCustomerCommand, Guid>>
    {
        public IdentifiedCommandValidator(ILogger<IdentifiedCommandValidator> logger)
        {
            RuleFor(command => command.Id).NotEmpty();

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
