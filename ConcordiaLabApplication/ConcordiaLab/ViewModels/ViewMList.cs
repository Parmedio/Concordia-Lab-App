namespace ConcordiaLab.ViewModels
{
    public class ViewMList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ViewMExperiment>? Experiments { get; set; }
    }
}
