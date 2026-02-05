using Application.AnaliticsReports.Commands.CreateAnaliticsReport;
using Application.Common.Dto;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.AnaliticsReports.Commands.CreateAnalyticsReportRange;

public record CreateAnalyticsReportRangeCommand(List<AnalyticsReportWithCoinIdDto> Reports) : IRequest<Guid>;

public class CreateAnalyticsReportRangeCommandHandler : IRequestHandler<CreateAnalyticsReportRangeCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateAnalyticsReportRangeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAnalyticsReportRangeCommand request, CancellationToken cancellationToken)
    {
        if (request.Reports is null || request.Reports.Count == 0)
            return Guid.Empty;

        var entities = request.Reports.Select(r =>
        {
            var timeframe = new Timeframe(r.Reports.Timeframe);

            var probability = new Probability(
                r.Reports.Probabilities.ProbabilityUp,
                r.Reports.Probabilities.ProbabilityDown,
                r.Reports.Probabilities.ProbabilityFlat);

            var indicators = r.Reports.Indicators
                .Select(i => new Indicator(i.Name, i.Value, i.Importance))
                .ToList();

            return new AnalyticsReport(
                r.CoinPairId,
                timeframe,
                probability,
                r.Reports.GeneratedAtUtc,
                r.Reports.PredictedCandleOpenUtc,
                r.Reports.PredictedCandleCloseUtc,
                indicators);
        }).ToList();

        _context.AnalyticsReports.AddRange(entities);
        await _context.SaveChangesAsync(cancellationToken);

        return entities[^1].Id;
    }
}


