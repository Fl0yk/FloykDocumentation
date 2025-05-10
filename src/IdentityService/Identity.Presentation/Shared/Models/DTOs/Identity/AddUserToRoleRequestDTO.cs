namespace Identity.Presentation.Shared.Models.DTOs.Identity;

public record class AddUserToRoleRequestDTO(Guid UserId, string RoleName);
