using Application.Common.Interfaces;
using Application.Common.Models;

namespace Application.AnaliticsReports.Queries.GetAnaliticsReport;

public record GetAnaliticsReportQuery(Guid Id) : IRequest<ReportDto>
{ }

public class GetAnaliticsReportQueryHandler : IRequestHandler<GetAnaliticsReportQuery, ReportDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAnaliticsReportQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReportDto> Handle(GetAnaliticsReportQuery request, CancellationToken cancellationToken)
    {
        var entity =  await _context.AnaliticsReport.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        return _mapper.Map<ReportDto>(entity);
    }
}

