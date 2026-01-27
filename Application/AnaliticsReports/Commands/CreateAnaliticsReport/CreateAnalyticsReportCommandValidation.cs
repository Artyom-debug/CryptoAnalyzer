using Application.Common.Models;
using Domain.ValueObjects;

namespace Application.AnaliticsReports.Commands.CreateAnaliticsReport;

public class CreateAnaliticsReportCommandValidation : AbstractValidator<CreateAnaliticsReportCommand>
{
    public CreateAnaliticsReportCommandValidation()
    {
        RuleFor(v => v.report.Coin)
                .NotEmpty().WithMessage("Coin name is required.")
                .MaximumLength(20).WithMessage("The coin name must not exceed 10 characters..");
        RuleFor(v => v.report.CurrentPrice)
            .GreaterThan(0).WithMessage("The price must be greater than zero.");
        RuleFor(v => v.report.ChangePercent)
            .InclusiveBetween(-100, 100).WithMessage("The percentage change must be between -100 and 100.");
        RuleFor(v => v.report.Indicators)
            .NotNull()
            .Must(HaveUniqueNames).WithMessage("Indicators must have unique names.");
        RuleForEach(v => v.report.Indicators)
            .SetValidator(new IndicatorDtoValidation());
        RuleFor(v => v.report.Summary)
            .NotNull()
            .SetValidator(new RecommendationDtoValidation());
            
    }

    private bool HaveUniqueNames(List<IndicatorDto> list)
    {
        return list.Select(x => x.Name).Distinct().Count() == list.Count;
    }
}

public class RecommendationDtoValidation : AbstractValidator<RecommendationDto>
{
    public RecommendationDtoValidation() 
    {
        RuleFor(v => v.Action)
            .NotNull();
        RuleFor(v => v.Confidence)
            .InclusiveBetween(0, 1);
        RuleFor(v => v.Risk)
            .NotNull();
        RuleFor(v => v.Summary)
            .NotNull();
    }
}

public class IndicatorDtoValidation : AbstractValidator<IndicatorDto>
{
    public IndicatorDtoValidation()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Indicator name is required.");
        RuleFor(v => v.Status)
            .NotNull();
        RuleFor(v => v.Explanation)
            .NotNull();
    }
}


