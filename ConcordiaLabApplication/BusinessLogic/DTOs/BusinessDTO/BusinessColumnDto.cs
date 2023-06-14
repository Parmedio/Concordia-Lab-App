namespace BusinessLogic.DTOs.BusinessDTO;

public record BusinessColumnDto
{
    public int Id { get; set; } = 0!;
    public string Name { get; set; } = null!;
    public IEnumerable<BusinessExperimentDto>? Experiments { get; set; }
}
