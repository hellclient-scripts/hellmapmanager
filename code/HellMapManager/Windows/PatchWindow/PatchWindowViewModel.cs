
using System;
using System.Collections.Generic;
using HellMapManager.Models;
using HellMapManager.ViewModels;
using System.Collections.ObjectModel;


namespace HellMapManager.Windows.PatchWindow;


public class PatchTab(Patch patch, string name, string key, PatchCounts counts) : ViewModelBase
{
    public string Name { get; } = name;

    public string Key { get; } = key;

    public PatchCounts Counts { get; } = counts;
    public Patch Patch { get; } = patch;
    public string Label { get => $"{Name}变更( {Counts.Selected} / {Counts.CountAll} )"; }
    public PatchTab BindCount()
    {
        Counts.PropertyChanged += (sender, args) =>
        {
            OnPropertyChanged(nameof(Label));
        };
        return this;
    }
    public bool IsRoom { get => Key == Room.EncodeKey; }

}
public class PatchCounts : ViewModelBase
{
    public void Count(List<PatchItem> items)
    {
        CountAll = 0;
        CountRemoved = 0;
        CountNormal = 0;
        CountNew = 0;
        Selected = 0;
        items.ForEach(item =>
        {
            CountAll++;
            if (item.Selected)
            {
                Selected++;
            }
            switch (item.Mode)
            {
                case DiffMode.Removed:
                    CountRemoved++;
                    break;
                case DiffMode.New:
                    CountNew++;
                    break;
                case DiffMode.Normal:
                    CountNormal++;
                    break;
            }
        });
        OnPropertyChanged(nameof(CountAll));
        OnPropertyChanged(nameof(CountRemoved));
        OnPropertyChanged(nameof(CountNormal));
        OnPropertyChanged(nameof(CountNew));
        OnPropertyChanged(nameof(Selected));
    }
    public int CountAll { get; set; }
    public int CountRemoved { get; set; }
    public int CountNormal { get; set; }
    public int CountNew { get; set; }
    public int Selected { get; set; }
}
public partial class PatchWindowViewModel : ViewModelBase
{
    public partial void InitRooms();
    public event EventHandler? UpdatedEvent;
    public void RaiseUpdatedEvent(object? sender)
    {
        UpdatedEvent?.Invoke(sender, EventArgs.Empty);
    }
    public Avalonia.Media.IBrush PatchItemNewBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Brushes.LightBlue.Color, 0.1);
    public Avalonia.Media.IBrush PatchItemRemovedBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Brushes.LightCoral.Color, 0.1);
    public Avalonia.Media.IBrush PatchItemNormalBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Brushes.LightYellow.Color, 0.1);
    public void SetPatchItemRowBackground(PatchItem item, Avalonia.Controls.DataGridRow row)
    {
        switch (item.Mode)
        {
            case DiffMode.New:
                row.Background = PatchItemNewBrush;
                break;
            case DiffMode.Removed:
                row.Background = PatchItemRemovedBrush;
                break;
            case DiffMode.Normal:
                row.Background = PatchItemNormalBrush;
                break;
        }
    }
    public PatchWindowViewModel(Diffs diffs, Patch patch)
    {
        Diffs = diffs;
        Patch = patch;
        List<PatchTab> tabs =
        [
            new PatchTab(patch, "房间", Room.EncodeKey, CountRooms),
            new PatchTab(patch, "标记", Marker.EncodeKey, CountMarkers),
            new PatchTab(patch, "路线", Route.EncodeKey, CountRoutes),
            new PatchTab(patch, "足迹", Trace.EncodeKey, CountTraces),
            new PatchTab(patch, "地区", Region.EncodeKey, CountRegions),
            new PatchTab(patch, "定位", Landmark.EncodeKey, CountLandmarks),
            new PatchTab(patch, "捷径", Shortcut.EncodeKey, CountShortcuts),
            new PatchTab(patch, "变量", Variable.EncodeKey, CountVariables),
            new PatchTab(patch, "快照", Snapshot.EncodeKey, CountSnapshots),
        ];
        tabs.ForEach(t => Tabs.Add(t.BindCount()));
        SelectedTab = Tabs[0];
        Init();
        InitRooms();
        BindEvents();
        OnPropertyChanged(nameof(Tabs));
    }
    public void BindEvents()
    {
        UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(Tabs));
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(SelectedTab));
            OnPropertyChanged(nameof(CountAllTypes));
            OnPropertyChanged(nameof(CountRooms));
            OnPropertyChanged(nameof(CountMarkers));
            OnPropertyChanged(nameof(CountRoutes));
            OnPropertyChanged(nameof(CountTraces));
            OnPropertyChanged(nameof(CountRegions));
            OnPropertyChanged(nameof(CountLandmarks));
            OnPropertyChanged(nameof(CountShortcuts));
            OnPropertyChanged(nameof(CountVariables));
            OnPropertyChanged(nameof(CountSnapshots));
        };
    }
    public void Init()
    {
        ReCount();
    }
    public void ReCount()
    {
        CountRooms.Count(Patch.Rooms);
        CountMarkers.Count(Patch.Markers);
        CountRoutes.Count(Patch.Routes);
        CountTraces.Count(Patch.Traces);
        CountRegions.Count(Patch.Regions);
        CountLandmarks.Count(Patch.Landmarks);
        CountShortcuts.Count(Patch.Shortcuts);
        CountVariables.Count(Patch.Variables);
        CountSnapshots.Count(Patch.Snapshots);
        CountAllTypes.CountAll = CountRooms.CountAll + CountMarkers.CountAll + CountRoutes.CountAll + CountTraces.CountAll + CountRegions.CountAll + CountLandmarks.CountAll + CountShortcuts.CountAll + CountVariables.CountAll + CountSnapshots.CountAll;
        CountAllTypes.CountNew = CountRooms.CountNew + CountMarkers.CountNew + CountRoutes.CountNew + CountTraces.CountNew + CountRegions.CountNew + CountLandmarks.CountNew + CountShortcuts.CountNew + CountVariables.CountNew + CountSnapshots.CountNew;
        CountAllTypes.CountRemoved = CountRooms.CountRemoved + CountMarkers.CountRemoved + CountRoutes.CountRemoved + CountTraces.CountRemoved + CountRegions.CountRemoved + CountLandmarks.CountRemoved + CountShortcuts.CountRemoved + CountVariables.CountRemoved + CountSnapshots.CountRemoved;
        CountAllTypes.CountNormal = CountRooms.CountNormal + CountMarkers.CountNormal + CountRoutes.CountNormal + CountTraces.CountNormal + CountRegions.CountNormal + CountLandmarks.CountNormal + CountShortcuts.CountNormal + CountVariables.CountNormal;
        CountAllTypes.Selected = CountRooms.Selected + CountMarkers.Selected + CountRoutes.Selected + CountTraces.Selected + CountRegions.Selected + CountLandmarks.Selected + CountShortcuts.Selected + CountVariables.Selected + CountSnapshots.Selected;
        OnPropertyChanged(nameof(CountAllTypes));
        OnPropertyChanged(nameof(StatusMessage));
    }
    public Diffs Diffs { get; }
    public Patch Patch { get; }
    public PatchCounts CountAllTypes { get; set; } = new PatchCounts();
    public PatchCounts CountRooms { get; set; } = new PatchCounts();
    public PatchCounts CountMarkers { get; set; } = new PatchCounts();
    public PatchCounts CountRoutes { get; set; } = new PatchCounts();
    public PatchCounts CountTraces { get; set; } = new PatchCounts();
    public PatchCounts CountRegions { get; set; } = new PatchCounts();
    public PatchCounts CountLandmarks { get; set; } = new PatchCounts();
    public PatchCounts CountShortcuts { get; set; } = new PatchCounts();
    public PatchCounts CountVariables { get; set; } = new PatchCounts();
    public PatchCounts CountSnapshots { get; set; } = new PatchCounts();
    public ObservableCollection<PatchTab> Tabs { get; } = new ObservableCollection<PatchTab>();

    public PatchTab SelectedTab { get; set; }
    public string StatusMessage
    {
        get
        {
            var msg = $"全部变更:{CountAllTypes.CountAll}    新增:{CountAllTypes.CountNew}    删除:{CountAllTypes.CountRemoved}    修改:{CountAllTypes.CountNormal}    选中:{CountAllTypes.Selected}    ";
            if (SelectedTab != null)
            {
                msg += $"|    {SelectedTab.Name}变更:{SelectedTab.Counts.CountAll}    {SelectedTab.Name}新增:{SelectedTab.Counts.CountNew}    {SelectedTab.Name}删除:{SelectedTab.Counts.CountRemoved}    {SelectedTab.Name}修改:{SelectedTab.Counts.CountNormal}    {SelectedTab.Name}选中:{SelectedTab.Counts.Selected}";
            }
            return msg;
        }
    }
    public void OnSelectUpdate()
    {
        OnPropertyChanged(nameof(SelectedTab));
        OnPropertyChanged(nameof(StatusMessage));
    }
    public void OnSelectAll()
    {
        
    }
}
