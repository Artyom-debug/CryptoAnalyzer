using System.Text.Json.Serialization;

namespace Application.Common.Dto;

public class ProbabilitiesDto
{
    [JsonPropertyName("UP")]
    public decimal ProbabilityUp { get; set; }

    [JsonPropertyName("FLAT")]
    public decimal ProbabilityFlat { get; set; }

    [JsonPropertyName("DOWN")]
    public decimal ProbabilityDown { get; set; }
}
