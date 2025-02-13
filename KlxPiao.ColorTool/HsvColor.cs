using System.Windows.Media;

namespace KlxPiao.ColorTool;

/// <summary>
/// Provides extension methods for converting <see cref="Color"/> to HSV format.
/// </summary>
public static class HsvColorExtension
{
    /// <summary>
    /// Converts a <see cref="Color"/> to an HSV formatted string.
    /// </summary>
    /// <param name="color">The RGB color to convert.</param>
    /// <returns>A string in the format "H, S%, V%".</returns>
    public static string ToHsvString(this Color color)
    {
        var hsv = color.ToHsv();
        return $"{hsv.Hue}, {hsv.Saturation * 100}%, {hsv.Value * 100}%";
    }

    /// <summary>
    /// Converts a <see cref="Color"/> to an <see cref="HsvColor"/> structure.
    /// </summary>
    /// <param name="color">The RGB color to convert.</param>
    /// <returns>An HSV color representation.</returns>
    public static HsvColor ToHsv(this Color color)
    {
        float r0 = (float)color.R / 255;
        float g0 = (float)color.G / 255;
        float b0 = (float)color.B / 255;

        float cMax = Math.Max(r0, Math.Max(g0, b0));
        float cMin = Math.Min(r0, Math.Min(g0, b0));
        float Δ = cMax - cMin;

        float h = Δ switch
        {
            0 => 0,
            _ when cMax == r0 => 60 * ((g0 - b0) / Δ % 6),
            _ when cMax == g0 => 60 * ((b0 - r0) / Δ + 2),
            _ when cMax == b0 => 60 * ((r0 - g0) / Δ + 4),
            _ => 0
        };

        float s = cMax switch
        {
            0 => 0,
            _ => Δ / cMax
        };

        float v = cMax;

        if (h < 0) h += 360;

        return new HsvColor(h, s, v);
    }
}

/// <summary>
/// Represents a color in HSV (Hue, Saturation, Value) format.
/// </summary>
public readonly struct HsvColor
{
    public readonly float Hue;
    public readonly float Saturation;
    public readonly float Value;

    /// <summary>
    /// Converts the HSV color to an RGB <see cref="Color"/>.
    /// </summary>
    /// <returns>The equivalent RGB color.</returns>
    public readonly Color ToColor()
    {
        float c = Value * Saturation;
        float x = c * (1 - Math.Abs(Hue / 60 % 2 - 1));
        float m = Value - c;

        (float r0, float g0, float b0) = Hue switch
        {
            >= 0 and < 60 => (c, x, 0f),
            >= 60 and < 120 => (x, c, 0f),
            >= 120 and < 180 => (0f, c, x),
            >= 180 and < 240 => (0f, x, c),
            >= 240 and < 300 => (x, 0f, c),
            >= 300 and < 360 => (c, 0f, x),
            _ => (0, 0, 0)
        };

        byte r = (byte)((r0 + m) * 255);
        byte g = (byte)((g0 + m) * 255);
        byte b = (byte)((b0 + m) * 255);

        return Color.FromRgb(r, g, b);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HsvColor"/> structure.
    /// </summary>
    /// <param name="h">Hue value (0-360 degrees).</param>
    /// <param name="s">Saturation (0-1).</param>
    /// <param name="v">Value/Brightness (0-1).</param>
    public HsvColor(float h, float s, float v)
    {
        (Hue, Saturation, Value) = ColorParser.ClampValue((h, s, v));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HsvColor"/> structure from a string.
    /// </summary>
    /// <param name="str">Input string in "H, S%, V%" format.</param>
    /// <exception cref="ArgumentException">Thrown for invalid input format.</exception>
    public HsvColor(string str)
    {
        (Hue, Saturation, Value) = ColorParser.ParseString(str);
    }

    /// <summary>
    /// Creates an HSV color from specified components.
    /// </summary>
    /// <param name="h">Hue component.</param>
    /// <param name="s">Saturation component.</param>
    /// <param name="v">Value component.</param>
    public static HsvColor FromHsv(float h, float s, float v)
    {
        return new HsvColor(h, s, v);
    }

    /// <summary>
    /// Creates an HSV color from a formatted string.
    /// </summary>
    /// <param name="str">Input string in "H, S%, V%" format.</param>
    public static HsvColor FromString(string str)
    {
        return new HsvColor(str);
    }

    /// <summary>
    /// Returns a string representation in "H, S%, V%" format.
    /// </summary>
    public override readonly string ToString()
    {
        return $"{Hue}, {Saturation * 100}%, {Value * 100}%";
    }

    /// <summary>
    /// Converts HSV color to RGB color implicitly.
    /// </summary>
    public static implicit operator Color(HsvColor color)
    {
        return color.ToColor();
    }

    /// <summary>
    /// Converts RGB color to HSV color implicitly.
    /// </summary>
    public static implicit operator HsvColor(Color color)
    {
        return color.ToHsv();
    }

}
