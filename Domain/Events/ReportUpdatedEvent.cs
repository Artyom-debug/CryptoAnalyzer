using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class ReportUpdatedEvent : BaseEvent
{
    public ReportHtml Report {  get; }

    public ReportUpdatedEvent(ReportHtml report) =>
        Report = report;
}
