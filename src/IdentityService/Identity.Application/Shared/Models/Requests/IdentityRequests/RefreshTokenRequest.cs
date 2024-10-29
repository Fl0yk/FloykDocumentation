namespace Identity.Application.Shared.Models.Requests.IdentityRequests;

public record class RefreshTokenRequest(string Jwt, string Refresh);