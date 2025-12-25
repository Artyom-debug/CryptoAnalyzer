using Application.Common.Interfaces;
using Application.Common.Models;
using System.Text.Json;

namespace Infrastructure.Services;

public class ReportJsonParser : IReportJsonParser
{
    public ReportDto ParseJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON cannot be null or empty", nameof(json));
        var rawReport = JsonSerializer.Deserialize<ReportDto>(json);
        return rawReport ?? throw new InvalidOperationException("JSON deserialization returned null. Check the JSON format and DTO structure.");
    }
}
