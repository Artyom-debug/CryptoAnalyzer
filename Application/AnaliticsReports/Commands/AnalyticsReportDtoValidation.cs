using Application.AnaliticsReports.Commands.CreateAnaliticsReport;
using Application.Common.Dto;

namespace Application.AnaliticsReports.Commands;

public class AnalyticsReportDtoValidation : AbstractValidator<AnalyticsReportDto>
{
    public AnalyticsReportDtoValidation()
    {

        RuleFor(x => x.Timeframe)
            .NotEmpty().WithMessage("Timeframe is required.")
            .Must(BeValidTimeframe).WithMessage("Timeframe must look like '15m', '1h', '4h', '1d'.");

        RuleFor(x => x.GeneratedAtUtc)
            .Must(BeUtc).WithMessage("GeneratedAtUtc must be UTC (offset 0 or DateTimeKind.Utc).");

        RuleFor(x => x.PredictedCandleOpenUtc)
            .Must(BeUtc).WithMessage("PredictedCandleOpenUtc must be UTC.");

        RuleFor(x => x.PredictedCandleCloseUtc)
            .Must(BeUtc).WithMessage("PredictedCandleCloseUtc must be UTC.");

        RuleFor(x => x)
            .Must(r => r.PredictedCandleCloseUtc > r.PredictedCandleOpenUtc)
            .WithMessage("Predicted candle close must be greater than open.");

        RuleFor(x => x.Probabilities)
            .NotNull().WithMessage("Probabilities is required.");

        RuleFor(x => x.Probabilities.ProbabilityUp)
            .InclusiveBetween(0m, 1m);

        RuleFor(x => x.Probabilities.ProbabilityFlat)
            .InclusiveBetween(0m, 1m);

        RuleFor(x => x.Probabilities.ProbabilityDown)
            .InclusiveBetween(0m, 1m);

        RuleFor(x => x.Probabilities)
            .Must(p => SumIsOne(p.ProbabilityUp, p.ProbabilityFlat, p.ProbabilityDown))
            .WithMessage("Probabilities must sum to ~1 (tolerance ±0.001).");

        RuleForEach(x => x.Indicators).SetValidator(new IndicatorDtoValidator());

        RuleFor(x => x.Indicators)
            .Must(HaveUniqueNames)
            .WithMessage("Indicator names must be unique (case-insensitive).");
    }

    private static bool BeUtc(DateTimeOffset dto) => dto.Offset == TimeSpan.Zero;

    private static bool BeValidTimeframe(string tf)
    {
        tf = tf.Trim().ToLowerInvariant();
        if (tf.Length < 2) return false;

        var suffix = tf[^1];
        if (suffix is not ('m' or 'h' or 'd')) return false;

        return int.TryParse(tf[..^1], out var n) && n > 0;
    }

    private static bool SumIsOne(decimal up, decimal flat, decimal down)
    {
        var sum = up + flat + down;
        return Math.Abs(sum - 1m) <= 0.001m;
    }

    private static bool HaveUniqueNames(IList<IndicatorDto> indicators)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var i in indicators)
        {
            if (i?.Name is null) return false;
            if (!set.Add(i.Name.Trim())) return false;
        }
        return true;
    }
}

public class IndicatorDtoValidator : AbstractValidator<IndicatorDto>
{
    public IndicatorDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Indicator name is required")
            .Must(n => !string.IsNullOrWhiteSpace(n)).WithMessage("Indicator name cannot be whitespace.");

        RuleFor(x => x.Importance)
            .InclusiveBetween(0.0, 1.0)
            .WithMessage("Importance must be between 0 and 1.");

        When(x => x.Value.HasValue, () =>
        {
            RuleFor(x => x.Value!.Value)
                .InclusiveBetween(-1_000_000_000m, 1_000_000_000m)
                .WithMessage("Value is out of allowed range.");
        });
    }
}
