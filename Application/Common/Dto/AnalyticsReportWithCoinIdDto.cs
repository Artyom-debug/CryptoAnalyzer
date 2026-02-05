using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dto;

public class AnalyticsReportWithCoinIdDto
{
    public AnalyticsReportDto Reports { get; set; }
    public Guid CoinPairId { get; set; }
}
