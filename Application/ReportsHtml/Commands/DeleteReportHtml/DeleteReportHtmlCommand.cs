using Application.Common.Interfaces;
using Domain.Events;

namespace Application.ReportsHtml.Commands.DeleteReportHtml;

public record DeleteReportHtmlCommand(Guid Id) : IRequest;

public class DeleteReportHtmlCommandHandler : IRequestHandler<DeleteReportHtmlCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteReportHtmlCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteReportHtmlCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ReportHtml
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.ReportHtml.Remove(entity);

        entity.AddDomainEvent(new DeletedReportEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
