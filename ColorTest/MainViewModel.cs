using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KlxPiao.ColorTool;
using System.Windows.Media;

namespace ColorTest;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PreviewColor))]
    [NotifyPropertyChangedFor(nameof(ColorText))]
    private double _hue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PreviewColor))]
    [NotifyPropertyChangedFor(nameof(ColorText))]
    private double _saturation;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PreviewColor))]
    [NotifyPropertyChangedFor(nameof(ColorText))]
    private double _value;

    public MainViewModel()
    {
        ChangeColor(Colors.Red);
    }

    public SolidColorBrush PreviewColor => new(HsvColor.FromHsv((float)Hue, (float)Saturation, (float)Value));

    public string ColorText => $"{PreviewColor.Color}\r\n{Hue:F2}, {Saturation * 100:F2}%, {Value * 100:F2}%";

    [RelayCommand]
    private void ChangeColor(Color color)
    {
        HsvColor hsv = color;

        Hue = hsv.Hue;
        Saturation = hsv.Saturation;
        Value = hsv.Value;
    }
}
