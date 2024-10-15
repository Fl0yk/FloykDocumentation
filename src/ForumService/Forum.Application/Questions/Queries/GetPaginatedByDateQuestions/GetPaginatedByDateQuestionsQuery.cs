using Forum.Application.Shared.Models;
using Forum.Application.Shared.Models.DTOs;
using MediatR;

namespace Forum.Application.Questions.Queries.GetPaginatedByDateQuestions;

public record GetPaginatedByDateQuestionsQuery(
                    int PageSize, 
                    int PageNumber) : IRequest<PaginatedResult<QuestionDTO>>;
