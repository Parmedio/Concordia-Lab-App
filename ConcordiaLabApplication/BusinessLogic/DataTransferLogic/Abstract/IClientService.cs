using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Abstract
{
    public interface IClientService
    {
        BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId);
        IEnumerable<BusinessListDto> GetAllLists();
        IEnumerable<BusinessListDto> GetAllLists(int scientistId);
        IEnumerable<BusinessExperimentDto> GetAllExperiments();
        IEnumerable<BusinessExperimentDto> GetAllExperiments(int scientistId);
        IEnumerable<BusinessScientistDto> GetAllScientist();
        BusinessExperimentDto GetExperimentById(int experimentId);
        BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto);
        Task SyncDataAsyncs();
        Task<bool> UpdateConnectionStateAsync(bool connectionState);
    }
}