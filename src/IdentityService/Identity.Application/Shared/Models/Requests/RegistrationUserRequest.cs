namespace Identity.Application.Shared.Models.Requests;

public record class RegistrationUserRequest(string Username, string Email, string Password);
