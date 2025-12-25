namespace MyDbModels;

public interface IMyDbModel
{
    string Message { get; set; }
    IMyDbModel_Parameter Parameters { get; set; }
    IDictionary<object, object> OrderByItems { get; set; }
}

public interface IMyDbModel<T> : IMyDbModel where T : class, new()
{
    IEnumerable<T> Items { get; set; }
}

public sealed class MyDbModel<T> : IMyDbModel<T> where T : class, new()
{
    public MyDbModel() : this(1, 10, "")
    {
    }

    public MyDbModel(int pageNumber, int pageSize, string orderBy)
    {
        Parameters = MyDbModel_Parameter.Create(pageNumber, pageSize, orderBy);
        OrderByItems = this.GetOrderByItems();
        Items = new List<T>();
    }

    public IMyDbModel_Parameter Parameters { get; set; }
    public IDictionary<object, object> OrderByItems { get; set; }
    public IEnumerable<T> Items { get; set; }
    public string Message { get; set; }
}