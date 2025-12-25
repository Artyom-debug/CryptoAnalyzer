using Application.Common.Interfaces;

namespace Application.AnaliticsReports.Commands.DeteteAnaliticsReport;

public record DeleteAnaliticsReportCommand(Guid Id) : IRequest;

public class DeleteAnaliticsReportCommandHandler : IRequestHandler<DeleteAnaliticsReportCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAnaliticsReportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAnaliticsReportCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AnaliticsReport 
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.AnaliticsReport.Remove(entity);

        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
