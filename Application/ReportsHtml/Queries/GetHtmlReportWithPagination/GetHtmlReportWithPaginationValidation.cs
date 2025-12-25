namespace Application.ReportsHtml.Queries.GetHtmlReportWithPagination;

public class GetHtmlReportWithPaginationValidation : AbstractValidator<GetHtmlReportWithPaginationQuery>
{
    public GetHtmlReportWithPaginationValidation()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");
    }
}
