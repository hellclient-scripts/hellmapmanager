using System;
using HellMapManager.States;
using HellMapManager.Models;
using System.Collections.ObjectModel;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitOverview()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(GetMapRoomsCount));
            OnPropertyChanged(nameof(GetMapMarkersCount));
            OnPropertyChanged(nameof(GetMapRoutesCount));
            OnPropertyChanged(nameof(GetMapTracesCount));
            OnPropertyChanged(nameof(GetMapRegionsCount));
            OnPropertyChanged(nameof(GetMapLandmarksCount));
            OnPropertyChanged(nameof(GetMapShortcutsCount));
            OnPropertyChanged(nameof(GetMapSnapshotsCount));
            OnPropertyChanged(nameof(GetMapVariablesCount));

            OnPropertyChanged(nameof(GetMapNameLabel));
            OnPropertyChanged(nameof(GetMapEncodingLabel));
            OnPropertyChanged(nameof(GetMapPathLabel));
            OnPropertyChanged(nameof(GetMapDescLabel));
            OnPropertyChanged(nameof(LastModifiedLabel));
            OnPropertyChanged(nameof(IsMapNameEmpty));
            OnPropertyChanged(nameof(IsMapPathEmpty));
            OnPropertyChanged(nameof(IsMapDescEmpty));
        };
    }
    public int GetMapRoomsCount
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Rooms.Count) : 0;
    }

    public int GetMapMarkersCount
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Markers.Count) : 0;
    }
    public int GetMapRoutesCount
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Routes.Count) : 0;
    }
    public int GetMapTracesCount
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Traces.Count) : 0;
    }
    public int GetMapRegionsCount
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Regions.Count) : 0;
    }

    public int GetMapLandmarksCount
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Landmarks.Count) : 0;
    }
    public int GetMapShortcutsCount
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Shortcuts.Count) : 0;
    }
    public int GetMapVariablesCount
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Variables.Count) : 0;
    }
    public int GetMapSnapshotsCount
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Snapshots.Count) : 0;
    }
    public string GetMapPathLabel
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Path != "" ? AppState.Main.Current.Path : "<未保存>") : "";
    }

    public string GetMapNameLabel
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Info.NameLabel) : "";
    }
    public string GetMapNamePath
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Path) : "";
    }

    public string GetMapEncodingLabel
    {
        get
        {
            if (AppState.Main.Current is not null)
            {
                switch (AppState.Main.Current.Map.Encoding)
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
    public string LastModifiedLabel
    {
        get => AppState.Main.Current != null ? (DateTimeOffset.FromUnixTimeSeconds(AppState.Main.Current.Map.Info.UpdatedTime).LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss")) : "";
    }
    public string GetMapDescLabel
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Info.DescLabel) : "";
    }
    public bool IsMapPathEmpty
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Path == "") : true;
    }
    public bool IsMapNameEmpty
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Info.Name == "") : true;
    }
    public bool IsMapDescEmpty
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Info.Name == "") : true;
    }
}
