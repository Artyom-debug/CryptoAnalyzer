using Application.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.CoinPair.Queries.GetAllCoinPairQuery;

namespace Web.Server.Api.Controllers;

[ApiController]
public class CoinPairController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoinPairController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("CoinPairs")]
    public async Task<ActionResult<IEnumerable<CoinPairDto>>> GetCoinPairsAsync()
    {
        var coins = await _mediator.Send(new GetAllCoinPairQuery());
        if(coins == null)
        {
            return NotFound();
        }
        return Ok(coins);
    }
}
