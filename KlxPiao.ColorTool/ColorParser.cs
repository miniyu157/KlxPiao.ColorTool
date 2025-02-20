namespace KlxPiao.ColorTool;

internal class ColorParser
{
    internal static float ParsePercentage(string input, float value = 1) => input.EndsWith('%')
            ? float.Parse(input.TrimEnd('%')) / 100 * value
            : float.Parse(input);


    internal static (float v1, float v2, float v3, byte v4) ParseString(string str)
    {
        string s = str.Trim();
        string[] parts = s.Split(',');

        if (parts.Length != 3 && parts.Length != 4)
        {
            throw new FormatException("HSV / HSL must have exactly three or four components");
        }

        (float v1, float v2, float v3, byte v4) value = (0, 0, 0, 255);

        string p1 = parts[0].Trim();
        string p2 = parts[1].Trim();
        string p3 = parts[2].Trim();

        value.v1 = ParsePercentage(p1, 360);
        value.v2 = ParsePercentage(p2);
        value.v3 = ParsePercentage(p3);

        if (parts.Length == 4)
        {
            string p4 = parts[3].Trim();
            value.v4 = (byte)ParsePercentage(p4, 255);
        }

        return ClampValue(value);
    }

    internal static (float v1, float v2, float v3, byte v4) ClampValue((float v1, float v2, float v3, byte v4) value) =>
        ((value.v1 % 360 + 360) % 360,
        Math.Clamp(value.v2, 0, 1),
        Math.Clamp(value.v3, 0, 1),
        value.v4);
}
