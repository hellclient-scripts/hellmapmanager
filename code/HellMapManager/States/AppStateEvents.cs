using System;

namespace HellMapManager.States;

public partial class AppState
{

    public event EventHandler? NewFileDialogEvent;
    public void RaiseNewFileDialogEvent(object? sender)
    {
        this.NewFileDialogEvent?.Invoke(sender, EventArgs.Empty);
    }
    public event EventHandler? MapFileUpdatedEvent;
    public void RaiseMapFileUpdatedEvent(object? sender)
    {
        this.MapFileUpdatedEvent?.Invoke(sender, EventArgs.Empty);
    }


}