using FluentValidation;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Validators.Identity;

public class RegistrationUserRequestDTOValidator : AbstractValidator<RegistrationUserRequestDTO>
{
    public RegistrationUserRequestDTOValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(r => r.Password)
            .NotEmpty()
            .Length(8, 128)
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
    }
}
