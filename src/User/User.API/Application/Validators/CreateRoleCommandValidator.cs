namespace Services.User.API.Application.Validators
{
    using FluentValidation;
    using Services.User.API.Application.Models.Commands;

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
