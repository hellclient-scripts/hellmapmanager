
using System;
using System.Collections.Generic;
using HellMapManager.Models;
using HellMapManager.ViewModels;
using System.Collections.ObjectModel;


namespace HellMapManager.Windows.PatchWindow;


public class PatchTab(PatchType patchtype, string name, string key) : ViewModelBase
{
    public string Name { get; } = name;

    public string Key { get; } = key;

    public PatchCounts Counts { get; } = new PatchCounts();
    public PatchType Type { get; } = patchtype;
    public string Label { get => $"{Name}变更( {Counts.Selected} / {Counts.CountAll} )"; }
    public PatchTab BindCount()
    {
        Counts.PropertyChanged += (sender, args) =>
        {
            OnPropertyChanged(nameof(Label));
        };
        return this;
    }
    public void ReCount()
    {
        Counts.Count(Type.Items);
        OnPropertyChanged(nameof(Label));
    }

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
    public void Reset()
    {
        CountAll = 0;
        CountRemoved = 0;
        CountNormal = 0;
        CountNew = 0;
        Selected = 0;
    }
    public void SumTo(PatchCounts desc)
    {
        desc.CountAll += CountAll;
        desc.CountRemoved += CountRemoved;
        desc.CountNormal += CountNormal;
        desc.CountNew += CountNew;
        desc.Selected += Selected;
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
    public partial void InitMarkers();
    public partial void InitRoutes();
    public partial void InitTraces();
    public partial void InitRegions();
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
    public PatchWindowViewModel(MapFile mf, Diffs diffs)
    {

        Patch = Patch.CreatePatch(mf, diffs, true);
        List<PatchTab> tabs =
        [
            new PatchTab(Patch.Rooms, "房间", Room.EncodeKey),
            new PatchTab(Patch.Markers, "标记", Marker.EncodeKey),
            new PatchTab(Patch.Routes, "路线", Route.EncodeKey),
            new PatchTab(Patch.Traces, "足迹", Trace.EncodeKey),
            new PatchTab(Patch.Regions, "地区", Region.EncodeKey),
            new PatchTab(Patch.Landmarks, "定位", Landmark.EncodeKey),
            new PatchTab(Patch.Shortcuts, "捷径", Shortcut.EncodeKey),
            new PatchTab(Patch.Variables, "变量", Variable.EncodeKey),
            new PatchTab(Patch.Snapshots, "快照", Snapshot.EncodeKey),
        ];
        tabs.ForEach(t => Tabs.Add(t.BindCount()));
        SelectedTab = Tabs[0];
        Init();
        InitRooms();
        InitMarkers();
        InitRoutes();
        InitTraces();
        InitRegions();
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
        };
    }
    public void Init()
    {
        ReCount(false);
    }
    public void ReCount(bool sumOnly)
    {
        CountAllTypes.Reset();
        foreach (var v in Tabs)
        {
            if (!sumOnly)
            {
                v.ReCount();
            }
            v.Counts.SumTo(CountAllTypes);
        }
        OnPropertyChanged(nameof(CountAllTypes));
        OnPropertyChanged(nameof(StatusMessage));
    }
    public Patch Patch { get; }
    public PatchCounts CountAllTypes { get; set; } = new PatchCounts();
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
    public void SelectAll(bool value)
    {
        foreach (var tab in Tabs)
        {
            tab.Type.SelectAll(value);
        }
        ReCount(false);
        OnPropertyChanged(nameof(SelectedTab));
    }
    public void SelectByMode(DiffMode mode, bool value)
    {
        foreach (var tab in Tabs)
        {
            tab.Type.SelectByMode(mode, value);
        }
        ReCount(false);
        OnPropertyChanged(nameof(SelectedTab));
    }

}
