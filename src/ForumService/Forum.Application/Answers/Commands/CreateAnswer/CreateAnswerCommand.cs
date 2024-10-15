using MediatR;

namespace Forum.Application.Answers.Commands.CreateAnswer;

public record class CreateAnswerCommand(
                        string Text,
                        Guid AuthorId,
                        Guid QuestionId,
                        Guid? ParentId) : IRequest<Guid>;
