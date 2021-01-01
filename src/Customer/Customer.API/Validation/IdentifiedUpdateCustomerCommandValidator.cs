namespace Services.Customer.API.Validation
{
    using FluentValidation;
    using Microsoft.Extensions.Logging;
    using Services.Customer.API.Application.Commands;

    public class IdentifiedUpdateCustomerCommandValidator : AbstractValidator<IdentifiedCommand<UpdateCustomerCommand, bool>>
    {
        public IdentifiedUpdateCustomerCommandValidator(ILogger<IdentifiedUpdateCustomerCommandValidator> logger)
        {
            RuleFor(command => command.Id).NotEmpty();
            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
