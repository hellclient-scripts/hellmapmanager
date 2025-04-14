using System;
using HellMapManager.Services;

namespace HellMapManager.States;

public delegate void ShowRelationMapEventHandler(object? sender, RelationMapItem rm);

public partial class AppState
{
    public event EventHandler? MapFileUpdatedEvent;
    public void RaiseMapFileUpdatedEvent(object? sender)
    {
        MapFileUpdatedEvent?.Invoke(sender, EventArgs.Empty);
    }
    public event ShowRelationMapEventHandler? ShowRelationMapEvent;
    public void RaiseShowRelationMapEvent(object? sender, RelationMapItem rm)
    {
        ShowRelationMapEvent?.Invoke(sender, rm);
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