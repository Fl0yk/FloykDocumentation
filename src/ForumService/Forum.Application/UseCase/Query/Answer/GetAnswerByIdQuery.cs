using Forum.Application.Shared.Models.DTOs;
using MediatR;

namespace Forum.Application.UseCase.Query.Answer;

public record class GetAnswerByIdQuery(Guid Id) : IRequest<AnswerDTO>;
