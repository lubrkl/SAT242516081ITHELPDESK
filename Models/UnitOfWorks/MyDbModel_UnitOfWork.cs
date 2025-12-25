using System.Data;
using Extensions;
using Microsoft.EntityFrameworkCore;
using MyDbModels;

namespace UnitOfWorks;

public interface IMyDbModel_UnitOfWork
{
    Task Execute<T>(IMyDbModel<T> myDbModel, string spName = "", bool isPagination = true)
        where T : class, new();
}


public sealed class MyDbModel_UnitOfWork<TDbContext>(TDbContext context) : IMyDbModel_UnitOfWork where TDbContext : DbContext
{
    private readonly DbContext _context = context;

    public async Task Execute<T>(IMyDbModel<T> myDbModel,
        string spName = "",
        bool isPagination = true)
        where T : class, new()
    {
        var con = _context.Database.GetDbConnection();
        var connectionState = con.State;

        try
        {
            if (connectionState != ConnectionState.Open)
                con.Open();

            using (var cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;

                var pageno = myDbModel.Parameters.PageNumber;
                var pagesize = myDbModel.Parameters.PageSize;

                var total_record_count = 0;
                var total_page_count = 0;

                var orderby = myDbModel.Parameters.OrderBy;

                var _params = myDbModel.Parameters.Params;

                var where = myDbModel.Parameters.Where;

                cmd.Parameters.Clear();

                if (isPagination)
                {
                    var pagination = new Dictionary<string, string>
                    {
                        { "PageNumber", pageno.ToString() },
                        { "PageSize", pagesize.ToString() },
                        { "OrderBy", orderby },
                    };
                    cmd.Parameters.Add(pagination.ToSqlParameter_Table_Type_Dictionary("pagination"));
                }

                if (where?.Any() == true)
                    cmd.Parameters.Add(where.ToSqlParameter_Table_Type_Dictionary("where"));

                if (_params?.Any() == true)
                    foreach (var param in _params)
                        cmd.Parameters.Add(param.Value.ToSqlParameter_Data_Type(param.Key));
                
                //get
                var table = new DataTable();
                
                table.Load(await cmd.ExecuteReaderAsync());

                if (isPagination && table.Rows.Count > 0)
                    total_record_count = (int)table.Rows[0]["TotalRecordCount"];

                var items = table.DataTableToList<T>();

                total_page_count = (int)Math.Ceiling((decimal)total_record_count / pagesize);

                if (pageno > total_page_count)
                    pageno = total_page_count;
                else if (pageno <= 0)
                    pageno = 1;

                myDbModel.Parameters.TotalRecordCount = total_record_count;

                myDbModel.Parameters.PageNumber = pageno;

                myDbModel.Items = items;
            }
        }
        catch (Exception ex)
        {
            myDbModel.Message = $"{spName}: {ex.Message}";
        }
        finally
        {
            if (connectionState != ConnectionState.Closed)
                con.Close();
        }
    }
}



