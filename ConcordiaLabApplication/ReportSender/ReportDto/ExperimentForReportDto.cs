namespace BusinessLogic.DTOs.ReportDto;

public record ExperimentForReportDto
{
    public int ColumnId { get; set; }
    public IEnumerable<ScientistInExperimentForReportDto>? Scientists { get; set; }
}

public record ScientistInExperimentForReportDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
