using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace ConcordiaLab
{
    public class ExperimentDataAccess
    {
        private readonly IExperimentRepository _experimentRepository;

        public ExperimentDataAccess(IExperimentRepository experimentRepository)
        {
            _experimentRepository = experimentRepository;
        }

        public IEnumerable<Experiment> Add(IEnumerable<Experiment> experiments)
        {
            return _experimentRepository.Add(experiments);
        }
        public Experiment Add(Experiment experiment)
        {
            return _experimentRepository.Add(experiment);
        }
        public Experiment? Remove(int experimentId)
        {
            return _experimentRepository.Remove(experimentId);
        }
        public Experiment? Update(Experiment experiment)
        {
            return _experimentRepository?.Update(experiment);  
        }
        public IEnumerable<Experiment> GetAll()
        {
            return _experimentRepository.GetAll();
        }
        public Experiment? GetById(int experimentId)
        {
            return _experimentRepository.GetById(experimentId);
        }
    }
}
