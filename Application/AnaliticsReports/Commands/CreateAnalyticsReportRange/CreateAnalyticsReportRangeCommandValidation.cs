using Application.AnaliticsReports.Commands.CreateAnaliticsReport;
using Application.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AnaliticsReports.Commands.CreateAnalyticsReportRange;

public sealed class CreateAnalyticsReportRangeCommandValidation : AbstractValidator<CreateAnalyticsReportRangeCommand>
{
    public CreateAnalyticsReportRangeCommandValidation()
    {
        RuleFor(x => x.Reports)
            .NotNull().WithMessage("Reports is required.")
            .NotEmpty().WithMessage("Reports must not be empty.");

        RuleForEach(x => x.Reports)
            .SetValidator(new AnalyticsReportWithCoinIdDtoValidator());

    }
}

public class AnalyticsReportWithCoinIdDtoValidator : AbstractValidator<AnalyticsReportWithCoinIdDto>
{
    public AnalyticsReportWithCoinIdDtoValidator()
    {
        RuleFor(x => x.CoinPairId)
            .NotEmpty().WithMessage("CoinPairId is required.");

        RuleFor(x => x.Reports)
            .NotNull().WithMessage("Report is required.")
            .SetValidator(new AnalyticsReportDtoValidation());
    }
}