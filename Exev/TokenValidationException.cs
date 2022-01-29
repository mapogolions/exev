namespace Exev;

public class TokenValidationException : Exception
{
    public TokenValidationException(string message)
    {
        Message = message;
    }

    public override string Message { get; }
}
