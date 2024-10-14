using MediatR;

namespace Forum.Application.Questions.Commands.DeleteQuestion;

public record class DeleteQuestionCommand(Guid Id) : IRequest;
