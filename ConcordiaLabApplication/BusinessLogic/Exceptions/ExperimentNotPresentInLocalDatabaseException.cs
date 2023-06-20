namespace BusinessLogic.Exceptions;

public class ExperimentNotPresentInLocalDatabaseException : Exception
{
    public ExperimentNotPresentInLocalDatabaseException() { }

    public ExperimentNotPresentInLocalDatabaseException(string? message) : base(message) { }

    public ExperimentNotPresentInLocalDatabaseException(string? message, Exception? innerException) : base(message, innerException) { }
}
