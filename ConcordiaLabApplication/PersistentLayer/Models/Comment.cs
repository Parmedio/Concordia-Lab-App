using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Models
{
    public record Comment (int Id = default, string Body = "", DateTime Date = default)
    {
        public virtual Experiment? Experiment { get; set; }
        public int ExperimentId { get; set; }
    }
}
