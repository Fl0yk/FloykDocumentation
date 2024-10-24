namespace Identity.Application.Services.Requests.IdentityRequests;

public record class RefreshTokenRequest(string Jwt, string Refresh);