using System;

namespace HellMapManager.States;

public partial class AppState
{
    public event EventHandler? MapFileUpdatedEvent;
    public void RaiseMapFileUpdatedEvent(object? sender)
    {
        this.MapFileUpdatedEvent?.Invoke(sender, EventArgs.Empty);
    }


}