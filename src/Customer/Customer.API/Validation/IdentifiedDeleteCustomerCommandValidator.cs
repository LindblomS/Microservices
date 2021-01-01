namespace Services.Customer.API.Validation
{
    using FluentValidation;
    using Microsoft.Extensions.Logging;
    using Services.Customer.API.Application.Commands;

    public class IdentifiedDeleteCustomerCommandValidator : AbstractValidator<IdentifiedCommand<DeleteCustomerCommand, bool>>
    {
        public IdentifiedDeleteCustomerCommandValidator(ILogger<IdentifiedDeleteCustomerCommandValidator> logger)
        {
            RuleFor(command => command.Id).NotEmpty();
            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
