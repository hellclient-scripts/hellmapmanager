using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
using HellMapManager.States;
using System.Collections.ObjectModel;


namespace HellMapManager.Windows.UpdateMapWindow;
public class EncodingItem(MapEncoding key, string label)
{
    public MapEncoding Key { get; } = key;
    public string Label { get; } = label;
}
public class UpdateMapWindowViewModel(AppState appstate, MapSettings settings) : ObservableObject
{

    public AppState AppState { get; set; } = appstate;
    public MapSettings Settings { get; set; } = settings;
    public static ObservableCollection<EncodingItem> Items
    {
        get => new([new(MapEncoding.Default, "UTF-8"), new(MapEncoding.GB18030, "GB18030")]);
    }
}
