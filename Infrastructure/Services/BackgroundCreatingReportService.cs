using Application.AnaliticsReports.Commands.CreateAnalyticsReportRange;
using Application.CoinPair.Queries.GetAllCoinPairQuery;
using Application.Common.Dto;
using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Services;

public class BackgroundCreatingReportService : IJob
{
    private readonly IReportApiClient _reportApiClient;
    private readonly ILogger<BackgroundCreatingReportService> _logger;
    private readonly IMediator _mediator;

    public BackgroundCreatingReportService(IReportApiClient reportApi, IMediator mediator, ILogger<BackgroundCreatingReportService> logger)
    {
        _reportApiClient = reportApi;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var ct = context.CancellationToken;
        try
        {
            var timeframe = context.MergedJobDataMap.GetString("timeframe");
            Guard.Against.NullOrEmpty(timeframe);

            var coins = await _mediator.Send(new GetAllCoinPairQuery(), ct);
            Guard.Against.NullOrEmpty(coins);

            var coinIdByName = coins
                .Where(c => !string.IsNullOrWhiteSpace(c.CoinPair))
                .ToDictionary(c => c.CoinPair!.Trim().ToUpperInvariant(), c => c.CoinPairId);

            var coinsString = coins
                .Where(c => !string.IsNullOrWhiteSpace(c.CoinPair))
                .Select(c => c.CoinPair!.Trim())
                .ToArray();

            var jsonReports = await _reportApiClient.GetReportJsonAsync(timeframe, coinsString!, ct);
            List<AnalyticsReportWithCoinIdDto> reports = new(jsonReports.Count);

            foreach (var rep in jsonReports)
            {
                var key = rep.Symbol.Trim().ToUpperInvariant();

                if (!coinIdByName.TryGetValue(key, out var coinPairId))
                {
                    _logger.LogWarning("Unknown coin pair: {Symbol}", rep.Symbol);
                    continue;
                }

                reports.Add(new AnalyticsReportWithCoinIdDto
                {
                    Reports = rep,
                    CoinPairId = coinPairId
                });
            }

            if (reports.Count == 0)
            {
                _logger.LogWarning("No reports to save for timeframe {Timeframe}", timeframe);
                return;
            }

            await _mediator.Send(new CreateAnalyticsReportRangeCommand(reports), ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        { }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in BackgroundCreatingReportService");
            throw;
        }
    }
}
