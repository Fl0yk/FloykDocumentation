using Forum.Application.Shared.Models.DTOs;
using Forum.Domain.Models;
using MediatR;

namespace Forum.Application.Questions.Queries.GetPaginatedByDateQuestions;

public record GetPaginatedByDateQuestionsQuery(
                    int PageSize, 
                    int PageNumber) : IRequest<PaginatedResult<QuestionDTO>>;
