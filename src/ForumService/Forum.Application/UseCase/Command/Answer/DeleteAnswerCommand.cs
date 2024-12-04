using Forum.Application.Shared.Models.Responses;
using MediatR;

namespace Forum.Application.UseCase.Command.Answer;

public class DeleteAnswerCommand : IRequest<DeleteAnswerResponse>
{
    public Guid Id { get; init; }

    public Guid AuthorId { get; set; }
}
