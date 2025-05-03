using Core.Models;
using Forum.Application.Shared.Models.DTOs;
using MediatR;

namespace Forum.Application.UseCase.Query.Question;

public sealed class GetPaginatedByDateQuestionsQuery : IRequest<PaginatedResult<QuestionDTO>>
{
    public int PageSize { get; init; }

    public int PageNumber { get; init; }
}