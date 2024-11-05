namespace Identity.Presentation.Shared.Models.DTOs.Identity;

public record class RefreshTokenRequestDTO(string Jwt, string Refresh);
