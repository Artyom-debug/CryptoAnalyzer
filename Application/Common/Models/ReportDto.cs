using Domain.Entities;

namespace Application.Common.Models;

public class ReportDto
{
    public string Coin { get; init; }
    public decimal CurrentPrice { get; init; }
    public double ChangePercent {  get; init; }
    public List<IndicatorDto> Indicators { get; init; }
    public RecommendationDto Summary { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AnalyticsReport, ReportDto>();
            CreateMap<ReportDto, AnalyticsReport>();
        }
    }
}
