using System.IO;
using System.Reflection;
using System.Windows;

namespace WPFTaskbarUI;

public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var jumpListService = new JumpListService();
        
        jumpListService.AddActionItem("Quit", "close", ActionIcon.Close);

        jumpListService.AddRecentItem("Presentation", "start=presentation");
        jumpListService.AddRecentItem("Sales", "start=sales");
        jumpListService.AddRecentItem("Forecast", "start=forecast");
        
        jumpListService.Apply();

        MainWindow = new MainWindow(jumpListService);
        MainWindow.Show();
    }

}
