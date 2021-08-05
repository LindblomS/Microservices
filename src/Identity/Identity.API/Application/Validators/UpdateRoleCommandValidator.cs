namespace Identity.API.Application.Validators
{
    using FluentValidation;
    using Services.Identity.Contracts.Commands;

    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(c => c.DisplayName).NotEmpty();
            RuleFor(c => c.Id).NotEmpty();
            RuleForEach(c => c.Claims).SetValidator(new ClaimValidator());
        }
    }
}
