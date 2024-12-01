namespace Shared.RequestFeatures;

public abstract class RequestParameters
{
    private const int MinPageSize = 1;
    private const int MaxPageSize = 50;

    public int PageNumber { get; set; } = 1;
    
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