namespace Application.AnaliticsReports.Queries.GetAnalyticsReportWithPagination;

public class GetAnalyticsReportWithPaginationQueryValidation : AbstractValidator<GetAnalyticsReportWithPaginationQuery>
{
    public GetAnalyticsReportWithPaginationQueryValidation()
    {
        RuleFor(x => x.CoinPairId)
            .NotEmpty().WithMessage("CoinPair Id is required");

        RuleFor(x => x.PageNumber)
            .InclusiveBetween(1, 10)
            .WithMessage("PageNumber must be between 1 and 10.");

        RuleFor(x => x.Timeframe)
            .NotEmpty().WithMessage("Timeframe is required.")
            .Must(BeValidTimeframe).WithMessage("Timeframe must look like '15m', '1h', '4h', '1d'.");
    }

    private static bool BeValidTimeframe(string tf)
    {
        tf = tf.Trim().ToLowerInvariant();
        if (tf.Length < 2) return false;

        var suffix = tf[^1];
        if (suffix is not ('m' or 'h' or 'd')) return false;

        return int.TryParse(tf[..^1], out var n) && n > 0;
    }
}
