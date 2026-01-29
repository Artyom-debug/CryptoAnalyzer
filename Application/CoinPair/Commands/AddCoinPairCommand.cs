using Application.Common.Interfaces;

namespace Application.CoinPair.Commands;

public record AddCoinPairCommand(string CoinPairName) : IRequest<Guid>;

public class AddCoinPairCommandHandler : IRequestHandler<AddCoinPairCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public AddCoinPairCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(AddCoinPairCommand command, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.CoinPair(command.CoinPairName);
        Guid id = entity.Id;
        _context.CoinPairs.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return id;
    }
}

