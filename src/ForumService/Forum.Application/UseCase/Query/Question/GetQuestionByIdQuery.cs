using Forum.Application.Shared.Models.DTOs;
using MediatR;

namespace Forum.Application.UseCase.Query.Question;

public record class GetQuestionByIdQuery : IRequest<QuestionDTO>
{
    public Guid Id { get; init; }
}
