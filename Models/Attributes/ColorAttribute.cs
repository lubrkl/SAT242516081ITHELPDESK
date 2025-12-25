namespace Attributes;

public class ColorAttribute(string color) : Attribute
{
    public string Color { get; set; } = color;
}
