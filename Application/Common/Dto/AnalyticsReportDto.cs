using System.Text.Json.Serialization;
using Application.Common.Dto;


namespace Application.Common.Dto;

public class AnalyticsReportDto
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("timeframe")]
    public string Timeframe { get; set; } = null!;

    [JsonPropertyName("generated_at_utc")]
    public DateTimeOffset GeneratedAtUtc { get; set; }

    [JsonPropertyName("predicted_candle_open_utc")]
    public DateTimeOffset PredictedCandleOpenUtc { get; set; }

    [JsonPropertyName("predicted_candle_close_utc")]
    public DateTimeOffset PredictedCandleCloseUtc { get; set; }

    [JsonPropertyName("probabilities")]
    public ProbabilitiesDto Probabilities { get; set; } = null!;

    [JsonPropertyName("indicators")]
    public List<IndicatorDto> Indicators { get; set; } = new();
}
