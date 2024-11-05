using FluentValidation;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Validators.Identity;

public class AddUserToRoleRequestDTOValidator : AbstractValidator<AddUserToRoleRequestDTO>
{
    public AddUserToRoleRequestDTOValidator()
    {
        RuleFor(r => r.Username).NotEmpty();

        RuleFor(r => r.RoleName).NotEmpty();
    }
}
