using Forum.Application.Shared.Models.DTOs;
using MediatR;

namespace Forum.Application.UseCase.Command.Answer;

public sealed class AddAnswerCommand : IRequest<AnswerDTO>
{
    public string Text { get; init; } = null!;

    public Guid QuestionId { get; init; }

    public Guid? ParentId { get; init; }
}

