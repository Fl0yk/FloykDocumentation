using Forum.Application.Shared.Models.DTOs;
using MediatR;

namespace Forum.Application.UseCase.Query.Answer;

public sealed class GetAnswerByIdQuery : IRequest<AnswerDTO>
{
    public Guid Id { get; init; }
}
