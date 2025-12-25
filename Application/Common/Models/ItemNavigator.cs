namespace Application.Common.Models;

public class ItemNavigator<T>
{
    public T Item { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }

    public ItemNavigator(T items, int count, int pageNumber)
    {
        PageNumber = pageNumber;
        TotalPages = count;
        Item = items;
    }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<ItemNavigator<T>> CreateAsync(IQueryable<T> source, int pageNumber, CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var item = await source.Skip(pageNumber - 1).FirstOrDefaultAsync(cancellationToken);
        if (item == null)
            throw new KeyNotFoundException($"Report with page number {pageNumber} not found");

        return new ItemNavigator<T>(item, count, pageNumber);
    }
}
