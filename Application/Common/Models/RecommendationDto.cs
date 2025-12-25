using Domain.ValueObjects;

namespace Application.Common.Models;

public class RecommendationDto
{
    public string Action { get; init; }
    public double Confidence { get; init; }
    public string Risk { get; init; }
    public string Summary { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Recommendation, RecommendationDto>();
            CreateMap<RecommendationDto, Recommendation>();
        }
    }
}
