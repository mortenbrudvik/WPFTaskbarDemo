using System.Collections.Generic;
using System.Windows;
using System.Windows.Shell;

namespace WPFTaskbarUI;

public class TaskbarJumpListService
{
    private readonly string _commandRelayPath;
    private readonly List<JumpTask> _recentList;
    private readonly List<JumpTask> _actionList;

    public TaskbarJumpListService(string commandRelayPath)
    {
        _commandRelayPath = commandRelayPath;
        _recentList = new List<JumpTask>();
        _actionList = new List<JumpTask>();
    }
    
    public void AddRecentItem(string title, string command) =>
        _recentList.Add(CreateRecentItem(title, _commandRelayPath, command));

    public void AddActionItem(string title, string command, ActionIcon icon)
    {
        var iconResourcePath = "";
        var iconResourceIndex = 0;
        if (icon == ActionIcon.Close)
        {
            iconResourcePath = "";
            iconResourceIndex = 0;
        }
        _actionList.Add(CreateTask(title, _commandRelayPath, command, iconResourcePath, iconResourceIndex));
    }

    public void Apply()
    {
        var jumpList = new JumpList();
        JumpList.SetJumpList(Application.Current, jumpList);

        _actionList.ForEach(item => jumpList.JumpItems.Add(item));
        _recentList.ForEach(item => jumpList.JumpItems.Add(item));
        
        jumpList.Apply();
    }
    
    private JumpTask CreateTask(string title, string path, string args = "", string resourcePath = "", int resourceIndex = 0) =>
        new()
        {
            Title = title,
            ApplicationPath = path,
            IconResourcePath = resourcePath,
            IconResourceIndex = resourceIndex,
            Arguments = args
        };

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
public enum ActionIcon {Close}