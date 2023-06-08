using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Abstract
{
    public interface IClientService
    {
        BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId);
        IEnumerable<BusinessListDto> GetAllLists(int scientistId);
        BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto);
        Task SyncDataAsyncs();
        Task<bool> UpdateConnectionStateAsync(bool connectionState);
    }
}