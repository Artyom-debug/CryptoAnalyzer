using Application.AnaliticsReports.Queries.GetAnalyticsReportWithPagination;
using Application.Common.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Web.Server.Api.Controllers;

[ApiController]
[Authorize]
[Route ("reports")]
public class AnalyticsReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnalyticsReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{coinPairId}")]
    public async Task<ActionResult<AnalyticsReportDto>> GetReport([FromQuery] string timeframe, [FromRoute] Guid coinPairId, [FromQuery] int pageNumber)
    {
        var dto = await _mediator.Send(new GetAnalyticsReportWithPaginationQuery(timeframe, pageNumber, coinPairId));
        if(dto == null)
        {
            return NotFound();
        }
        return Ok(dto);
    }
}
