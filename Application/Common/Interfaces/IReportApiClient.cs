namespace Application.Common.Interfaces;

public interface IReportApiClient
{
    public Task<string> GetReportJsonAsync(string coinSymbol, CancellationToken cancellationToken);
}
