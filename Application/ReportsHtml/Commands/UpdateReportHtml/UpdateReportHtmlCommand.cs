using Application.Common.Interfaces;
using Domain.Events;

namespace Application.ReportsHtml.Commands.UpdateReportHtml;

public record UpdateReportHtmlCommand : IRequest
{
    public Guid Id { get; init; }
    public string? Content { get; init; }
}

public class UpdateReportHtmlCommandHandler : IRequestHandler<UpdateReportHtmlCommand>
{ 
    private readonly IApplicationDbContext _context;

    public UpdateReportHtmlCommandHandler(IApplicationDbContext context)
    { 
        _context = context; 
    }

    public async Task Handle(UpdateReportHtmlCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ReportHtml
            .FindAsync(new object[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        entity.HtmlContent = request.Content;
        entity.AddDomainEvent(new ReportUpdatedEvent(entity));
        await _context.SaveChangesAsync(cancellationToken);
    }
}

