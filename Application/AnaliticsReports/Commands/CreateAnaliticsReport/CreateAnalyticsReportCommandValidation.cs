using Application.Common.Dto;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.AnaliticsReports.Commands.CreateAnaliticsReport;

public class CreateAnalyticsReportCommandValidation : AbstractValidator<CreateAnalyticsReportCommand>
{
    public CreateAnalyticsReportCommandValidation()
    {
        RuleFor(x => x.CoinPairId)
            .NotEmpty().WithMessage("CoinPairId is required.");

        RuleFor(x => x.Report)
            .SetValidator(new AnalyticsReportDtoValidation());
    }
}
