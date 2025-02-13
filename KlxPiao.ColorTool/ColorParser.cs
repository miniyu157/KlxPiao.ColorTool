namespace KlxPiao.ColorTool;

internal class ColorParser
{
    internal static float ParsePercentage(string input)
    {
        float value = input.EndsWith('%')
            ? float.Parse(input.TrimEnd('%')) / 100
            : float.Parse(input);

        return value;
    }

    internal static (float v1, float v2, float v3) ParseString(string str)
    {
        string s = str.Trim();
        string[] parts = s.Split(',');

        if (parts.Length != 3)
            throw new FormatException("HSV / HSL must have exactly three components.");

        string p1 = parts[0].Trim();
        string p2 = parts[1].Trim();
        string p3 = parts[2].Trim();

        float v1 = float.Parse(p1);
        float v2 = ParsePercentage(p2);
        float v3 = ParsePercentage(p3);

        return ClampValue((v1, v2, v3));
    }

    internal static (float v1, float v2, float v3) ClampValue((float v1, float v2, float v3) value) =>
        ((value.v1 % 360 + 360) % 360,
        Math.Clamp(value.v2, 0, 1),
        Math.Clamp(value.v3, 0, 1));
}
