namespace BusinessLogic.Exceptions;

public class DownloadFailedException : Exception
{
    public DownloadFailedException() { }

    public DownloadFailedException(string? message) : base(message) { }

    public DownloadFailedException(string? message, Exception? innerException) : base(message, innerException) { }
}
