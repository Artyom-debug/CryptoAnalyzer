using Application.Common.Dto;

namespace Application.Common.Interfaces;

public interface IReportJsonParser
{
    public AnalyticsReportDto ParseJson(string json);
}
