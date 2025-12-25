using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class DeletedReportEvent : BaseEvent
{
    public ReportHtml Report { get; }

    public DeletedReportEvent(ReportHtml report) =>
        Report = report;
}
