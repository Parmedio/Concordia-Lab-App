namespace ConcordiaLab.ViewModels
{
    public class ViewMColumn
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<ViewMExperiment>? Experiments { get; set; }
    }
}
