using Application.Common.Dto;
using Application.Common.Interfaces;

namespace Application.AnaliticsReports.Queries.GetAnalyticsReportWithPagination;

public record GetAnalyticsReportWithPaginationQuery(string Timeframe, int PageNumber, Guid CoinPairId) : IRequest<AnalyticsReportDto>;

public class GetAnalyticsReportWithPaginationQueryHandler : IRequestHandler<GetAnalyticsReportWithPaginationQuery, AnalyticsReportDto>
{
    private readonly IApplicationDbContext _context;

    public GetAnalyticsReportWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnalyticsReportDto> Handle(GetAnalyticsReportWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AnalyticsReports.AsNoTracking()
                                              .Include(a => a.CoinPair)
                                              .Where(a => a.CoinPairId == request.CoinPairId && a.Timeframe.Value == request.Timeframe)
                                              .OrderByDescending(a => a.CreatedAt)
                                              .Take(10)
                                              .Skip(request.PageNumber - 1)
                                              .Take(1)
                                              .Select(a => new AnalyticsReportDto
                                              {
                                                  Symbol = a.CoinPair.Symbol,
                                                  Timeframe = request.Timeframe,
                                                  GeneratedAtUtc = a.CreatedAt,
                                                  PredictedCandleOpenUtc = a.CandleOpen,
                                                  PredictedCandleCloseUtc = a.CandleClose,
                                                  Probabilities = new ProbabilitiesDto
                                                  {
                                                      ProbabilityDown = a.Probability.ProbabilityDown,
                                                      ProbabilityUp = a.Probability.ProbabilityUp,
                                                      ProbabilityFlat = a.Probability.ProbabilityFlat,
                                                  },
                                                  Indicators = a.Indicators.Select(i => new IndicatorDto {Importance = i.Importance, Name = i.Name, Value = i.Value}).ToList(),
                                              })
                                              .FirstOrDefaultAsync(cancellationToken);
        Guard.Against.Null(entity);
        return entity;
    }
}




