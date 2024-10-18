using MediatR;

namespace Forum.Application.UseCase.Command.Question;

public record class DeleteQuestionCommand(Guid Id) : IRequest;
