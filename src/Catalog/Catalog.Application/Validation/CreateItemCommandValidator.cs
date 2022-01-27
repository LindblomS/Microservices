namespace Catalog.Application.Validation;

using Catalog.Contracts.Commands;
using FluentValidation;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
}
