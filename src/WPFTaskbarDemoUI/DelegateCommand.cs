using System;

namespace WPFTaskbarUI;

public class DelegateCommand : DelegateCommandBase
{
    private readonly Action _executeMethod;
    private readonly Func<bool> _canExecuteMethod;

    public DelegateCommand(Action executeMethod) : this(executeMethod, () => true) {}
    public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
    {
        _executeMethod = executeMethod;
        _canExecuteMethod = canExecuteMethod;
    }

    public void Execute() => _executeMethod();
    public bool CanExecute() => _canExecuteMethod();

    protected override void Execute(object? parameter) => Execute();
    protected override bool CanExecute(object? parameter) => CanExecute();
}