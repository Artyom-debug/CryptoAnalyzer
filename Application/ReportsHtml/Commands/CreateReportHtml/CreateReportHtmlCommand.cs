using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;

namespace Application.ReportsHtml.Commands.CreateReportHtml;

public record CreateReportHtmlCommand(Guid Id, string Content) : IRequest<Guid>
{ }

public class CreateReportHtmlCommandHandler : IRequestHandler<CreateReportHtmlCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateReportHtmlCommandHandler(IApplicationDbContext context) 
    { 
        _context = context; 
    }

    public async Task<Guid> Handle(CreateReportHtmlCommand request, CancellationToken cancellationToken)
    {
        var entity = new ReportHtml(request.Id, request.Content);
        entity.AddDomainEvent(new CreatedReportEvent(entity));
        _context.ReportHtml.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

