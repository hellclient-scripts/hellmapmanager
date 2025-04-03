using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using HellMapManager.Models;
using HellMapManager.Services;
using System.Threading.Tasks;
using System.Reflection;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void UpdateOverview()
    {
        OnPropertyChanged(nameof(GetMapRoomsCount));
        OnPropertyChanged(nameof(GetMapAliasesCount));
        OnPropertyChanged(nameof(GetMapRoutesCount));
        OnPropertyChanged(nameof(GetMapVariablesCount));
        OnPropertyChanged(nameof(GetMapNameLabel));
        OnPropertyChanged(nameof(GetMapDescLabel));

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
    public int GetMapVariablesCount
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Variables.Count) : 0;
    }
    public string GetMapNameLabel
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Info.NameLabel) : "";
    }
    public string GetMapDescLabel
    {
        get => this.AppState.Current != null ? (this.AppState.Current.Map.Info.DescLabel) : "";
    }

}
