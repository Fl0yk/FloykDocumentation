using MediatR;

namespace Forum.Application.UseCase.Command.Answer;

public record class AddAnswerCommand(
                        string Text,
                        Guid AuthorId,
                        Guid QuestionId,
                        Guid? ParentId) : IRequest<Guid>;
