namespace BusinessLogic.Exceptions;

public class AllListsEmptyException : Exception
{
    public AllListsEmptyException()
    {
    }

    public AllListsEmptyException(string? message) : base(message)
    {
    }

    public AllListsEmptyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
