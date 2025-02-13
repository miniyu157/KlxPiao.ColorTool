using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ColorTest;

/// <summary>
/// HsvPicker 控件：横向为色相（0～360），纵向为饱和度（0～1），支持拖动选择，并触发 ValueChanged 事件
/// </summary>
public partial class HsvPicker : UserControl
{
    // 标记当前是否处于拖动中
    private bool isDragging = false;

    /// <summary>
    /// 当 Hue 或 Saturation 发生改变时触发
    /// </summary>
    public event EventHandler? ValueChanged;

    public HsvPicker()
    {
        InitializeComponent();

        MouseLeftButtonDown += HsvPicker_MouseLeftButtonDown;
        MouseLeftButtonUp += HsvPicker_MouseLeftButtonUp;
        MouseMove += HsvPicker_MouseMove;
        SizeChanged += HsvPicker_SizeChanged;
    }

    private void HsvPicker_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateSelectorPosition();
    }

    #region Dependency Properties

    /// <summary>
    /// Hue 属性：范围 0～360（float 类型）
    /// </summary>
    public float Hue
    {
        get { return (float)GetValue(HueProperty); }
        set { SetValue(HueProperty, value); }
    }

    public static readonly DependencyProperty HueProperty =
        DependencyProperty.Register("Hue", typeof(float), typeof(HsvPicker),
            new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHueChanged));

    private static void OnHueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var picker = d as HsvPicker;
        picker?.UpdateSelectorPosition();
        picker?.OnValueChanged();
    }

    /// <summary>
    /// Saturation 属性：范围 0～1（float 类型）
    /// </summary>
    public float Saturation
    {
        get { return (float)GetValue(SaturationProperty); }
        set { SetValue(SaturationProperty, value); }
    }

    public static readonly DependencyProperty SaturationProperty =
        DependencyProperty.Register("Saturation", typeof(float), typeof(HsvPicker),
            new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSaturationChanged));

    private static void OnSaturationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var picker = d as HsvPicker;
        picker?.UpdateSelectorPosition();
        picker?.OnValueChanged();
    }

    #endregion

    /// <summary>
    /// 当值改变时调用，触发 ValueChanged 事件
    /// </summary>
    protected virtual void OnValueChanged()
    {
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }

    #region 鼠标事件处理

    private void HsvPicker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        isDragging = true;
        CaptureMouse();
        UpdateColorFromPoint(e.GetPosition(this));
    }

    private void HsvPicker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (isDragging)
        {
            isDragging = false;
            ReleaseMouseCapture();
        }
    }

    private void HsvPicker_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDragging)
        {
            UpdateColorFromPoint(e.GetPosition(this));
        }
    }

    #endregion

    /// <summary>
    /// 根据鼠标在控件中的位置更新 Hue 和 Saturation
    /// </summary>
    /// <param name="point">鼠标位置（相对于控件）</param>
    private void UpdateColorFromPoint(Point point)
    {
        double width = ActualWidth;
        double height = ActualHeight;
        if (width <= 0 || height <= 0)
            return;

        // 横向计算：点的 X 坐标映射到 0～360 的色相值
        float newHue = (float)(point.X / width * 360.0);
        newHue = Math.Max(0, Math.Min(360, newHue));

        // 纵向计算：点的 Y 坐标映射到 0～1 的饱和度（上方为 0，下方为 1）
        float newSaturation = (float)(1 - point.Y / height);
        newSaturation = Math.Max(0, Math.Min(1, newSaturation));

        Hue = newHue;
        Saturation = newSaturation;
    }

    /// <summary>
    /// 根据当前的 Hue 和 Saturation 值更新选择器指示器的位置
    /// </summary>
    private void UpdateSelectorPosition()
    {
        double width = ActualWidth;
        double height = ActualHeight;
        if (width <= 0 || height <= 0)
            return;

        // 根据 Hue 计算 X 坐标（0～360 映射到 0～width）
        double x = (Hue / 360.0) * width;
        // 根据 Saturation 计算 Y 坐标（0～1 映射到 0～height）
        double y = (1 - Saturation) * height;

        // 将圆点置于 (x,y) 处居中显示
        double selectorWidth = Selector.ActualWidth;
        double selectorHeight = Selector.ActualHeight;
        if (double.IsNaN(selectorWidth) || selectorWidth == 0)
            selectorWidth = Selector.Width;
        if (double.IsNaN(selectorHeight) || selectorHeight == 0)
            selectorHeight = Selector.Height;

        Canvas.SetLeft(Selector, x - selectorWidth / 2);
        Canvas.SetTop(Selector, y - selectorHeight / 2);
    }
}
