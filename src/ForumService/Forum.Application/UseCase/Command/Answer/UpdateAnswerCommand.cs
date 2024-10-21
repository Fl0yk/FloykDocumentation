using MediatR;

namespace Forum.Application.UseCase.Command.Answer;

public record class UpdateAnswerCommand(Guid Id, string Text) : IRequest<Guid>;
