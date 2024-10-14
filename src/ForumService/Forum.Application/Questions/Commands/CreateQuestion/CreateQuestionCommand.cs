using MediatR;

namespace Forum.Application.Questions.Commands.CreateQuestion;

public record CreateQuestionCommand(
                string Title, 
                string Description,
                Guid AuthorId) : IRequest<Guid>;