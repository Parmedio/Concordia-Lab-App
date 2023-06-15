using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Abstract
{
    public interface IClientService
    {
        public BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId);
        public IEnumerable<BusinessColumnDto> GetAllLists();
        public IEnumerable<BusinessColumnDto> GetAllLists(int scientistId);
        public IEnumerable<BusinessColumnDto> GetAllSimple();
        public IEnumerable<BusinessExperimentDto> GetAllExperiments();
        public IEnumerable<BusinessExperimentDto> GetAllExperiments(int scientistId);
        public IEnumerable<BusinessScientistDto> GetAllScientist();
        public BusinessExperimentDto GetExperimentById(int experimentId);
        public BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto);
        public Task SyncDataAsyncs();
        public Task<bool> UpdateConnectionStateAsync(bool connectionState);
    }
}