using System;

namespace HellMapManager.States;

public partial class AppState
{

    public event EventHandler? NewFileDialogEvent;
    public void RaiseNewFileDialogEvent(object? sender)
    {
        this.NewFileDialogEvent?.Invoke(sender, EventArgs.Empty);
    }
    public event EventHandler? MudFileUpdatedEvent;
    public void RaiseMudFileUpdatedEvent(object? sender)
    {
        this.MudFileUpdatedEvent?.Invoke(sender, EventArgs.Empty);
    }


}