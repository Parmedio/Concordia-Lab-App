namespace BusinessLogic.Exceptions;

public class LocalCommentWithoutScientistException : Exception
{
    public LocalCommentWithoutScientistException() { }

    public LocalCommentWithoutScientistException(string? message) : base(message) { }

    public LocalCommentWithoutScientistException(string? message, Exception? innerException) : base(message, innerException) { }
}
