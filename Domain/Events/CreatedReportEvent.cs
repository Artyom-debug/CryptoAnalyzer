using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class CreatedReportEvent : BaseEvent
{
    public ReportHtml Report { get; }
    public CreatedReportEvent(ReportHtml report) 
    { 
        Report = report;
    }

}
