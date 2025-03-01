﻿using System.Windows.Media;

namespace KlxPiao.ColorTool;

/// <summary>
/// Provides extension methods for converting <see cref="Color"/> to HSL format.
/// </summary>
public static class HslColorExtension
{
    /// <summary>
    /// Converts a <see cref="Color"/> to an HSL formatted string.
    /// </summary>
    /// <param name="color">The RGB color to convert.</param>
    /// <returns>"H, S%, L%, A" where A is the alpha value (0-255).</returns>
    public static string ToHslString(this Color color)
    {
        var hsl = color.ToHsl();
        return hsl.ToString();
    }

    /// <summary>
    /// Converts a <see cref="Color"/> to an <see cref="HslColor"/> structure.
    /// </summary>
    /// <param name="color">The RGB color to convert.</param>
    /// <returns>An HSL color representation.</returns>
    public static HslColor ToHsl(this Color color)
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

        if (h < 0) h += 360;

        float l = (cMax + cMin) / 2;

        float s = Δ switch
        {
            0 => 0,
            _ when l <= 0.5f => Δ / (cMax + cMin),
            _ => Δ / (2 - cMax - cMin)
        };

        return new HslColor(h, s, l, color.A);
    }
}

/// <summary>
/// Represents a color in HSL (Hue, Saturation, Lightness) format.
/// </summary>
public readonly struct HslColor
{
    public readonly float Hue;
    public readonly float Saturation;
    public readonly float Lightness;
    public readonly byte Alpha;

    /// <summary>
    /// Converts the HSL color to an RGB <see cref="Color"/>.
    /// </summary>
    /// <returns>The equivalent RGB color.</returns>
    public readonly Color ToColor()
    {
        float c = (1 - Math.Abs(2 * Lightness - 1)) * Saturation;
        float x = c * (1 - Math.Abs(Hue / 60 % 2 - 1));
        float m = Lightness - c / 2;

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

        return Color.FromArgb(Alpha, r, g, b);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HslColor"/> structure.
    /// </summary>
    /// <param name="h">Hue value (0-360 degrees).</param>
    /// <param name="s">Saturation (0-1).</param>
    /// <param name="l">Lightness (0-1).</param>
    /// <param name="a">Alpha (0-255).</param>
    public HslColor(float h, float s, float l, byte a = 255)
    {
        (Hue, Saturation, Lightness, Alpha) = ColorParser.ClampValue((h, s, l, a));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HslColor"/> structure.
    /// </summary>
    /// <param name="h">Hue value (0-360 degrees).</param>
    /// <param name="s">Saturation (0-1).</param>
    /// <param name="l">Lightness (0-1).</param>
    /// <param name="a">Alpha value (0-1).</param>
    public HslColor(float h, float s, float l, float a)
    {
        (Hue, Saturation, Lightness, Alpha) = ColorParser.ClampValue((h, s, l, (byte)(a * 255)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HslColor"/> structure from a string.
    /// </summary>
    /// <param name="str">Input string in "H, S%, L%" or "H, S%, L%, A" format, where A is the alpha value (0-255).</param>
    /// <exception cref="FormatException">Thrown when the input string does not have exactly three or four components.</exception>
    public HslColor(string str)
    {
        (Hue, Saturation, Lightness, Alpha) = ColorParser.ParseString(str);
    }

    /// <summary>
    /// Creates an HSL color from specified components.
    /// </summary>
    /// <param name="h">Hue component (0-360).</param>
    /// <param name="s">Saturation component (0-1).</param>
    /// <param name="l">Lightness component (0-1).</param>
    /// <param name="a">Alpha component (0-255).</param>
    public static HslColor FromHsl(float h, float s, float l, byte a = 255)
    {
        return new HslColor(h, s, l, a);
    }

    /// <summary>
    /// Creates an HSLA color from specified components.
    /// </summary>
    /// <param name="h">Hue component (0-360).</param>
    /// <param name="s">Saturation component (0-1).</param>
    /// <param name="l">Lightness component (0-1).</param>
    /// <param name="a">Alpha component (0-1).</param>
    public static HslColor FromHsl(float h, float s, float l, float a)
    {
        return new HslColor(h, s, l, a);
    }

    /// <summary>
    /// Creates an HSL color from a formatted string.
    /// </summary>
    /// <param name="str">Input string in "H, S%, L%" or "H, S%, L%, A" format.</param>
    public static HslColor FromString(string str)
    {
        return new HslColor(str);
    }

    /// <summary>
    /// "H, S%, L%, A" where A is the alpha value (0-255).
    /// </summary>
    public override readonly string ToString()
    {
        return $"{Hue}, {Saturation * 100}%, {Lightness * 100}%, {Alpha}";
    }

    /// <summary>
    /// Converts HSL color to RGB color implicitly.
    /// </summary>
    public static implicit operator Color(HslColor color)
    {
        return color.ToColor();
    }

    /// <summary>
    /// Converts RGB color to HSL color implicitly.
    /// </summary>
    public static implicit operator HslColor(Color color)
    {
        return color.ToHsl();
    }

}
