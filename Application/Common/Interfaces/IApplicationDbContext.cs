using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AnalyticsReport> AnalyticsReports { get; }
    DbSet<CoinPair> CoinPairs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
