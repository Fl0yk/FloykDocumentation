using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public class UpdateUsernameForAuthorsRequest : IRequest
{
    public required string OldUsername { get; set; }

    public required string NewUsername { get; set; }
}
