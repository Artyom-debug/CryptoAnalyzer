using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.ReportsHtml.EventHandlers;

public class UpdatedReportEventHandler : INotificationHandler<ReportUpdatedEvent>
{
    private readonly ILogger<ReportUpdatedEvent> _logger;

    public UpdatedReportEventHandler(ILogger<ReportUpdatedEvent> logger) =>
        _logger = logger;

    public Task Handle(ReportUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Application Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
