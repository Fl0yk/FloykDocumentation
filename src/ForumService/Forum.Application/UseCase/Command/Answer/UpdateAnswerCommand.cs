using Forum.Application.Shared.Models.DTOs;
using MediatR;

namespace Forum.Application.UseCase.Command.Answer;

public class UpdateAnswerCommand : IRequest<AnswerDTO>
{
    public Guid Id { get; init; }

    public required string Text { get; init; }

    public Guid AuthorId { get; init; }
}

