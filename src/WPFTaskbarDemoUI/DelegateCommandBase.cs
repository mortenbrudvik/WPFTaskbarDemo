using System;
using System.Windows.Input;

namespace WPFTaskbarUI;

public abstract class DelegateCommandBase : ICommand
{
    public virtual event EventHandler? CanExecuteChanged;

    void ICommand.Execute(object? parameter) => Execute(parameter);
    bool ICommand.CanExecute(object? parameter) => CanExecute(parameter);

    public void RaiseCanExecuteChanged() =>
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    protected abstract void Execute(object? parameter);

    protected abstract bool CanExecute(object? parameter);
}