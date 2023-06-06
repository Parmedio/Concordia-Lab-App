using Microsoft.EntityFrameworkCore.Update.Internal;
using PersistentLayer.Models;
using System.Collections.Generic;

namespace PersistentLayer.Repositories.Abstract;

public interface IExperimentRepository
{
    public IEnumerable<Experiment> Add (IEnumerable<Experiment> experiments);
    public Experiment Add (Experiment experiment);
    public Experiment? Remove (int experimentId);
    //public Experiment Update (int experimentId, int listIdDestination);
    //public Experiment Update(int experimentId, string comment);
    public Experiment? Update (Experiment experiment);
    public IEnumerable<Experiment> GetAll ();
    public Experiment? GetById(int experimentId);
}