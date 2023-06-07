namespace BusinessLogic.DTOs.BusinessDTO;

public record BusinessListDto
{
    public int Id { get; set; } = 0!;
    public IEnumerable<BusinessExperimentDto>? Experiments { get; set; }

}
