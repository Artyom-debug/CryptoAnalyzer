using Application.AnaliticsReports.Queries.GetAnaliticsReport;
using Application.Common.Dto;
using Application.Common.Interfaces;

namespace Application.CoinPair.Queries.GetAllCoinPairQuery;

public record GetAllCoinPairQuery() : IRequest<List<CoinPairDto>>;

public class GetAllCoinPairQueryHandler : IRequestHandler<GetAllCoinPairQuery, List<CoinPairDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllCoinPairQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CoinPairDto>> Handle(GetAllCoinPairQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.CoinPairs.ToListAsync(cancellationToken);
        var result = entities.Select(e => new CoinPairDto { CoinPair = e.Symbol, CoinPairId = e.Id }).ToList();
        return result; 
    }
}




