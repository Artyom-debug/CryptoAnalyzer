using Application.Common.Dto;
using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Infrastructure.Services;

public record Request(string Timeframe, string[] CoinPairs);

public class ReportApiClient : IReportApiClient
{
    private readonly ILogger<ReportApiClient> _logger;
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
    { 
        PropertyNameCaseInsensitive = true,
    };


    public ReportApiClient(ILogger<ReportApiClient> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<AnalyticsReportDto>> GetReportJsonAsync(string timeframe, string[] coinPairs, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(timeframe))
            throw new ArgumentException("Timeframe is required.", nameof(timeframe));
        if (coinPairs is null || coinPairs.Length == 0)
            return new List<AnalyticsReportDto>();
        var request = new Request(timeframe, coinPairs);
        try
        {
            _logger.LogInformation("Sending request to endpoint {Endpoint}. Timeframe={Timeframe}, CoinPairs={Count}", "generateReports", timeframe, coinPairs.Length);
            using var response = await _httpClient.PostAsJsonAsync("generateReports", request, jsonOptions, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<AnalyticsReportDto>>(jsonOptions, cancellationToken);
            return result ?? new List<AnalyticsReportDto>();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Request to prediction service was cancelled.");
            throw;
        }
        catch (JsonException ex) 
        {
            _logger.LogError(ex, "Invalid JSON returned by prediction service.");
            return new List<AnalyticsReportDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while calling prediction service.");
            return new List<AnalyticsReportDto>();
        }
    }
}
