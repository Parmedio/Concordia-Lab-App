namespace BusinessLogic.DataTransferLogic.Abstract
{
    public interface IDataSyncer
    {
        Task<bool> Download();
        Task<bool> Upload();
    }
}