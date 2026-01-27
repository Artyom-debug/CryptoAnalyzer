using Application.Common.Dto;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.AnaliticsReports.Commands.CreateAnaliticsReport;

public record CreateAnalyticsReportCommand(AnalyticsReportDto Report, Guid CoinPairId) : IRequest<Guid>;

public class CreateAnalyticsReportCommandHandler : IRequestHandler<CreateAnalyticsReportCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateAnalyticsReportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAnalyticsReportCommand request, CancellationToken cancellationToken)
    {
        var Timeframe = new Timeframe(request.Report.Timeframe);
        var Probability = new Probability(request.Report.Probabilities.ProbabilityUp, request.Report.Probabilities.ProbabilityDown, request.Report.Probabilities.ProbabilityFlat);
        var Indicators = request.Report.Indicators.Select(i => new Indicator(i.Name, i.Value, i.Importance)).ToList();
        var entity = new AnalyticsReport(request.CoinPairId,
                                         Timeframe,
                                         Probability,
                                         request.Report.GeneratedAtUtc,
                                         request.Report.PredictedCandleOpenUtc,
                                         request.Report.PredictedCandleCloseUtc,
                                         Indicators
                                         );

        _context.AnalyticsReports.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}