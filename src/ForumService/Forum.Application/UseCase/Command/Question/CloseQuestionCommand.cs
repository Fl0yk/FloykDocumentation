using MediatR;

namespace Forum.Application.UseCase.Command.Question;

public record class CloseQuestionCommand(Guid Id) : IRequest<Guid>;