namespace Identity.Application.Services.Requests.IdentityRequests;

public record class AddUserToRoleRequest(string Username, string RoleName);
