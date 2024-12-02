namespace Shared.RequestFeatures;

public abstract class RequestParameters
{
    private int _pageNumber = 1;
    public int PageNumber
    {
        get => _pageNumber;
        set
        {
            const int minPageNumber = 1;

            switch (value)
            {
                case < minPageNumber:
                    _pageNumber = minPageNumber;
                    return;
                default:
                    _pageNumber = value;
                    break;
            }
        }
    }

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set
        { 
            const int minPageSize = 1;
            const int maxPageSize = 50;
            
            switch (value)
            {
                case < minPageSize:
                    _pageSize = minPageSize;
                    return;
                case > maxPageSize:
                    _pageSize = maxPageSize;
                    return;
                default:
                    _pageSize = value;
                    break;
            }
        }
    }

    private string _orderBy = "name";
    public string OrderBy
    {
        get => _orderBy;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            
            _orderBy = value;
        }
    }

    private string _fields = string.Empty;
    public string Fields
    {
        get => _fields;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            _fields = value;
        }
    }
}