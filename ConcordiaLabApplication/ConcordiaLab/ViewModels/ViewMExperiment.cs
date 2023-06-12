namespace ConcordiaLab.ViewModels
{
    public record ViewMExperiment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Priority { get; set; }
        public string? LastComment { get; set; }
        public string BelongToList { get; set; }
        public IEnumerable<ViewMScientist>? Scientists { get; set; }
    }
}
