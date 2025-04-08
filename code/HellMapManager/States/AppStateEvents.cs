using System;
using System.Threading.Tasks;
using HellMapManager.Services;

namespace HellMapManager.States;

public delegate void ShowRelationMapEventHandler(object? sender, RelationMapItem rm);

public partial class AppState
{
    public event EventHandler? MapFileUpdatedEvent;
    public void RaiseMapFileUpdatedEvent(object? sender)
    {
        this.MapFileUpdatedEvent?.Invoke(sender, EventArgs.Empty);
    }
    public event ShowRelationMapEventHandler? ShowRelationMapEvent;
    public void RaiseShowRelationMapEvent(object? sender, RelationMapItem rm)
    {
        this.ShowRelationMapEvent?.Invoke(sender, rm);
    }
}