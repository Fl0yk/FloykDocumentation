namespace Identity.Domain.Abstractions.Managers;

public interface IEmailManager
{
    Task SendRegistrationCompleteEmailAsync(string email, string token, Guid userId);

    Task SendArticleApprovedEmailAsync(string email, string username, string articleTitle);
}
