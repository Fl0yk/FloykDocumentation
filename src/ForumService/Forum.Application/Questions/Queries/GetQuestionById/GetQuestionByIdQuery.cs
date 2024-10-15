using Forum.Application.Shared.Models.DTOs;
using MediatR;

namespace Forum.Application.Questions.Queries.GetQuestionById;

public record class GetQuestionByIdQuery(Guid Id) : IRequest<QuestionDTO>;
