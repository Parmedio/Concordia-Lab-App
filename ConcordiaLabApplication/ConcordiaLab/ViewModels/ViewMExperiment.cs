namespace ConcordiaLab.ViewModels
{
    public record ViewMExperiment
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Priority { get; set; }
        public string? LastComment { get; set; }
        public string? AuthorComment { get; set; }
        public string BelongToColumn { get; set; } = null!;

        public DateTime? DueDate { get; set; }
        public IEnumerable<ViewMScientist>? Scientists { get; set; }
    }
}
