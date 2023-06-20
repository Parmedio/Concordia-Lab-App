namespace ReportSender.ReportDto;

public record ScientistForReportDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public IEnumerable<ExperimentOfScientistDto> Experiments { get; set; } = new List<ExperimentOfScientistDto>();
}

public record ExperimentOfScientistDto
{
    public int ColumnId { get; set; }
}
