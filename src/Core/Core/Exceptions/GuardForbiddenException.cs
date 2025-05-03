using Core.Models.Enums;

namespace Core.Exceptions;
public sealed class GuardForbiddenException : CustomExceptionBase
{
    public override object? Details { get; set; }

    public override ErrorCode ErrorCode { get; } = ErrorCode.Forbidden;

    public override string? SensitiveMessage { get; set; }

    public GuardForbiddenException(string message) : base(message)
    {

    }
}
