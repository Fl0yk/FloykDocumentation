namespace Identity.Application.Shared.Models.Requests.IdentityRequests;

public record class AddUserToRoleRequest(string Username, string RoleName);
