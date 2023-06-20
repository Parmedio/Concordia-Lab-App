using BusinessLogic.DataTransferLogic.Concrete;

using PersistentLayer.Models;

namespace BusinessLogic.DataTransferLogic.Abstract;

public interface IExperimentDownloader
{
    Task<SyncResult<Experiment>> DownloadExperiments();
}
