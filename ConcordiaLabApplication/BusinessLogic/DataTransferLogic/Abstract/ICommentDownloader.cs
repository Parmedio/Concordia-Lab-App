using BusinessLogic.DataTransferLogic.Concrete;

using PersistentLayer.Models;

namespace BusinessLogic.DataTransferLogic.Abstract;

public interface ICommentDownloader
{
    Task<SyncResult<Comment>> DownloadComments();
}
