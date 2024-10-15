using MediatR;

namespace Forum.Application.Answers.Commands.UpdateAnswer;

public record class UpdateAnswerCommand(Guid Id, string Text) : IRequest<Guid>;
