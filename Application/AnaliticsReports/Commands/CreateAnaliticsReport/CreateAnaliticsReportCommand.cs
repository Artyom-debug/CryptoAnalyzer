using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;

namespace Application.AnaliticsReports.Commands.CreateAnaliticsReport;

public record CreateAnaliticsReportCommand(
    ReportDto report
) : IRequest<Guid>;

public class CreateAnaliticsReportCommandHandler : IRequestHandler<CreateAnaliticsReportCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateAnaliticsReportCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateAnaliticsReportCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<AnalyticsReport>(request);

        _context.AnaliticsReport.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}