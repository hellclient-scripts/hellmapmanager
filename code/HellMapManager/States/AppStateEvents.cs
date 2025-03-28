using System;

namespace HellMapManager.States;
public delegate void EventHandler(object? sender);

public partial class AppState
{

    public event EventHandler? NewFileDialogEvent;
    public void RaiseNewFileDialogEvent(object? sender)
    {
        this.NewFileDialogEvent?.Invoke(sender);
    }
}