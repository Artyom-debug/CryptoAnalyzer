using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Common.Dto;

public class IndicatorDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("value")]
    public decimal? Value { get; set; }

    [JsonPropertyName("importance")]
    public double Importance { get; set; }
}

