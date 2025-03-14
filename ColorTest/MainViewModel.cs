using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KlxPiao.ColorTool;
using System.Windows;
using System.Windows.Media;

namespace ColorTest;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PreviewBrush))]
    [NotifyPropertyChangedFor(nameof(Hex))]
    [NotifyPropertyChangedFor(nameof(Hsv))]
    [NotifyPropertyChangedFor(nameof(Rgb))]
    private double _hue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PreviewBrush))]
    [NotifyPropertyChangedFor(nameof(Hex))]
    [NotifyPropertyChangedFor(nameof(Hsv))]
    [NotifyPropertyChangedFor(nameof(Rgb))]
    private double _saturation;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PreviewBrush))]
    [NotifyPropertyChangedFor(nameof(Hex))]
    [NotifyPropertyChangedFor(nameof(Hsv))]
    [NotifyPropertyChangedFor(nameof(Rgb))]
    private double _value;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PreviewBrush))]
    [NotifyPropertyChangedFor(nameof(Hex))]
    [NotifyPropertyChangedFor(nameof(Hsv))]
    [NotifyPropertyChangedFor(nameof(Rgb))]
    private byte _alpha;

    public MainViewModel()
    {
        SetColor(Colors.Red);
    }

    private HsvColor HsvColor => new((float)Hue, (float)Saturation, (float)Value, (byte)Alpha);
    private Color Color => HsvColor;
    public SolidColorBrush PreviewBrush => new(HsvColor);

    public string Hex => $"{Color}";
    public string Hsv => $"{HsvColor}";
    public string Rgb => $"{Color.R}, {Color.G}, {Color.B}, {Color.A}";

    [RelayCommand]
    public void CopyHex()
    {
        SetClipBoard(Hex);
    }

    [RelayCommand]
    public void CopyHsv()
    {
        SetClipBoard(Hsv);
    }

    [RelayCommand]
    public void CopyRgb()
    {
        SetClipBoard(Rgb);
    }

    private void SetColor(Color color)
    {
        HsvColor hsv = color;

        Hue = hsv.Hue;
        Saturation = hsv.Saturation;
        Value = hsv.Value;
        Alpha = hsv.Alpha;
    }

    [RelayCommand]
    private void EditHex()
    {
        string str = Microsoft.VisualBasic.Interaction.InputBox("", "", Hex);
        try
        {
            Color color = (Color)ColorConverter.ConvertFromString(str);
            SetColor(color);
        }
        catch { }
    }

    [RelayCommand]
    private void EditHsv()
    {
        string str = Microsoft.VisualBasic.Interaction.InputBox("", "", Hsv);
        try
        {
            static float ParsePercentage(string input, float value = 1) => input.EndsWith('%')
                    ? float.Parse(input.TrimEnd('%')) / 100 * value
                    : float.Parse(input);


            static (float v1, float v2, float v3, byte v4) ParseString(string str)
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

            static (float v1, float v2, float v3, byte v4) ClampValue((float v1, float v2, float v3, byte v4) value) =>
                ((value.v1 % 360 + 360) % 360,
                Math.Clamp(value.v2, 0, 1),
                Math.Clamp(value.v3, 0, 1),
                value.v4);

            var (h, s, v, a) = ParseString(str);

            Hue = h;
            Saturation = s;
            Value = v;
            Alpha = a;
        }
        catch { }
    }

    [RelayCommand]
    private void EditRgb()
    {
        string str = Microsoft.VisualBasic.Interaction.InputBox("", "", Rgb);
        try
        {
            string s = str.Trim();
            string[] parts = s.Split(',');

            if (parts.Length != 3 && parts.Length != 4)
            {
                return;
            }

            byte r = byte.Parse(parts[0].Trim());
            byte g = byte.Parse(parts[1].Trim());
            byte b = byte.Parse(parts[2].Trim());
            byte a = 255;

            if (parts.Length == 4)
            {
                a = byte.Parse(parts[3].Trim());
            }

            Color color = Color.FromArgb(a, r, g, b);
            SetColor(color);
        }
        catch { }
    }

    private static void SetClipBoard(string s)
    {
        try
        {
            Clipboard.SetText(s);
        }
        catch { }
    }
}
