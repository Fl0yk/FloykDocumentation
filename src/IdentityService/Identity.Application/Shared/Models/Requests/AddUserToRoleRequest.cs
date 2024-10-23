namespace Identity.Application.Shared.Models.Requests;

public record class AddUserToRoleRequest(string Username, string RoleName);
