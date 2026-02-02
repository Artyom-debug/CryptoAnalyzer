using Application.Common.Dto;

namespace Application.Common.Interfaces;

public interface IReportApiClient
{
    public Task<IReadOnlyList<AnalyticsReportDto>> GetReportJsonAsync(string timeframe, string[] coinPairs, CancellationToken cancellationToken);
}
