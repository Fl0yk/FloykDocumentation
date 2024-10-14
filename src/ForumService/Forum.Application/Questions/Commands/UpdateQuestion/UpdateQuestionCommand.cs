using MediatR;

namespace Forum.Application.Questions.Commands.UpdateQuestion;

public record class UpdateQuestionCommand(
                        Guid Id, 
                        string Title, 
                        string Description) : IRequest<Guid>;
