using FluentValidation;
using Identity.Application.UseCases.Command.Identity;

namespace Identity.Presentation.Shared.Validators.Identity;

//TODO: в какой момент валидировать? Мб до контроллера?
public class LoginUserRequestDTOValidator : AbstractValidator<LoginCommand>
{
    public LoginUserRequestDTOValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(r => r.Password)
            .NotEmpty();
    }
}
