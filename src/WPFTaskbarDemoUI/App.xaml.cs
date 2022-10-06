using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Shell;

namespace WPFTaskbarUI;

public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var exeFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var exePath = Path.Combine(exeFolderPath, "CommandRelay.exe");
            
        var jumpList = new JumpList();
        JumpList.SetJumpList(Current, jumpList);
        
        jumpList.JumpItems.Add(CreateTask("Quit", exePath, "close", @"C:\windows\System32\SHELL32.dll", 131)); // TODO: Use ImageLib to fetch icon
            
        jumpList.JumpItems.Add(CreateRecentItem("Presentation", exePath, "\"start=presentation\""));
        jumpList.JumpItems.Add(CreateRecentItem("Sales", exePath, "\"start=sales\""));
        jumpList.JumpItems.Add(CreateRecentItem("Forecast", exePath, "\"start=forecast\""));
            
        jumpList.Apply();
    }

    private JumpTask CreateTask(string title, string path, string args = "", string resourcePath = "", int resourceIndex = 0)
    {
        return new JumpTask()
        {
            Title = title,
            ApplicationPath = path,
            IconResourcePath = resourcePath,
            IconResourceIndex = resourceIndex,
            Arguments = args
        };
    }

    private JumpTask CreateRecentItem(string title, string path, string args = "") =>
        new()
        {
            CustomCategory = "Recent",
            Title = title,
            ApplicationPath = path,
            IconResourcePath = path,
            Arguments = args
        };
}
