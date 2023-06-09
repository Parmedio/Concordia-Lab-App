namespace ConcordiaLab.Controllers.Mock_Data.MockModels
{
    public record MockExperiment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Priority { get; set; }
        public string? LastComment { get; set; }
        public int? BelongToList { get; set; }
        public IEnumerable<int>? IntScientists { get; set; }
        public IEnumerable<MockScientist>? Scientists { get; set; }

        public MockExperiment(int id, string title, string description, DateTime? dueDate, string? priority, string? lastComment, IEnumerable<int>? scientist)
        {
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            LastComment = lastComment;
            IntScientists = scientist;
        }

        public MockExperiment(int id, string title, string description, string? priority, string? lastComment, IEnumerable<int>? scientist)
        {
            Id = id;
            Title = title;
            Description = description;
            Priority = priority;
            LastComment = lastComment;
            IntScientists = scientist;
        }
    }
}
