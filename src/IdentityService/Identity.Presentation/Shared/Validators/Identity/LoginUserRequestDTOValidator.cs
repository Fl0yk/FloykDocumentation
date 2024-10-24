using FluentValidation;
using Identity.Application.Services.Requests.IdentityRequests;

namespace Identity.Presentation.Shared.Validators.Identity;

public class LoginUserRequestDTOValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestDTOValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(r => r.Password)
            .NotEmpty()
            .Length(8, 128)
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-])$");
    }
}
