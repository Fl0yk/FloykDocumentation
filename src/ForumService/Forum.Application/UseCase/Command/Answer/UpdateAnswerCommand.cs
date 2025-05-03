using Forum.Application.Shared.Models.DTOs;
using MediatR;

namespace Forum.Application.UseCase.Command.Answer;

public sealed class UpdateAnswerCommand : IRequest<AnswerDTO>
{
    public Guid Id { get; init; }

    public string Text { get; init; } = null!;
}

