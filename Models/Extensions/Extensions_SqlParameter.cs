namespace Extensions;

using System.Data;
using Microsoft.Data.SqlClient;

public static class Extensions_SqlParameter
{
    #region ToSqlParameter_Table_Type_Dictionary

    public static SqlParameter ToSqlParameter_Table_Type_Dictionary<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        string parameterName,
        string parameterTypeName = "",
        int length = 0,
        SqlDbType sqlDbType = SqlDbType.Structured,
        ParameterDirection direction = ParameterDirection.Input)
    {
        var keyTypeName = "Int";
        if (typeof(TKey) == typeof(int) || typeof(TValue) == typeof(int?))
            keyTypeName = "Int";
        else if (typeof(TKey) == typeof(string))
            keyTypeName = "String";
        else if (typeof(TKey) == typeof(object))
            keyTypeName = "Object";

        var valueTypeName = "Int";
        if (typeof(TValue) == typeof(int) || typeof(TValue) == typeof(int?))
            valueTypeName = "Int";
        else if (typeof(TValue) == typeof(string))
            valueTypeName = "String";
        else if (typeof(TValue) == typeof(object))
            valueTypeName = "Object";

        //Type_Table_String_String
        parameterTypeName = $"Type_Dictionary_{keyTypeName}_{valueTypeName}";

        var dt = new DataTable();
        dt.Columns.Add("Key", typeof(TKey));
        dt.Columns.Add("Value", typeof(TValue));
        foreach (var item in dictionary)
        {
            var row = dt.NewRow();
            row[0] = item.Key;
            row[1] = item.Value;
            dt.Rows.Add(row);
        }

        return new SqlParameter()
        {
            SqlDbType = sqlDbType,
            Direction = direction,
            ParameterName = parameterName,
            TypeName = parameterTypeName,
            Value = dt
        };
    }

    #endregion

    #region ToSqlParameter_Data_Type

    public static SqlParameter ToSqlParameter_Data_Type<T>(
        this T value,
        string parameterName,
        ParameterDirection direction = ParameterDirection.Input,
        SqlDbType sqlDbType = SqlDbType.NVarChar)
    {
        if (typeof(T) == typeof(int) || typeof(T) == typeof(int?))
            sqlDbType = SqlDbType.Int;
        else if (typeof(T) == typeof(float) || typeof(T) == typeof(float?))
            sqlDbType = SqlDbType.Float;
        else if (typeof(T) == typeof(double) || typeof(T) == typeof(double?))
            sqlDbType = SqlDbType.Float;
        else if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
            sqlDbType = SqlDbType.DateTime2;
        else if (typeof(T) == typeof(char))
            sqlDbType = SqlDbType.NVarChar;
        else if (typeof(T) == typeof(bool) || typeof(T) == typeof(bool?))
            sqlDbType = SqlDbType.Bit;

        var parameter = new SqlParameter
        {
            SqlDbType = sqlDbType,
            Direction = direction,
            ParameterName = parameterName,
            Value = value == null ? DBNull.Value : value
        };

        return parameter;
    }

    #endregion
}