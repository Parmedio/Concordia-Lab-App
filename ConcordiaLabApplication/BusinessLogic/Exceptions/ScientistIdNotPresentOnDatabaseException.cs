namespace BusinessLogic.Exceptions;

public class ScientistIdNotPresentOnDatabaseException : Exception
{
    public ScientistIdNotPresentOnDatabaseException()
    {
    }

    public ScientistIdNotPresentOnDatabaseException(string? message) : base(message)
    {
    }

    public ScientistIdNotPresentOnDatabaseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }


}
