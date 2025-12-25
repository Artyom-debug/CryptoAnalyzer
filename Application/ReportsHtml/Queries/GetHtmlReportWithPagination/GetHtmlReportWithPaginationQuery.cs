using Application.Common.Interfaces;
using Domain.Entities;
using Application.Common.Models;

namespace Application.ReportsHtml.Queries.GetHtmlReportWithPagination;

public record GetHtmlReportWithPaginationQuery : IRequest<NavigatorDto>
{
    public int PageNumber { get; init; } = 1;
}

public class GetHtmlReportWithPaginationQueryHandler : IRequestHandler<GetHtmlReportWithPaginationQuery, NavigatorDto>
{
    private readonly IApplicationDbContext _context;

    public GetHtmlReportWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<NavigatorDto> Handle(GetHtmlReportWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var source = _context.ReportHtml.AsNoTracking().Include(reportHtml => reportHtml.Report).OrderByDescending(x => x.Created); //составление чертежа запроса
        var navigator = await ItemNavigator<ReportHtml>.CreateAsync(source, request.PageNumber, cancellationToken);
        return new NavigatorDto
        {
            HtmlContent = navigator.Item.HtmlContent,
            CurrentPage = navigator.PageNumber,
            TotalPages = navigator.TotalPages,
            HasNewerReport = navigator.HasPreviousPage,
            HasOlderReport = navigator.HasNextPage
        };
    }
}

