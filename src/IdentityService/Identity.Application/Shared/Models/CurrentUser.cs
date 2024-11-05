namespace Identity.Application.Shared.Models;

public record class CurrentUser(
                        Guid Id, 
                        string Email,
                        string Username,
                        IEnumerable<string> Roles);
