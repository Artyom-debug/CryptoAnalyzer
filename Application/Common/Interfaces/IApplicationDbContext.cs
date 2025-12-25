using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AnalyticsReport> AnaliticsReport { get; }
    DbSet<ReportHtml> ReportHtml { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
