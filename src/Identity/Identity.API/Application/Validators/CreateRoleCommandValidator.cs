namespace Identity.API.Application.Validators
{
    using FluentValidation;
    using Services.Identity.Contracts.Commands;

    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.DisplayName).NotEmpty();
            RuleForEach(c => c.Claims).SetValidator(new ClaimValidator());
        }
    }
}
