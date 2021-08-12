namespace Services.User.API.Application.Validators
{
    using FluentValidation;
    using Services.User.API.Application.Models.Commands;

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
