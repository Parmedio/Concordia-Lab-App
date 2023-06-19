using BusinessLogic.DataTransferLogic.Concrete;

using PersistentLayer.Models;

namespace BusinessLogic.DataTransferLogic.Abstract;

public interface IUploader
{
    public Task<SyncResult<Experiment>> Upload();
}
