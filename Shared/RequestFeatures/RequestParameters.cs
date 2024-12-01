namespace Shared.RequestFeatures;

public abstract class RequestParameters
{
    private const int MinPageNumber = 1;
    
    private const int MinPageSize = 1;
    private const int MaxPageSize = 50;

    private int _pageNumber = 1;
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < MinPageNumber ? MinPageNumber : value;
    }

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set
        {
            switch (value)
            {
                case < MinPageSize:
                    _pageSize = MinPageSize;
                    return;
                case > MaxPageSize:
                    _pageSize = MaxPageSize;
                    return;
                default:
                    _pageSize = value;
                    break;
            }
        }
    }
}