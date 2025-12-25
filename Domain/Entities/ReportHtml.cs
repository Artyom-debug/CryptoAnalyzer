using Domain.Common;

namespace Domain.Entities;

public class ReportHtml : BaseAuditableEntity
{
    public string HtmlContent {  get; set; }
    private bool IsCurrent { get; set; } = true;
    public AnalyticsReport Report { get; set; }

    public ReportHtml(Guid id, string content)
    {
        Id = id;
        HtmlContent = content ?? throw new ArgumentNullException(nameof(content)); ;
    }

    public void MarkAsHistorical() => IsCurrent = false;
}
