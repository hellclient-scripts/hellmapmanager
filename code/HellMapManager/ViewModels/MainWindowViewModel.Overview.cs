using System;
using HellMapManager.States;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitOverview()
    {
        AppState.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(GetMapRoomsCount));
            OnPropertyChanged(nameof(GetMapAliasesCount));
            OnPropertyChanged(nameof(GetMapRoutesCount));
            OnPropertyChanged(nameof(GetMapTracesCount));
            OnPropertyChanged(nameof(GetMapRegionsCount));
            OnPropertyChanged(nameof(GetMapLandmarksCount));
            OnPropertyChanged(nameof(GetMapShortcutsCount));
            OnPropertyChanged(nameof(GetMapSnapshotsCount));
            OnPropertyChanged(nameof(GetMapVariablesCount));
            OnPropertyChanged(nameof(GetMapQueriesCount));

            OnPropertyChanged(nameof(GetMapNameLabel));
            OnPropertyChanged(nameof(GetMapEncodingLabel));
            OnPropertyChanged(nameof(GetMapPathLabel));
            OnPropertyChanged(nameof(GetMapDescLabel));
        };
    }
    public int GetMapRoomsCount
    {
        get => AppState.Current != null ? (AppState.Current.Map.Rooms.Count) : 0;
    }
    public int GetMapAliasesCount
    {
        get => AppState.Current != null ? (AppState.Current.Map.Aliases.Count) : 0;
    }
    public int GetMapRoutesCount
    {
        get => AppState.Current != null ? (AppState.Current.Map.Routes.Count) : 0;
    }
    public int GetMapTracesCount
    {
        get => AppState.Current != null ? (AppState.Current.Map.Traces.Count) : 0;
    }
    public int GetMapRegionsCount
    {
        get => AppState.Current != null ? (AppState.Current.Map.Regions.Count) : 0;
    }

    public int GetMapLandmarksCount
    {
        get => AppState.Current != null ? (AppState.Current.Map.Landmarks.Count) : 0;
    }
    public int GetMapShortcutsCount
    {
        get => AppState.Current != null ? (AppState.Current.Map.Shortcuts.Count) : 0;
    }
    public int GetMapVariablesCount
    {
        get => AppState.Current != null ? (AppState.Current.Map.Variables.Count) : 0;
    }
    public int GetMapSnapshotsCount
    {
        get => AppState.Current != null ? (AppState.Current.Map.Snapshots.Count) : 0;
    }
    public int GetMapQueriesCount
    {
        get => AppState.Current != null ? (AppState.Current.Map.Querys.Count) : 0;
    }
    public string GetMapPathLabel
    {
        get => AppState.Current != null ? (AppState.Current.Path != "" ? AppState.Current.Path : "<未保存>") : "";
    }

    public string GetMapNameLabel
    {
        get => AppState.Current != null ? (AppState.Current.Map.Info.NameLabel) : "";
    }
    public string GetMapNamePath
    {
        get => AppState.Current != null ? (AppState.Current.Path) : "";
    }

    public string GetMapEncodingLabel
    {
        get
        {
            if (AppState.Current is not null)
            {
                switch (AppState.Current.Map.Encoding)
                {
                    case MapEncoding.GB18030:
                        return "GB18030";
                    default:
                        return "UTF8";
                }

            }
            return "";
        }
    }
    public string GetMapDescLabel
    {
        get => AppState.Current != null ? (AppState.Current.Map.Info.DescLabel) : "";
    }

}
