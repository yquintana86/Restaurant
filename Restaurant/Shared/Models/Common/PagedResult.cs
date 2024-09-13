namespace SharedLib.Models.Common;

public abstract class PagedResult
{
    public int Currentpage { get; set; }
    public int? PageCount { get; set; }
    public int PageSize { get; set; }
    public int ItemCount { get; set; }
    public int? TotalItemCount { get; set; }
    public bool HasNextPage { get; set; }
    public int DisplayFrom => ItemCount > 0 ? (Currentpage - 1) * PageSize + 1 : 0;
    public int DisplayTo => ItemCount > 0 ? DisplayFrom + ItemCount - 1 : 0;
    public int? DisplayTotal => TotalItemCount.HasValue ? TotalItemCount.Value : HasNextPage ? int.MaxValue : DisplayTo;
}

public class PagedResult<T> : PagedResult
{
    private List<T>? _results;
    private readonly ApiOperationError? _error;

    public PagedResult()
    {
        PageSize = 25;
        Currentpage = 1;
        _results = null;
        _error = null;
    }

    public PagedResult(ApiOperationError? error)
    {
        _error = error;
    }

    public IList<T>? Results
    {
        get { return _results; }
        init { _results = value is null ? 
                                   null : 
                                   new List<T>(value); }
    }

    public ApiOperationError? Error
    {
        get { return _error; }
    }

    public void InitializateWithCollection(IList<T> values)
    {
        if(_results is null)
            _results = new List<T>();
        else
            _results.Clear();
        
        _results.AddRange(values);
    }

    public static PagedResult<T> WithError(ApiOperationError error) => new(error);
}
