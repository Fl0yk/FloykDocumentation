using MediatR;

namespace Forum.Application.UseCase.Command.Answer;

public record class DeleteAnswerCommand(Guid Id) : IRequest;
