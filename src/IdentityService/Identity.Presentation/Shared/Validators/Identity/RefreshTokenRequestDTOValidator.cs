using FluentValidation;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Validators.Identity;

public class RefreshTokenRequestDTOValidator : AbstractValidator<RefreshTokenRequestDTO>
{
    public RefreshTokenRequestDTOValidator()
    {
        RuleFor(r => r.Jwt).NotEmpty();

        RuleFor(r => r.Refresh).NotEmpty();
    }
}
