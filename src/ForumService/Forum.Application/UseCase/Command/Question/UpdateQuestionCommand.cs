using MediatR;

namespace Forum.Application.UseCase.Command.Question;

public record class UpdateQuestionCommand(
                        Guid Id,
                        string Title,
                        string Description) : IRequest<Guid>;
