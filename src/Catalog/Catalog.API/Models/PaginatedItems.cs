namespace Catalog.API.Models;

public class PaginatedItems<T> where T : class
{
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int Count { get; private set; }
    public IEnumerable<T> Items { get; private set; }

    public PaginatedItems(int pageIndex, int pageSize, int count, IEnumerable<T> items)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Items = items;
    }
}
