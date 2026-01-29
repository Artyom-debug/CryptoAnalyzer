using Application.AnaliticsReports.Queries.GetAnalyticsReport;

namespace Application.AnaliticsReports.Queries.GetAnaliticsReport;

public class GetAnalyticsReportQueryValidation : AbstractValidator<GetAnalyticsReportQuery>
{
    public GetAnalyticsReportQueryValidation() 
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Report ID is required.");
    }
}
