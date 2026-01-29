using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AnalyticsReport> AnalyticsReports { get; }
    DbSet<Domain.Entities.CoinPair> CoinPairs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
