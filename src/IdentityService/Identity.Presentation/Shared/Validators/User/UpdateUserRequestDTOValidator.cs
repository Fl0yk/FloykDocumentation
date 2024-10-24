using FluentValidation;
using Identity.Presentation.Shared.Models.DTOs.User;

namespace Identity.Presentation.Shared.Validators.User;

public class UpdateUserRequestDTOValidator : AbstractValidator<UpdateUserRequestDTO>
{
    public UpdateUserRequestDTOValidator()
    {
        RuleFor(r => r.NewUsername)
            .NotEmpty()
            .MaximumLength(20);
    }
}
