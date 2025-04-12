using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using HellMapManager.Models;
using HellMapManager.Services;
using System.Threading.Tasks;
using System.Reflection;
using System.Text;
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
            OnPropertyChanged(nameof(GetMapVariablesCount));
            OnPropertyChanged(nameof(GetMapNameLabel));
            OnPropertyChanged(nameof(GetMapEncodingLabel));


        };
    }
    public int GetMapRoomsCount
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Rooms.Count) : 0;
    }
    public int GetMapAliasesCount
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Aliases.Count) : 0;
    }
    public int GetMapRoutesCount
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Routes.Count) : 0;
    }
    public int GetMapTracesCount
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Traces.Count) : 0;
    }
    public int GetMapRegionsCount
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Regions.Count) : 0;
    }

    public int GetMapLandmarksCount
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Landmarks.Count) : 0;
    }
    public int GetMapShortcutsCount
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Shortcuts.Count) : 0;
    }
    public int GetMapVariablesCount
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Variables.Count) : 0;
    }
    public int GetMapSnapshotsCount
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Snapshots.Count) : 0;
    }
    public int GetMapQueriesCount
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Querys.Count) : 0;
    }

    public string GetMapNameLabel
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Info.NameLabel) : "";
    }
    public string GetMapEncodingLabel
    {
        get
        {
            if (this.AppState.Current is not null)
            {
                switch (this.AppState.Current.Map.Encoding)
                {
                    case MapEncoding.GB18030:
                        return "GB18030";
                    default:
                        return "UTF";
                }

            }
            return "";
        }
    }
    public string GetMapDescLabel
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Info.DescLabel) : "";
    }

}
