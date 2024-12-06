namespace Shared.RequestFeatures;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; set; }

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData
        {
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            PageSize = pageSize,
            TotalCount = count
        };

        AddRange(items);
    }

    public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        const int minimalValue = 1;
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfLessThan(value: pageNumber, other: minimalValue);
        ArgumentOutOfRangeException.ThrowIfLessThan(value: pageSize, other: minimalValue);
        
        var enumerable = source.ToList();
        
        var count = enumerable.Count;
        var items = enumerable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToList();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}