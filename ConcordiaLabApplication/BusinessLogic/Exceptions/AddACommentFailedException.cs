namespace BusinessLogic.Exceptions;

public class AddACommentFailedException : Exception
{
    public AddACommentFailedException()
    {
    }

    public AddACommentFailedException(string? message) : base(message)
    {
    }

    public AddACommentFailedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
