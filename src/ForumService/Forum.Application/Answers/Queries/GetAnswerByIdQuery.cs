using Forum.Application.Shared.Models.DTOs;
using MediatR;

namespace Forum.Application.Answers.Queries;

public record class GetAnswerByIdQuery(Guid Id) : IRequest<AnswerDTO>;
