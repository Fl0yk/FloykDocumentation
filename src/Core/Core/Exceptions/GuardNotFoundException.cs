using Core.Models.Enums;

namespace Core.Exceptions;
public sealed class GuardNotFoundException : CustomExceptionBase
{
    public override object? Details { get; set; }

    public override ErrorCode ErrorCode => ErrorCode.NotFound;

    public override string? SensitiveMessage { get; set; }

    public GuardNotFoundException(string message) : base(message)
    {

    }
}
