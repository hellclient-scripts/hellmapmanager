using System;

namespace HellMapManager.States;


public partial class AppState
{
    public event EventHandler? MapFileUpdatedEvent;
    public void RaiseMapFileUpdatedEvent(object? sender)
    {
        MapFileUpdatedEvent?.Invoke(sender, EventArgs.Empty);
    }
    public event EventHandler? SettingsUpdatedEvent;
    public void RaiseSettingsUpdatedEvent(object? sender)
    {
        SettingsUpdatedEvent?.Invoke(sender, EventArgs.Empty);
    }
    public event EventHandler? ExitEvent;
    public void RaiseExitEvent(object? sender)
    {
        ExitEvent?.Invoke(sender, EventArgs.Empty);
    }

}