namespace Shared.RequestFeatures;

public abstract class RequestParameters
{
    private int _pageNumber = 1;
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = Math.Max(1, value);
    }

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Clamp(value, 1, 50);
    }

    private string _orderBy = "name";
    public string OrderBy
    {
        get => _orderBy;
        set => _orderBy = !string.IsNullOrWhiteSpace(value) ? value : _orderBy;
    }

    private string _fields = string.Empty;
    public string Fields
    {
        get => _fields;
        set => _fields = !string.IsNullOrEmpty(value) ? value : _fields;
    }
}