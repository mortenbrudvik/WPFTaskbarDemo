using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace WPFTaskbarUI;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly Action<string> _updateCount;
    private bool _isRunning;
    private readonly ImageFactory _imageFactory;
    private string _consoleText;
    private bool _progressIndicatorEnabled;
    private string _progressStatus = "None";
    private double _progressValue;
    private int _indicatorCount = 1;

    public MainWindowViewModel(Action<string> updateCount)
    {
        _updateCount = updateCount;
        _imageFactory = new ImageFactory();
        ProgressStatusStates = new List<string> {"None", "Indeterminate", "Normal", "Error", "Paused"};
        ProgressStatusSelected = "None";
    }

    public ImageSource PlayerIcon => IsRunning
        ? _imageFactory.Create(PackIconMaterialKind.Stop)
        : _imageFactory.Create(PackIconMaterialKind.Play);

    public ImageSource StopIcon => _imageFactory.Create(PackIconMaterialKind.Stop, Brushes.Red);
    
    
    public string ConsoleText
    {
        get => _consoleText;
        set => SetField(ref _consoleText, value);
    }

    public void Log(string text) => ConsoleText += text + Environment.NewLine;

    public DelegateCommand PlayStopCommand => new(() =>
    {
        if (IsRunning)
        {
            IsRunning = false;  
            Log("Button: Stop");
        }
        else
        {
            IsRunning = true;
            Log("Button: Start");
        }
        OnPropertyChanged(nameof(PlayerIcon));
    }, () => true);

    public bool IsRunning
    {
        get => _isRunning;
        private set => SetField(ref _isRunning, value);
    }

    public DelegateCommand RefreshCommand => new(() =>
    {
        Log("Button: Refresh");
    });

    public DelegateCommand IncreaseCommand => new(() =>
    {
        _indicatorCount = (_indicatorCount +1) % 9;
        Log("Increase count: " + _indicatorCount);

        _updateCount(_indicatorCount + "");
    });
    
    
    // Progress Indicator
    public List<string> ProgressStatusStates { get; init; }

    public double ProgressValue
    {
        get => _progressValue;
        set => SetField(ref _progressValue, value);
    }

    public string ProgressStatus
    {
        get => _progressStatus;
        set => SetField(ref _progressStatus, value);
    }

    public string ProgressStatusSelected
    {
        get => ProgressStatus;
        set
        {
            Log("Progress Status: " + value);
            ProgressStatus = value;

            if( value != "Normal")
                ProgressValue = 1;
        }
    }

    
    // Status Indicator
    public string IndicatorCount => _indicatorCount + "";

    public bool ProgressIndicatorEnabled
    {
        get => _progressIndicatorEnabled;
        set
        {
            Log("Status indicator enabled: " + value);
            SetField(ref _progressIndicatorEnabled, value);
        }
    }

    public bool ShowIndicator { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    
    
    //  MVVM Helper methods
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}