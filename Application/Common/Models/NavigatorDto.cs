namespace Application.Common.Models;

public class NavigatorDto
{
    public string HtmlContent { get; init; }
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public bool HasNewerReport { get; init; }
    public bool HasOlderReport { get; init; }
}
