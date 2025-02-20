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
        HsvColor hsv = Colors.Red;

        Hue = hsv.Hue;
        Saturation = hsv.Saturation;
        Value = hsv.Value;
        Alpha = hsv.Alpha;
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

    private static void SetClipBoard(string s)
    {
        try
        {

            Clipboard.SetText(s);
        }
        catch { }
    }
}
