namespace Identity.Application.Services.Requests.UserRequests;

public record class SaveArticleRequest(Guid Id, string ArticleName);
