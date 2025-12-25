using Attributes;

namespace Extensions;

public static class Extensions_Enum
{
    public static string Color<T>(this T value)
    {
        var result = value.ToString();

        try
        {
            var fi = value
                .GetType()
                .GetField(value.ToString());
        
            if (fi != null)
            {
                var attributes = (ColorAttribute[])fi.GetCustomAttributes(typeof(ColorAttribute), false);
                result = attributes != null && attributes.Length > 0 
                    ? attributes[0].Color
                    : value.ToString();
            }
        }
        catch (Exception) { }

        return result;
    }
    
    public static string Title<T>(this T value)
    {
        var result = value.ToString();

        try
        {
            var fi = value
                .GetType()
                .GetField(value.ToString());
        
            if (fi != null)
            {
                var attributes = (TitleAttribute[])fi.GetCustomAttributes(typeof(TitleAttribute), false);
                result = attributes != null && attributes.Length > 0 
                    ? attributes[0].Title
                    : value.ToString();
            }
        }
        catch (Exception) { }

        return result;
    }

}