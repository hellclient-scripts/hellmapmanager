using System.Collections.Generic;

namespace HellMapManager.Models;

public enum DiffType
{
    RoomDiff,
    RemovedRoomDiff,
    MarkerDiff,
    RemovedMarkerDiff,
    RouteDiff,
    RemovedRouteDiff,
    TraceDiff,
    RemovedTraceDiff,
    RegionDiff,
    RemovedRegionDiff,
    LandmarkDiff,
    RemovedLandmarkDiff,
    ShortcutDiff,
    RemovedShortcutDiff,
    VariableDiff,
    RemovedVariableDiff,
    SnapshotDiff,
    RemovedSnapshotDiff,
}
public enum DiffMode
{
    Removed,
    Normal,
    New,
}

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

public class Patch
{
    public bool SkipRooms = false;
    public bool SkipMarkers = false;
    public bool SkipRoutes = false;
    public bool SkipTraces = false;
    public bool SkipRegions = false;
    public bool SkipLandmarks = false;
    public bool SkipShortcuts = false;
    public bool SkipVariables = false;
    public bool SkipSnapshots = false;

    public List<PatchItem> Rooms = [];
    public List<PatchItem> Markers = [];
    public List<PatchItem> Routes = [];
    public List<PatchItem> Traces = [];
    public List<PatchItem> Regions = [];
    public List<PatchItem> Landmarks = [];
    public List<PatchItem> Shortcuts = [];
    public List<PatchItem> Variables = [];
    public List<PatchItem> Snapshots = [];
    public void SelectAll(bool value)
    {
        foreach (var r in Rooms)
        {
            r.Selected = value;
        }
        foreach (var m in Markers)
        {
            m.Selected = value;
        }
        foreach (var r in Routes)
        {
            r.Selected = value;
        }
        foreach (var t in Traces)
        {
            t.Selected = value;
        }
        foreach (var r in Regions)
        {
            r.Selected = value;
        }
        foreach (var l in Landmarks)
        {
            l.Selected = value;
        }
        foreach (var s in Shortcuts)
        {
            s.Selected = value;
        }
        foreach (var v in Variables)
        {
            v.Selected = value;
        }
        foreach (var s in Snapshots)
        {
            s.Selected = value;
        }
    }

    public void SelectByTypeAndMode(string type, DiffMode mode, bool selected)
    {
        List<PatchItem> items;
        switch (type)
        {
            case "Room":
                items = Rooms;
                break;
            case "Marker":
                items = Markers;
                break;
            case "Route":
                items = Routes;
                break;
            case "Trace":
                items = Traces;
                break;
            case "Region":
                items = Regions;
                break;
            case "Landmark":
                items = Landmarks;
                break;
            case "Shortcut":
                items = Shortcuts;
                break;
            case "Variable":
                items = Variables;
                break;
            case "Snapshot":
                items = Snapshots;
                break;
            default:
                return;
        }
        foreach (var item in items)
        {
            if (item.Mode == mode)
            {
                item.Selected = selected;
            }
        }
    }
    public void Sort()
    {
        Rooms.Sort((a, b) => a.Display.DiffKey.CompareTo(b.Display.DiffKey));
        Markers.Sort((a, b) => a.Display.DiffKey.CompareTo(b.Display.DiffKey));
        Routes.Sort((a, b) => a.Display.DiffKey.CompareTo(b.Display.DiffKey));
        Traces.Sort((a, b) => a.Display.DiffKey.CompareTo(b.Display.DiffKey));
        Regions.Sort((a, b) => a.Display.DiffKey.CompareTo(b.Display.DiffKey));
        Landmarks.Sort((a, b) => a.Display.DiffKey.CompareTo(b.Display.DiffKey));
        Shortcuts.Sort((a, b) => a.Display.DiffKey.CompareTo(b.Display.DiffKey));
        Variables.Sort((a, b) => a.Display.DiffKey.CompareTo(b.Display.DiffKey));
        Snapshots.Sort((a, b) => a.Display.DiffKey.CompareTo(b.Display.DiffKey));
    }
}

public class Diffs
{
    public List<IRoomDiff> Rooms = [];
    public List<IMarkerDiff> Markers = [];
    public List<IRouteDiff> Routes = [];
    public List<ITraceDiff> Traces = [];
    public List<IRegionDiff> Regions = [];
    public List<ILandmarkDiff> Landmarks = [];
    public List<IShortcutDiff> Shortcuts = [];
    public List<IVariableDiff> Variables = [];
    public List<ISnapshotDiff> Snapshots = [];
    public void Sort()
    {
        Rooms.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
        Markers.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
        Routes.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
        Traces.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
        Regions.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
        Landmarks.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
        Shortcuts.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
        Variables.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
        Snapshots.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
    }
}

public interface IDiffItem
{
    public string DiffKey { get; }

    public DiffType Type { get; }
    public DiffMode Mode { get; }
    public string Encode();
    public bool Validated();

}
public interface IRoomDiff : IDiffItem
{
    public Room? Data { get; }
    public string Key { get; }

}
public class RoomDiff(Room model) : IRoomDiff
{
    public string DiffKey { get => Model.Key; }
    public DiffType Type { get; } = DiffType.RoomDiff;
    public DiffMode Mode { get; } = DiffMode.Normal;
    public Room Model { get; } = model;
    public Room? Data { get => Model; }
    public string Key { get => Model.Key; }

    public string Encode()
    {
        return Model.Encode();
    }
    public bool Validated()
    {
        return Model.Validated();
    }
}

public class RemovedRoomDiff(string key) : IRoomDiff
{
    public const string EncodeKey = "RemoveRoom";

    public string DiffKey { get => Key; }
    public DiffType Type { get; } = DiffType.RemovedRoomDiff;
    public DiffMode Mode { get; } = DiffMode.Removed;
    public string Key { get; } = key;
    public Room? Data { get => null; }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey, this.Key);
    }
    public static RemovedRoomDiff Decode(string data)
    {
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, data);
        return new RemovedRoomDiff(kv.Value);
    }
    public bool Validated()
    {
        return ItemKey.Validate(Key);
    }
}

public interface IMarkerDiff : IDiffItem
{
    public Marker? Data { get; }
    public string Key { get; }

}

public class MarkerDiff(Marker model) : IMarkerDiff
{
    public string DiffKey { get => Model.Key; }
    public DiffType Type { get; } = DiffType.MarkerDiff;
    public DiffMode Mode { get; } = DiffMode.Normal;
    public Marker Model { get; } = model;
    public Marker? Data { get => Model; }
    public string Key { get => Model.Key; }

    public string Encode()
    {
        return Model.Encode();
    }
    public bool Validated()
    {
        return Model.Validated();
    }
}

public class RemovedMarkerDiff(string key) : IMarkerDiff
{
    public const string EncodeKey = "RemoveMarker";

    public string DiffKey { get => Key; }
    public DiffType Type { get; } = DiffType.RemovedMarkerDiff;
    public DiffMode Mode { get; } = DiffMode.Removed;
    public string Key { get; } = key;
    public Marker? Data { get => null; }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey, this.Key);
    }
    public static RemovedMarkerDiff Decode(string data)
    {
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, data);
        return new RemovedMarkerDiff(kv.Value);
    }
    public bool Validated()
    {
        return ItemKey.Validate(Key);
    }
}

public interface IRouteDiff : IDiffItem
{
    public Route? Data { get; }
    public string Key { get; }

}
public class RouteDiff(Route model) : IRouteDiff
{
    public string DiffKey { get => Model.Key; }
    public DiffType Type { get; } = DiffType.RouteDiff;
    public DiffMode Mode { get; } = DiffMode.Normal;
    public Route Model { get; } = model;
    public Route? Data { get => Model; }
    public string Key { get => Model.Key; }

    public string Encode()
    {
        return Model.Encode();
    }
    public bool Validated()
    {
        return Model.Validated();
    }
}

public class RemovedRouteDiff(string key) : IRouteDiff
{
    public const string EncodeKey = "RemoveRoute";

    public string DiffKey { get => Key; }
    public DiffType Type { get; } = DiffType.RemovedRouteDiff;
    public DiffMode Mode { get; } = DiffMode.Removed;
    public string Key { get; } = key;
    public Route? Data { get => null; }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey, this.Key);
    }
    public static RemovedRouteDiff Decode(string data)
    {
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, data);
        return new RemovedRouteDiff(kv.Value);
    }
    public bool Validated()
    {
        return ItemKey.Validate(Key);
    }
}

public interface ITraceDiff : IDiffItem
{
    public Trace? Data { get; }
    public string Key { get; }

}
public class TraceDiff(Trace model) : ITraceDiff
{
    public string DiffKey { get => Model.Key; }
    public DiffType Type { get; } = DiffType.TraceDiff;
    public DiffMode Mode { get; } = DiffMode.Normal;
    public Trace Model { get; } = model;
    public Trace? Data { get => Model; }
    public string Key { get => Model.Key; }
    public string Encode()
    {
        return Model.Encode();
    }
    public bool Validated()
    {
        return Model.Validated();
    }
}

public class RemovedTraceDiff(string key) : ITraceDiff
{
    public const string EncodeKey = "RemoveTrace";

    public string DiffKey { get => Key; }
    public DiffType Type { get; } = DiffType.RemovedTraceDiff;
    public DiffMode Mode { get; } = DiffMode.Removed;
    public string Key { get; } = key;
    public Trace? Data { get => null; }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey, this.Key);
    }
    public static RemovedTraceDiff Decode(string data)
    {
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, data);
        return new RemovedTraceDiff(kv.Value);
    }
    public bool Validated()
    {
        return ItemKey.Validate(Key);
    }
}

public interface IRegionDiff : IDiffItem
{
    public Region? Data { get; }
    public string Key { get; }

}
public class RegionDiff(Region model) : IRegionDiff
{
    public string DiffKey { get => Model.Key; }
    public DiffType Type { get; } = DiffType.RegionDiff;
    public DiffMode Mode { get; } = DiffMode.Normal;
    public Region Model { get; } = model;
    public Region? Data { get => Model; }
    public string Key { get => Model.Key; }

    public string Encode()
    {
        return Model.Encode();
    }
    public bool Validated()
    {
        return Model.Validated();
    }
}

public class RemovedRegionDiff(string key) : IRegionDiff
{
    public const string EncodeKey = "RemoveRegion";

    public string DiffKey { get => Key; }
    public DiffType Type { get; } = DiffType.RemovedRegionDiff;
    public DiffMode Mode { get; } = DiffMode.Removed;
    public string Key { get; } = key;
    public Region? Data { get => null; }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey, this.Key);
    }
    public static RemovedRegionDiff Decode(string data)
    {
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, data);
        return new RemovedRegionDiff(kv.Value);
    }
    public bool Validated()
    {
        return ItemKey.Validate(Key);
    }
}

public interface ILandmarkDiff : IDiffItem
{
    public Landmark? Data { get; }
    public string LandmarkKey { get; }
    public string LandmarkType { get; }

}
public class LandmarkDiff(Landmark model) : ILandmarkDiff
{
    public string DiffKey { get => Model.UniqueKey().ToString(); }
    public DiffType Type { get; } = DiffType.LandmarkDiff;
    public DiffMode Mode { get; } = DiffMode.Normal;
    public Landmark Model { get; } = model;
    public Landmark? Data { get => Model; }
    public string LandmarkKey { get => Model.Key; }
    public string LandmarkType { get => Model.Type; }
    public string Encode()
    {
        return Model.Encode();
    }
    public bool Validated()
    {
        return Model.Validated();
    }
}

public class RemovedLandmarkDiff(string key, string type) : ILandmarkDiff
{
    public const string EncodeKey = "RemoveLandmark";

    public string DiffKey { get => new LandmarkKey(ModelKey, ModelType).ToString(); }
    public DiffType Type { get; } = DiffType.RemovedLandmarkDiff;
    public DiffMode Mode { get; } = DiffMode.Removed;
    public string ModelKey { get; } = key;
    public string ModelType { get; } = type;
    public string LandmarkKey { get => ModelKey; }
    public string LandmarkType { get => ModelType; }

    public Landmark? Data { get => null; }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey, HMMFormatter.EncodeList(HMMFormatter.Level1, [ModelKey, ModelType]));
    }
    public static RemovedLandmarkDiff Decode(string data)
    {
        var d = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, data);
        var kv = HMMFormatter.DecodeList(HMMFormatter.Level1, d.Value);
        if (kv.Count != 2)
        {
            return new RemovedLandmarkDiff("", "");
        }
        return new RemovedLandmarkDiff(kv[0], kv[1]);
    }
    public bool Validated()
    {
        return ItemKey.Validate(ModelKey);
    }
}

public interface IShortcutDiff : IDiffItem
{
    public Shortcut? Data { get; }
    public string Key { get; }

}
public class ShortcutDiff(Shortcut model) : IShortcutDiff
{
    public string DiffKey { get => Model.Key; }
    public DiffType Type { get; } = DiffType.ShortcutDiff;
    public DiffMode Mode { get; } = DiffMode.Normal;
    public Shortcut Model { get; } = model;
    public Shortcut? Data { get => Model; }
    public string Key { get => Model.Key; }

    public string Encode()
    {
        return Model.Encode();
    }
    public bool Validated()
    {
        return Model.Validated();
    }
}

public class RemovedShortcutDiff(string key) : IShortcutDiff
{
    public const string EncodeKey = "RemoveShortcut";

    public string DiffKey { get => Key; }
    public DiffType Type { get; } = DiffType.RemovedShortcutDiff;
    public DiffMode Mode { get; } = DiffMode.Removed;
    public string Key { get; } = key;
    public Shortcut? Data { get => null; }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey, this.Key);
    }
    public static RemovedShortcutDiff Decode(string data)
    {
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, data);
        return new RemovedShortcutDiff(kv.Value);
    }
    public bool Validated()
    {
        return ItemKey.Validate(Key);
    }
}

public interface IVariableDiff : IDiffItem
{
    public Variable? Data { get; }
    public string Key { get; }
}
public class VariableDiff(Variable model) : IVariableDiff
{
    public string DiffKey { get => Model.Key; }
    public DiffType Type { get; } = DiffType.VariableDiff;
    public DiffMode Mode { get; } = DiffMode.Normal;
    public Variable Model { get; } = model;
    public Variable? Data { get => Model; }
    public string Key { get => Model.Key; }

    public string Encode()
    {
        return Model.Encode();
    }
    public bool Validated()
    {
        return Model.Validated();
    }
}

public class RemovedVariableDiff(string key) : IVariableDiff
{
    public const string EncodeKey = "RemoveVariable";

    public string DiffKey { get => Key; }
    public DiffType Type { get; } = DiffType.RemovedVariableDiff;
    public DiffMode Mode { get; } = DiffMode.Removed;
    public string Key { get; } = key;
    public Variable? Data { get => null; }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey, this.Key);
    }
    public static RemovedVariableDiff Decode(string data)
    {
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, data);
        return new RemovedVariableDiff(kv.Value);
    }
    public bool Validated()
    {
        return ItemKey.Validate(Key);
    }
}

public interface ISnapshotDiff : IDiffItem
{
    public Snapshot? Data { get; }
    public string SnapshotKey { get; }
    public string SnapshotType { get; }
    public string SnapshotValue { get; }

}
public class SnapshotDiff(Snapshot model) : ISnapshotDiff
{
    public string DiffKey { get => Model.UniqueKey().ToString(); }
    public DiffType Type { get; } = DiffType.SnapshotDiff;
    public DiffMode Mode { get; } = DiffMode.Normal;
    public Snapshot Model { get; } = model;
    public Snapshot? Data { get => Model; }
    public string SnapshotKey { get => Model.Key; }
    public string SnapshotType { get => Model.Type; }
    public string SnapshotValue { get => Model.Value; }

    public string Encode()
    {
        return Model.Encode();
    }
    public bool Validated()
    {
        return Model.Validated();
    }
}

public class RemovedSnapshotDiff(string key, string type, string value) : ISnapshotDiff
{
    public const string EncodeKey = "RemoveSnapshot";

    public string DiffKey { get => new SnapshotKey(ModelKey, ModelType, ModelValue).ToString(); }
    public DiffType Type { get; } = DiffType.RemovedSnapshotDiff;
    public DiffMode Mode { get; } = DiffMode.Removed;
    public string ModelKey { get; } = key;
    public string ModelType { get; } = type;
    public string ModelValue { get; } = value;
    public string SnapshotKey { get => ModelKey; }
    public string SnapshotType { get => ModelType; }
    public string SnapshotValue { get => ModelValue; }

    public Snapshot? Data { get => null; }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey, HMMFormatter.EncodeList(HMMFormatter.Level1, [ModelKey, ModelType, ModelValue]));
    }
    public static RemovedSnapshotDiff Decode(string data)
    {
        var d = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, data);
        var kv = HMMFormatter.DecodeList(HMMFormatter.Level1, d.Value);
        if (kv.Count != 3)
        {
            return new RemovedSnapshotDiff("", "", "");
        }
        return new RemovedSnapshotDiff(kv[0], kv[1], kv[2]);
    }
    public bool Validated()
    {
        return ItemKey.Validate(ModelKey);
    }
}


