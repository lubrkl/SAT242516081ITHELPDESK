namespace MyDbModels;

public interface IMyDbModel_Parameter
{
    string OrderBy { get; set; }
    int PageNumber { get; set; }
    int PageSize { get; set; }
    int TotalPageCount { get; }
    int TotalRecordCount { get; set; }
    IDictionary<string, object> Params { get; set; }
    IDictionary<string, string> Where { get; set; }
}

internal sealed class MyDbModel_Parameter : IMyDbModel_Parameter
{
    public static MyDbModel_Parameter Create(int pageNumber, int pageSize, string orderBy) => new(pageNumber, pageSize, orderBy);
    private MyDbModel_Parameter(int pageNumber, int pageSize, string orderBy)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        OrderBy = orderBy;

        if (Params == null) Params = new Dictionary<string, object>();
        if (Where == null) Where = new Dictionary<string, string>();
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecordCount { get; set; }
    public int TotalPageCount => (int)Math.Ceiling(TotalRecordCount / (double)(PageSize <= 0 ? 1 : PageSize));
    public string OrderBy { get; set; }
    public IDictionary<string, object> Params { get; set; }
    public IDictionary<string, string> Where { get; set; }
}