using PersistentLayer.Models;

namespace BusinessLogic.DataTransferLogic.Abstract;

public interface IExperimentDownloader
{
    Task<(IEnumerable<Experiment>?, string)> DownloadExperiments();
}
