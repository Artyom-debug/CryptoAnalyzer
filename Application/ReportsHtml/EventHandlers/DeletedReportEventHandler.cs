using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.ReportsHtml.EventHandlers;

public class DeletedReportEventHandler : INotificationHandler<DeletedReportEvent>
{
    private readonly ILogger<DeletedReportEvent> _logger;

    public DeletedReportEventHandler(ILogger<DeletedReportEvent> logger) =>
        _logger = logger;

    public Task Handle(DeletedReportEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Application Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
