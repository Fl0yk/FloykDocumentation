namespace Identity.Application.Shared.Models.Requests;

public record class RefreshTokenRequest(string Jwt, string Refresh);