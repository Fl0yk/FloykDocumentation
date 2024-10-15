using MediatR;

namespace Forum.Application.Answers.Commands.DeleteAnswer;

public record class DeleteAnswerCommand(Guid Id) : IRequest;
