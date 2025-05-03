using Core.Models.Enums;

namespace Core.Exceptions;
public sealed class AppException : CustomExceptionBase
{
    public override object? Details { get; set; }

    public override ErrorCode ErrorCode => ErrorCode.App;

    public override string? SensitiveMessage { get; set; }

    public AppException(string message) : base(message)
    {

    }
}
