using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFTaskbarUI;

public partial class MainWindow
{
    private readonly JumpListService _jumpListService;
    private readonly MainWindowViewModel _viewModel;

    public MainWindow(JumpListService jumpListService)
    {
        _jumpListService = jumpListService;
        InitializeComponent();

        _viewModel = new MainWindowViewModel(UpdateCount);
        DataContext = _viewModel;
    }

    private void UpdateCount(string count)
    {
        ShowTaskbarIndicator(count);
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
            
        HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
        source.AddHook(WndProc);
    }
    const uint WM_COPYDATA = 0x004A;
    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_COPYDATA)
        {
            COPYDATASTRUCT copyData = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT));
            string message = Marshal.PtrToStringAnsi(copyData.lpData);

            _viewModel.Log("IPC Message: " + message);
        }

        return IntPtr.Zero;
    }

    private void ShowTaskbarIndicator(string number)
    {
        int iconWidth = 20;
        int iconHeight = 20;

        RenderTargetBitmap bmp = new RenderTargetBitmap(iconWidth, iconHeight, 96, 96, PixelFormats.Default);
        ContentControl root = new ContentControl();

        root.ContentTemplate = (DataTemplate) Resources["OverlayIcon"];
        root.Content = number;

        root.Arrange(new Rect(0, 0, iconWidth, iconHeight));

        bmp.Render(root);

        TaskbarItemInfo.Overlay = bmp;
    }

    private void ShowIndicator_OnClick(object sender, RoutedEventArgs e)
    {
        _viewModel.ShowIndicator = !_viewModel.ShowIndicator;

        if (_viewModel.ShowIndicator)
            ShowTaskbarIndicator(_viewModel.IndicatorCount);
        else
            HideTaskbarIndicator();
    }

    private void HideTaskbarIndicator()
    {
        TaskbarItemInfo.Overlay = null;
    }
}


[StructLayout(LayoutKind.Sequential)]
public struct COPYDATASTRUCT
{
    /// <summary>
    /// User defined data to be passed to the receiving application.
    /// </summary>
    public IntPtr dwData;

    /// <summary>
    /// The size, in bytes, of the data pointed to by the lpData member.
    /// </summary>
    public int cbData;

    /// <summary>
    /// The data to be passed to the receiving application. This member can be IntPtr.Zero.
    /// </summary>
    public IntPtr lpData;
}