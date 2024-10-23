namespace Identity.Application.Shared.Exceptions;

class UnauthorizedException : Exception
{
    public UnauthorizedException() : base() { }

    public UnauthorizedException(string message) : base(message) { }
}
