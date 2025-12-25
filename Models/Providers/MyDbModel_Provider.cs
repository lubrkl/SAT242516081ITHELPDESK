using MyDbModels;
using UnitOfWorks;

namespace Providers;

public interface IMyDbModel_Provider
{
    ValueTask<IMyDbModel<TResult>> Execute<TResult>(IMyDbModel<TResult> myResultModel,
        string spName = "",
        bool isPagination = true) where TResult : class, new();

    ValueTask<IMyDbModel<TResult>> Execute<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new();

    ValueTask<IEnumerable<TResult>> GetItems<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new();

    ValueTask<IEnumerable<TResult>> SetItems<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new();
}

public class MyDbModel_Provider(IMyDbModel_UnitOfWork myDbModel_UnitOfWork) : IMyDbModel_Provider
{
    #region Execute : Pagination=true

    public async ValueTask<IMyDbModel<TResult>> Execute<TResult>(IMyDbModel<TResult> myResultModel,
        string spName = "",
        bool isPagination = true) where TResult : class, new()
    {
        if (myResultModel == null)
            myResultModel = new MyDbModel<TResult>();

        await myDbModel_UnitOfWork.Execute(myResultModel, spName, isPagination);

        return myResultModel;
    }

    #endregion

    #region Execute : Pagination=false

    public async ValueTask<IMyDbModel<TResult>> Execute<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new()
    {
        var myResultModel = new MyDbModel<TResult>();
        if (parameters != null)
            foreach (var item in parameters)
                myResultModel.Parameters.Params.Add(item.Key, item.Value);

        await myDbModel_UnitOfWork.Execute(myResultModel, spName, false);
        return myResultModel;
    }

    #endregion

    #region GetItems : Pagination=false

    public async ValueTask<IEnumerable<TResult>> GetItems<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new() =>
        (await this.Execute<TResult>(spName, parameters)).Items;

    #endregion

    #region SetItems : Pagination=false

    public async ValueTask<IEnumerable<TResult>> SetItems<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new() =>
        (await this.Execute<TResult>(spName, parameters)).Items;

    #endregion
}