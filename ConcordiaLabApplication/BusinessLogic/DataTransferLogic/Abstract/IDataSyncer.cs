using PersistentLayer.Models;

namespace BusinessLogic.DataTransferLogic.Abstract
{
    public interface IDataSyncer
    {
        Task<(IEnumerable<int>?, IEnumerable<Experiment>?)> Download();
        Task<bool> Upload();
    }
}