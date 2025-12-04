namespace GesAppro.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageIndex { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int TotalCount { get; }

    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items.ToList();
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}
