using Core.Models.Enums;

namespace Core.Exceptions;
public class GuardArgumentException : CustomExceptionBase
{
    public override object? Details { get; set; }

    public override ErrorCode ErrorCode { get; } = ErrorCode.Argument;

    public override string? SensitiveMessage { get; set; }

    public GuardArgumentException(string message) : base(message)
    {

    }

    public GuardArgumentException(string message, string argumentName) 
        : base($"Argument: {argumentName}\n Message: {message}")
    {
        
    }

    public GuardArgumentException(string message, ErrorCode errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public GuardArgumentException(string message, string argumentName, ErrorCode errorCode)
        : base($"Argument: {argumentName}\n Message: {message}")
    {
        ErrorCode = errorCode;
    }
}
