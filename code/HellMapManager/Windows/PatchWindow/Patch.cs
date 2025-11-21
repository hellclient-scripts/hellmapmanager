using HellMapManager.Models;
using System.Collections.Generic;

namespace HellMapManager.Windows.PatchWindow;

public class PatchItem(IDiffItem display, IDiffItem raw, DiffMode mode, bool selected)
{
    public DiffMode Mode { get; } = mode;
    public IDiffItem Raw { get; } = raw;
    public IDiffItem Display { get; } = display;
    public bool Selected { get; set; } = selected;

    public string ModeName
    {
        get
        {
            return Mode switch
            {
                DiffMode.Removed => "删除",
                DiffMode.New => "新增",
                DiffMode.Normal => "修改",
                _ => "未知",
            };
        }
    }
}

public class PatchType
{
    public bool Skip = false;
    public List<PatchItem> Items = [];
    public void Arrange()
    {
        Items.Sort((a, b) => a.Display.DiffKey.CompareTo(b.Display.DiffKey));
    }
    public void Apply(Diffs diffs)
    {
        Items.ForEach(i => diffs.Items.Add(i.Raw));
    }
    public void SelectByMode(DiffMode mode, bool selected)
    {
        foreach (var item in Items)
        {
            if (item.Mode == mode)
            {
                item.Selected = selected;
            }
        }
    }
    public void SelectAll(bool selected)
    {
        foreach (var item in Items)
        {
            item.Selected = selected;
        }
    }
}

public class Patch
{
    public PatchType Rooms = new();
    public PatchType Markers = new();

    public PatchType Routes = new();

    public PatchType Traces = new();
    public PatchType Regions = new();
    public PatchType Landmarks = new();
    public PatchType Shortcuts = new();
    public PatchType Variables = new();
    public PatchType Snapshots = new();
    public void SelectAll(bool value)
    {
        Rooms.SelectAll(value);
        Markers.SelectAll(value);
        Routes.SelectAll(value);
        Traces.SelectAll(value);
        Regions.SelectAll(value);
        Landmarks.SelectAll(value);
        Shortcuts.SelectAll(value);
        Variables.SelectAll(value);
        Snapshots.SelectAll(value);
    }

    public void SelectByMode(DiffMode mode, bool selected)
    {
        Rooms.SelectByMode(mode, selected);
        Markers.SelectByMode(mode, selected);
        Routes.SelectByMode(mode, selected);
        Traces.SelectByMode(mode, selected);
        Regions.SelectByMode(mode, selected);
        Landmarks.SelectByMode(mode, selected);
        Shortcuts.SelectByMode(mode, selected);
        Variables.SelectByMode(mode, selected);
        Snapshots.SelectByMode(mode, selected);
    }
    public void Arrange()
    {
        Rooms.Arrange();
        Markers.Arrange();
        Routes.Arrange();
        Traces.Arrange();
        Regions.Arrange();
        Landmarks.Arrange();
        Shortcuts.Arrange();
        Variables.Arrange();
        Snapshots.Arrange();
    }
    public static Patch CreatePatch(MapFile mf, Diffs diffs, bool defaultSelected)
    {
        var patch = new Patch();
        foreach (var item in diffs.Items)
        {
            switch (item)
            {

                case IRoomDiff model:

                    if (mf.Cache.Rooms.ContainsKey(model.DiffKey))
                    {
                        if (model.Data == null)
                        {
                            patch.Rooms.Items.Add(new PatchItem(new RoomDiff(mf.Cache.Rooms[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                        }
                        else if (!mf.Cache.Rooms[model.DiffKey].Equal(model.Data))
                        {
                            patch.Rooms.Items.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                        }
                    }
                    else if (model.Data is not null)
                    {
                        patch.Rooms.Items.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
                    }
                    break;

                case IMarkerDiff model:
                    if (mf.Cache.Markers.ContainsKey(model.DiffKey))
                    {
                        if (model.Data == null)
                        {
                            patch.Markers.Items.Add(new PatchItem(new MarkerDiff(mf.Cache.Markers[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                        }
                        else if (!mf.Cache.Markers[model.DiffKey].Equal(model.Data))
                        {
                            patch.Markers.Items.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                        }
                    }
                    else if (model.Data is not null)
                    {
                        patch.Markers.Items.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
                    }
                    break;
                case IRouteDiff model:

                    if (mf.Cache.Routes.ContainsKey(model.DiffKey))
                    {
                        if (model.Data == null)
                        {
                            patch.Routes.Items.Add(new PatchItem(new RouteDiff(mf.Cache.Routes[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                        }
                        else if (!mf.Cache.Routes[model.DiffKey].Equal(model.Data))
                        {
                            patch.Routes.Items.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                        }
                    }
                    else if (model.Data is not null)
                    {
                        patch.Routes.Items.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
                    }
                    break;
                case ITraceDiff model:

                    if (mf.Cache.Traces.ContainsKey(model.DiffKey))
                    {
                        if (model.Data == null)
                        {
                            patch.Traces.Items.Add(new PatchItem(new TraceDiff(mf.Cache.Traces[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                        }
                        else if (!mf.Cache.Traces[model.DiffKey].Equal(model.Data))
                        {
                            patch.Traces.Items.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                        }
                    }
                    else if (model.Data is not null)
                    {
                        patch.Traces.Items.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
                    }
                    break;
                case IRegionDiff model:
                    if (mf.Cache.Regions.ContainsKey(model.DiffKey))
                    {
                        if (model.Data == null)
                        {
                            patch.Regions.Items.Add(new PatchItem(new RegionDiff(mf.Cache.Regions[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                        }
                        else if (!mf.Cache.Regions[model.DiffKey].Equal(model.Data))
                        {
                            patch.Regions.Items.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                        }
                    }
                    else if (model.Data is not null)
                    {
                        patch.Regions.Items.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
                    }
                    break;
                case ILandmarkDiff model:
                    if (mf.Cache.Landmarks.ContainsKey(model.DiffKey))
                    {
                        if (model.Data == null)
                        {
                            patch.Landmarks.Items.Add(new PatchItem(new LandmarkDiff(mf.Cache.Landmarks[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                        }
                        else if (!mf.Cache.Landmarks[model.DiffKey].Equal(model.Data))
                        {
                            patch.Landmarks.Items.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                        }
                    }
                    else if (model.Data is not null)
                    {
                        patch.Landmarks.Items.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
                    }
                    break;
                case IShortcutDiff model:

                    if (mf.Cache.Shortcuts.ContainsKey(model.DiffKey))
                    {
                        if (model.Data == null)
                        {
                            patch.Shortcuts.Items.Add(new PatchItem(new ShortcutDiff(mf.Cache.Shortcuts[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                        }
                        else if (!mf.Cache.Shortcuts[model.DiffKey].Equal(model.Data))
                        {
                            patch.Shortcuts.Items.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                        }
                    }
                    else if (model.Data is not null)
                    {
                        patch.Shortcuts.Items.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
                    }
                    break;
                case IVariableDiff model:
                    if (mf.Cache.Variables.ContainsKey(model.DiffKey))
                    {
                        if (model.Data == null)
                        {
                            patch.Variables.Items.Add(new PatchItem(new VariableDiff(mf.Cache.Variables[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                        }
                        else if (!mf.Cache.Variables[model.DiffKey].Equal(model.Data))
                        {
                            patch.Variables.Items.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                        }
                    }
                    else if (model.Data is not null)
                    {
                        patch.Variables.Items.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
                    }
                    break;
                case ISnapshotDiff model:

                    if (mf.Cache.Snapshots.ContainsKey(model.DiffKey))
                    {
                        if (model.Data == null)
                        {
                            patch.Snapshots.Items.Add(new PatchItem(new SnapshotDiff(mf.Cache.Snapshots[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                        }
                        else if (!mf.Cache.Snapshots[model.DiffKey].Equal(model.Data))
                        {
                            patch.Snapshots.Items.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                        }
                    }
                    else if (model.Data is not null)
                    {
                        patch.Snapshots.Items.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
                    }
                    break;
            }

        }

        return patch;
    }
    //根据选择的差异过滤差异
    //实现用户选择性的应用差异
    public Diffs Apply()
    {
        var result = new Diffs();
        Rooms.Apply(result);
        Markers.Apply(result);
        Routes.Apply(result);
        Traces.Apply(result);
        Regions.Apply(result);
        Landmarks.Apply(result);
        Shortcuts.Apply(result);
        Variables.Apply(result);
        Snapshots.Apply(result);
        return result;
    }

}

