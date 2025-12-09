namespace Identity.Core.Exceptions;

public class InvalidRefreshTokenException : Exception
{
    public InvalidRefreshTokenException()
        : base("Invalid refresh token")
    {
    }

    public InvalidRefreshTokenException(string message)
        : base(message)
    {
    }

    public InvalidRefreshTokenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}