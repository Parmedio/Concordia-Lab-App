namespace BusinessLogic.DataTransferLogic.Abstract;

public interface ICommentDownloader
{
    Task<IEnumerable<int>?> DownloadComments();
}
