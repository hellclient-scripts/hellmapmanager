using System;
using HellMapManager.Cores;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitOverview()
    {
        AppKernel.MapDatabase.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
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
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Rooms.Count) : 0;
    }

    public int GetMapMarkersCount
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Markers.Count) : 0;
    }
    public int GetMapRoutesCount
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Routes.Count) : 0;
    }
    public int GetMapTracesCount
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Traces.Count) : 0;
    }
    public int GetMapRegionsCount
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Regions.Count) : 0;
    }

    public int GetMapLandmarksCount
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Landmarks.Count) : 0;
    }
    public int GetMapShortcutsCount
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Shortcuts.Count) : 0;
    }
    public int GetMapVariablesCount
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Variables.Count) : 0;
    }
    public int GetMapSnapshotsCount
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Snapshots.Count) : 0;
    }
    public string GetMapPathLabel
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Path != "" ? AppKernel.MapDatabase.Current.Path : "<未保存>") : "";
    }

    public string GetMapNameLabel
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Info.NameLabel) : "";
    }

    public string GetMapEncodingLabel
    {
        get
        {
            if (AppKernel.MapDatabase.Current is not null)
            {
                switch (AppKernel.MapDatabase.Current.Map.Encoding)
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
        get => AppKernel.MapDatabase.Current != null ? (DateTimeOffset.FromUnixTimeSeconds(AppKernel.MapDatabase.Current.Map.Info.UpdatedTime).LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss")) : "";
    }
    public string GetMapDescLabel
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Info.DescLabel) : "";
    }
    public bool IsMapPathEmpty
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Path == "") : true;
    }
    public bool IsMapNameEmpty
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Info.Name == "") : true;
    }
    public bool IsMapDescEmpty
    {
        get => AppKernel.MapDatabase.Current != null ? (AppKernel.MapDatabase.Current.Map.Info.Name == "") : true;
    }
}
