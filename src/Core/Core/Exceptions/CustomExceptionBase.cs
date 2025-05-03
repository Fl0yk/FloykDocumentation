using Core.Models.Enums;

namespace Core.Exceptions;
public abstract class CustomExceptionBase : Exception
{
    public CustomExceptionBase(string message) : base(message)
    {
    }

    public CustomExceptionBase(string message, Exception innerException) : base(message, innerException)
    {
    }

    public abstract object? Details { get; set; }

    public abstract ErrorCode ErrorCode { get; }

    public abstract string? SensitiveMessage { get; set; }

    public CustomExceptionBase WithDetails(object details)
    {
        Details = details;

        return this;
    }
}