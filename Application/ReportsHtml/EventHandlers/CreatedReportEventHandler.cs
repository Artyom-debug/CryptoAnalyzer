using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.ReportsHtml.EventHandlers;

public class CreatedReportEventHandler : INotificationHandler<CreatedReportEvent>
{
    private readonly ILogger<CreatedReportEvent> _logger;

    public CreatedReportEventHandler(ILogger<CreatedReportEvent> logger) =>
        _logger = logger;

    public Task Handle(CreatedReportEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Application Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
