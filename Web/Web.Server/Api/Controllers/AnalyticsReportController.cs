using Application.AnaliticsReports.Queries.GetAnalyticsReportWithPagination;
using Application.Common.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Web.Server.Api.Controllers;

[ApiController]
public class AnalyticsReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnalyticsReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{timeframe}/{coinPair}")]
    public async Task<ActionResult<AnalyticsReportDto>> GetReport(string timeframe, string coinPair, Guid coinPairId, int pageNumber)
    {
        //if (string.IsNullOrWhiteSpace(timeframe) || string.IsNullOrWhiteSpace(coinPair))
        //{
        //    return NotFound();
        //}
        var dto = await _mediator.Send(new GetAnalyticsReportWithPaginationQuery(timeframe, pageNumber, coinPairId));
        if(dto == null)
        {
            return NotFound();
        }
        return Ok(dto);
    }
}
