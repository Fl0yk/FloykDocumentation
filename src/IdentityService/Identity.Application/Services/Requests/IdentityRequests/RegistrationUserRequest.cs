namespace Identity.Application.Services.Requests.IdentityRequests;

public record class RegistrationUserRequest(string Username, string Email, string Password);
