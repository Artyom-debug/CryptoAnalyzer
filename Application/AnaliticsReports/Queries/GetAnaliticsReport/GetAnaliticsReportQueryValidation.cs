namespace Application.AnaliticsReports.Queries.GetAnaliticsReport;

public class GetAnaliticsReportQueryValidation : AbstractValidator<GetAnaliticsReportQuery>
{
    public GetAnaliticsReportQueryValidation() 
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Report ID is required.");
    }
}
