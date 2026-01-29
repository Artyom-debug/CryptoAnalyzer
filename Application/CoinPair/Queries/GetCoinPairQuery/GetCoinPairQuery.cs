using Application.Common.Dto;
using Application.Common.Interfaces;

namespace Application.CoinPair.Queries.GetCoinPairQuery;

public record GetCoinPairQuery(string Symbol) : IRequest<CoinPairDto>;

public class GetCoinPairQueryHandler : IRequestHandler<GetCoinPairQuery, CoinPairDto>
{
    private readonly IApplicationDbContext _context;

    public GetCoinPairQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CoinPairDto> Handle(GetCoinPairQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.CoinPairs.Where(c => c.Symbol == request.Symbol)
                                       .Select(c => new CoinPairDto {CoinPair = request.Symbol, CoinPairId = c.Id})
                                       .FirstOrDefault(cancellationToken);
        Guard.Against.Null(entity);
        return entity;
    }
}


