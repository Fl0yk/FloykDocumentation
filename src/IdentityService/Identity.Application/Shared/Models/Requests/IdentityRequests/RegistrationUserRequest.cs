namespace Identity.Application.Shared.Models.Requests.IdentityRequests;

public record class RegistrationUserRequest(string Username, string Email, string Password);
