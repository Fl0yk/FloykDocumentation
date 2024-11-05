using FluentValidation;
using Identity.Application.Shared.Models.Requests.IdentityRequests;

namespace Identity.Presentation.Shared.Validators.Identity;

public class LoginUserRequestDTOValidator : AbstractValidator<LoginUserRequest>
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
