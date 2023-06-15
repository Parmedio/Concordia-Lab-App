namespace BusinessLogic.Exceptions;

public class allColumnsEmptyException : Exception
{
    public allColumnsEmptyException()
    {
    }

    public allColumnsEmptyException(string? message) : base(message)
    {
    }

    public allColumnsEmptyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
