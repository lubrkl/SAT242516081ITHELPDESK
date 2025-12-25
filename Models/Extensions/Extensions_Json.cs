namespace Extensions;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

public static class Extensions_Json
{
    public static T JsonToItem<T>(this string jsonItem)
    {
        var json = default(T);
        try
        {
            json = JsonSerializer.Deserialize<T>(jsonItem ?? "");
        }
        catch (Exception)
        {
        }

        return json;
    }

    public static string ItemToJson<T>(this T item)
    {
        string str = "";
        var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)};
        try
        {
            str = JsonSerializer.Serialize(item, options);
        }
        catch (Exception)
        {
        }

        return str;
    }

    public static List<T> JsonToList<T>(this string json)
    {
        var jsonList = new List<T>();
        try
        {
            jsonList = JsonSerializer.Deserialize<List<T>>(json ?? "");
        }
        catch (Exception)
        {
        }

        return jsonList;
    }

    public static string ListToJson<T>(this List<T> list)
    {
        string str = "";
        var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
        try
        {
            str = JsonSerializer.Serialize(list, options);
        }
        catch (Exception)
        {
        }

        return str;
    }
}