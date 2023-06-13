using Newtonsoft.Json;

namespace BusinessLogic.DTOs.TrelloDtos;

public record TrelloExperimentDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public List<string>? IdMembers { get; set; }
    public DateTime? Due { get; set; }
    public List<string>? IdLabels { get; set; }

    [JsonProperty(PropertyName = "idList")]
    public string TrelloListId { get; set; } = null!;
    public string? Desc { get; set; }
}
