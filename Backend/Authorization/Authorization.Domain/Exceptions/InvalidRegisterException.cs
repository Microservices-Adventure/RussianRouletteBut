namespace Authorization.Domain.Exceptions;

public class InvalidRegisterException : Exception
{
    public InvalidRegisterException(string? message) : base(message)
    {
    }
}
