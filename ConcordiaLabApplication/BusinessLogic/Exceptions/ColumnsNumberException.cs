namespace BusinessLogic.Exceptions;

public class ColumnsNumberException : Exception
{
    public ColumnsNumberException()
    {
    }

    public ColumnsNumberException(string? message) : base(message)
    {
    }

    public ColumnsNumberException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
