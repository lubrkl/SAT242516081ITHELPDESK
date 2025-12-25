using Extensions;

namespace MyDbModels;

public static class MyDbModel_Extension
{
    public static IDictionary<object, object> GetOrderByItems<E>(this MyDbModel<E> myDbModel) where E : class, new()
    {
        var sortByItems = new Dictionary<object, object>();

        return sortByItems;
    }

}
