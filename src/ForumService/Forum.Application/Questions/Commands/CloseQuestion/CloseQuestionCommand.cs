using MediatR;

namespace Forum.Application.Questions.Commands.CloseQuestion;

public record class CloseQuestionCommand(Guid Id) : IRequest<Guid>;