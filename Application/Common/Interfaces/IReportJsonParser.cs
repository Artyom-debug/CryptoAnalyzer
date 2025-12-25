using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IReportJsonParser
{
    public ReportDto ParseJson(string json);
}
