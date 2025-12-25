using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IHtmlReportGenerator
{
    public Task<string> GenerateHtmlReportAsync(AnalyticsReport report);
}
