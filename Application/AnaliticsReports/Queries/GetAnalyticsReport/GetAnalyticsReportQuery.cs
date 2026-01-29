using Application.Common.Dto;
using Application.Common.Interfaces;

namespace Application.AnaliticsReports.Queries.GetAnalyticsReport;

public record GetAnalyticsReportQuery(Guid Id) : IRequest<AnalyticsReportDto>;

public class GetAnalyticsReportQueryHandler : IRequestHandler<GetAnalyticsReportQuery, AnalyticsReportDto>
{
    private readonly IApplicationDbContext _context;

    public GetAnalyticsReportQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnalyticsReportDto> Handle(GetAnalyticsReportQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AnalyticsReports.Where(a => a.Id == request.Id)
                                                     .Select(a => new AnalyticsReportDto
                                                     {
                                                         Symbol = a.CoinPair.Symbol,
                                                         Timeframe = a.Timeframe.Value,
                                                         GeneratedAtUtc = a.CreatedAt,
                                                         PredictedCandleOpenUtc = a.CandleOpen,
                                                         PredictedCandleCloseUtc = a.CandleClose,
                                                         Probabilities = new ProbabilitiesDto
                                                         {
                                                             ProbabilityDown = a.Probability.ProbabilityDown,
                                                             ProbabilityUp = a.Probability.ProbabilityUp,
                                                             ProbabilityFlat = a.Probability.ProbabilityFlat,
                                                         },
                                                         Indicators = a.Indicators.Select(i => new IndicatorDto { Importance = i.Importance, Name = i.Name, Value = i.Value }).ToList(),
                                                     }).FirstOrDefaultAsync(cancellationToken);
        Guard.Against.Null(entity);
        return entity;
    }
}

