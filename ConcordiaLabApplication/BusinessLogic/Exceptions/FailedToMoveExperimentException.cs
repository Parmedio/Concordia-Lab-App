namespace BusinessLogic.Exceptions;

public class FailedToMoveExperimentException : Exception
{
    public FailedToMoveExperimentException()
    {
    }

    public FailedToMoveExperimentException(string? message) : base(message)
    {
    }

    public FailedToMoveExperimentException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
