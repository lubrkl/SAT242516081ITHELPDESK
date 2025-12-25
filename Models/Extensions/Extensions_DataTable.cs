namespace Extensions;

using System.Data;
using System.Reflection;

public static class Extensions_DataTable
{
    public static IEnumerable<T> DataTableToList<T>(this DataTable table) where T : class
    {
        var list = new List<T>();
        try
        {
            var columnsNames = new List<string>();
            foreach (DataColumn DataColumn in table.Columns)
                columnsNames.Add(DataColumn.ColumnName);

            list = table.AsEnumerable().ToList()
                .ConvertAll(row => GetObject<T>(row, columnsNames));
        }
        catch (Exception)
        {
            // ...
        }

        return list;
    }

    public static T GetObject<T>(this DataRow row, List<string> columnsName) where T : class
    {
        T obj = (T)Activator.CreateInstance(typeof(T));
        try
        {
            PropertyInfo[] Properties = typeof(T).GetProperties();
            foreach (PropertyInfo objProperty in Properties)
            {
                string columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                if (!string.IsNullOrEmpty(columnname))
                {
                    object dbValue = row[columnname];
                    if (dbValue != DBNull.Value)
                    {
                        if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            objProperty.SetValue(obj,
                                Convert.ChangeType(dbValue,
                                    Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())),
                                null);
                        else
                            objProperty.SetValue(obj,
                                Convert.ChangeType(dbValue, Type.GetType(objProperty.PropertyType.ToString())), null);
                    }
                }
            }
        }
        catch (Exception)
        {
            // ...
        }

        return obj;
    }
}