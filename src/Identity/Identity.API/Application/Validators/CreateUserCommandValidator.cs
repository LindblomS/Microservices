namespace Identity.API.Application.Validators
{
    using FluentValidation;
    using Services.Identity.Contracts.Commands;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.Username).NotEmpty();
            RuleFor(c => c.PasswordHash).NotEmpty();
            RuleFor(c => c.Claims).NotNull();
            RuleFor(c => c.Roles).NotNull();
        }
    }
}
