using Domain.ValueObjects;

namespace Application.Common.Models;

public class IndicatorDto
{
    public string Name { get; init; }
    public double Value { get; init; }
    public string Explanation { get; init; }
    public string Status {  get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Indicator, IndicatorDto>();
            CreateMap<IndicatorDto, Indicator>();
        }
    }
}
