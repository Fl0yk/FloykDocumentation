using MediatR;

namespace Forum.Application.UseCase.Command.Question;

public record CreateQuestionCommand(
                string Title,
                string Description,
                Guid AuthorId) : IRequest<Guid>;
