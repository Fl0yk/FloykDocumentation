using Core.Models.Enums;

namespace Core.Exceptions;
public sealed class GuardUnauthorizedException : CustomExceptionBase
{
    public override object? Details { get; set; }

    public override ErrorCode ErrorCode => ErrorCode.Unauthorized;

    public override string? SensitiveMessage { get; set; }

    public GuardUnauthorizedException(string message) : base(message)
    {

    }
}
