using Forum.Application.Shared.Models.Responses;
using MediatR;

namespace Forum.Application.UseCase.Command.Answer;

public sealed class DeleteAnswerCommand : IRequest<DeleteAnswerResponse>
{
    public Guid Id { get; init; }
}
