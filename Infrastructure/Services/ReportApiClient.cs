using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class ReportApiSettings
{
    public string? BaseUrl { get; set; }
}

public class ReportApiClient : IReportApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly ReportApiSettings _settings;

    public ReportApiClient(HttpClient httpClient, ILogger logger, IOptions<ReportApiSettings> settings)
    {
        _httpClient = httpClient;
        _logger = logger;
        _settings = settings.Value;
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
    }

    public async Task<string> GetReportJsonAsync(string coinSymbol, CancellationToken cancellationToken = default)
    {
        var endpoint = $"api/reports/{coinSymbol}";
        _logger.LogInformation($"Request for address {endpoint}.");
        try
        {
            var response = await _httpClient.GetAsync(endpoint, cancellationToken);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Data successfully received.");
            return responseBody;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"Http request error. Status code {ex.StatusCode}");
            throw;
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            throw;
        }
    }
}
