using System.Collections.Generic;
using System;


using HellMapManager.Models;
using System.Linq;

namespace HellMapManager.Helpers;

public class DiffHelper
{
    //从source到dest的差异
    //统计出来的差异是相对source的，也就是说应用这些差异会把source变成dest
    public static Diffs Diff(Map source, Map dest)
    {
        var result = new Diffs();
        DiffRooms(source, dest, result);
        DiffMarkers(source, dest, result);
        DiffRoutes(source, dest, result);
        DiffTraces(source, dest, result);
        DiffRegions(source, dest, result);
        DiffLandmarks(source, dest, result);
        DiffShortcuts(source, dest, result);
        DiffVariables(source, dest, result);
        DiffSnapshots(source, dest, result);
        return result;
    }
    // 无法确定各元素是否最后依赖保持共性，所以不使用范型，方便调整
    private static void DiffRooms(Map source, Map dest, Diffs diffs)
    {
        var srcmodels = source.Rooms.GetRange(0, source.Rooms.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Rooms.GetRange(0, dest.Rooms.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var result = diffs.Rooms;
        while (srcmodels.Count > 0 && destmodels.Count > 0)
        {
            var srcmodel = srcmodels[0];
            var destmodel = destmodels[0];
            var delta = srcmodel.Key.CompareTo(destmodel.Key);
            if (delta < 0)
            {
                result.Add(new RemovedRoomDiff(srcmodel.Key));
                srcmodels.RemoveAt(0);
                continue;
            }
            else if (delta > 0)
            {
                result.Add(new RoomDiff(srcmodel));
                destmodels.RemoveAt(0);
                continue;

            }
            if (srcmodel.Encode() != destmodel.Encode())
            {
                result.Add(new RoomDiff(destmodel));
            }
            srcmodels.RemoveAt(0);
            destmodels.RemoveAt(0);
        }
        srcmodels.ForEach(r => result.Add(new RemovedRoomDiff(r.Key)));
        destmodels.ForEach(r => result.Add(new RoomDiff(r)));
        result.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
    }
    private static void DiffMarkers(Map source, Map dest, Diffs diffs)
    {
        var srcmodels = source.Markers.GetRange(0, source.Rooms.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Markers.GetRange(0, dest.Rooms.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var result = diffs.Markers;
        while (srcmodels.Count > 0 && destmodels.Count > 0)
        {
            var srcmodel = srcmodels[0];
            var destmodel = destmodels[0];
            var delta = srcmodel.Key.CompareTo(destmodel.Key);
            if (delta < 0)
            {
                result.Add(new RemovedMarkerDiff(srcmodel.Key));
                srcmodels.RemoveAt(0);
                continue;
            }
            else if (delta > 0)
            {
                result.Add(new MarkerDiff(srcmodel));
                destmodels.RemoveAt(0);
                continue;

            }
            if (srcmodel.Encode() != destmodel.Encode())
            {
                result.Add(new MarkerDiff(destmodel));
            }
            srcmodels.RemoveAt(0);
            destmodels.RemoveAt(0);
        }
        srcmodels.ForEach(r => result.Add(new RemovedMarkerDiff(r.Key)));
        destmodels.ForEach(r => result.Add(new MarkerDiff(r)));
        result.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
    }
    private static void DiffRoutes(Map source, Map dest, Diffs diffs)
    {
        var srcmodels = source.Routes.GetRange(0, source.Rooms.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Routes.GetRange(0, dest.Rooms.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var result = diffs.Routes;
        while (srcmodels.Count > 0 && destmodels.Count > 0)
        {
            var srcmodel = srcmodels[0];
            var destmodel = destmodels[0];
            var delta = srcmodel.Key.CompareTo(destmodel.Key);
            if (delta < 0)
            {
                result.Add(new RemovedRouteDiff(srcmodel.Key));
                srcmodels.RemoveAt(0);
                continue;
            }
            else if (delta > 0)
            {
                result.Add(new RouteDiff(srcmodel));
                destmodels.RemoveAt(0);
                continue;

            }
            if (srcmodel.Encode() != destmodel.Encode())
            {
                result.Add(new RouteDiff(destmodel));
            }
            srcmodels.RemoveAt(0);
            destmodels.RemoveAt(0);
        }
        srcmodels.ForEach(r => result.Add(new RemovedRouteDiff(r.Key)));
        destmodels.ForEach(r => result.Add(new RouteDiff(r)));
        result.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
    }
    private static void DiffTraces(Map source, Map dest, Diffs diffs)
    {
        var srcmodels = source.Traces.GetRange(0, source.Rooms.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Traces.GetRange(0, dest.Rooms.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var result = diffs.Traces;
        while (srcmodels.Count > 0 && destmodels.Count > 0)
        {
            var srcmodel = srcmodels[0];
            var destmodel = destmodels[0];
            var delta = srcmodel.Key.CompareTo(destmodel.Key);
            if (delta < 0)
            {
                result.Add(new RemovedTraceDiff(srcmodel.Key));
                srcmodels.RemoveAt(0);
                continue;
            }
            else if (delta > 0)
            {
                result.Add(new TraceDiff(srcmodel));
                destmodels.RemoveAt(0);
                continue;

            }
            if (srcmodel.Encode() != destmodel.Encode())
            {
                result.Add(new TraceDiff(destmodel));
            }
            srcmodels.RemoveAt(0);
            destmodels.RemoveAt(0);
        }
        srcmodels.ForEach(r => result.Add(new RemovedTraceDiff(r.Key)));
        destmodels.ForEach(r => result.Add(new TraceDiff(r)));
        result.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
    }
    private static void DiffRegions(Map source, Map dest, Diffs diffs)
    {
        var srcmodels = source.Regions.GetRange(0, source.Rooms.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Regions.GetRange(0, dest.Rooms.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var result = diffs.Regions;
        while (srcmodels.Count > 0 && destmodels.Count > 0)
        {
            var srcmodel = srcmodels[0];
            var destmodel = destmodels[0];
            var delta = srcmodel.Key.CompareTo(destmodel.Key);
            if (delta < 0)
            {
                result.Add(new RemovedRegionDiff(srcmodel.Key));
                srcmodels.RemoveAt(0);
                continue;
            }
            else if (delta > 0)
            {
                result.Add(new RegionDiff(srcmodel));
                destmodels.RemoveAt(0);
                continue;

            }
            if (srcmodel.Encode() != destmodel.Encode())
            {
                result.Add(new RegionDiff(destmodel));
            }
            srcmodels.RemoveAt(0);
            destmodels.RemoveAt(0);
        }
        srcmodels.ForEach(r => result.Add(new RemovedRegionDiff(r.Key)));
        destmodels.ForEach(r => result.Add(new RegionDiff(r)));
        result.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
    }
    private static void DiffLandmarks(Map source, Map dest, Diffs diffs)
    {
        var srcmodels = source.Landmarks.GetRange(0, source.Rooms.Count);
        srcmodels.Sort((a, b) => a.UniqueKey().ToString().CompareTo(b.UniqueKey().ToString()));
        var destmodels = dest.Landmarks.GetRange(0, dest.Rooms.Count);
        destmodels.Sort((a, b) => a.UniqueKey().ToString().CompareTo(b.UniqueKey().ToString()));
        var result = diffs.Landmarks;
        while (srcmodels.Count > 0 && destmodels.Count > 0)
        {
            var srcmodel = srcmodels[0];
            var destmodel = destmodels[0];
            var delta = srcmodel.UniqueKey().ToString().CompareTo(destmodel.UniqueKey().ToString());
            if (delta < 0)
            {
                result.Add(new RemovedLandmarkDiff(srcmodel.Key, srcmodel.Type));
                srcmodels.RemoveAt(0);
                continue;
            }
            else if (delta > 0)
            {
                result.Add(new LandmarkDiff(srcmodel));
                destmodels.RemoveAt(0);
                continue;

            }
            if (srcmodel.Encode() != destmodel.Encode())
            {
                result.Add(new LandmarkDiff(destmodel));
            }
            srcmodels.RemoveAt(0);
            destmodels.RemoveAt(0);
        }
        srcmodels.ForEach(r => result.Add(new RemovedLandmarkDiff(r.Key, r.Type)));
        destmodels.ForEach(r => result.Add(new LandmarkDiff(r)));
        result.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
    }

    private static void DiffShortcuts(Map source, Map dest, Diffs diffs)
    {
        var srcmodels = source.Shortcuts.GetRange(0, source.Rooms.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Shortcuts.GetRange(0, dest.Rooms.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var result = diffs.Shortcuts;
        while (srcmodels.Count > 0 && destmodels.Count > 0)
        {
            var srcmodel = srcmodels[0];
            var destmodel = destmodels[0];
            var delta = srcmodel.Key.CompareTo(destmodel.Key);
            if (delta < 0)
            {
                result.Add(new RemovedShortcutDiff(srcmodel.Key));
                srcmodels.RemoveAt(0);
                continue;
            }
            else if (delta > 0)
            {
                result.Add(new ShortcutDiff(srcmodel));
                destmodels.RemoveAt(0);
                continue;

            }
            if (srcmodel.Encode() != destmodel.Encode())
            {
                result.Add(new ShortcutDiff(destmodel));
            }
            srcmodels.RemoveAt(0);
            destmodels.RemoveAt(0);
        }
        srcmodels.ForEach(r => result.Add(new RemovedShortcutDiff(r.Key)));
        destmodels.ForEach(r => result.Add(new ShortcutDiff(r)));
        result.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
    }

    private static void DiffVariables(Map source, Map dest, Diffs diffs)
    {
        var srcmodels = source.Variables.GetRange(0, source.Rooms.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Variables.GetRange(0, dest.Rooms.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var result = diffs.Variables;
        while (srcmodels.Count > 0 && destmodels.Count > 0)
        {
            var srcmodel = srcmodels[0];
            var destmodel = destmodels[0];
            var delta = srcmodel.Key.CompareTo(destmodel.Key);
            if (delta < 0)
            {
                result.Add(new RemovedVariableDiff(srcmodel.Key));
                srcmodels.RemoveAt(0);
                continue;
            }
            else if (delta > 0)
            {
                result.Add(new VariableDiff(srcmodel));
                destmodels.RemoveAt(0);
                continue;

            }
            if (srcmodel.Encode() != destmodel.Encode())
            {
                result.Add(new VariableDiff(destmodel));
            }
            srcmodels.RemoveAt(0);
            destmodels.RemoveAt(0);
        }
        srcmodels.ForEach(r => result.Add(new RemovedVariableDiff(r.Key)));
        destmodels.ForEach(r => result.Add(new VariableDiff(r)));
        result.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
    }
    private static void DiffSnapshots(Map source, Map dest, Diffs diffs)
    {
        var srcmodels = source.Snapshots.GetRange(0, source.Rooms.Count);
        srcmodels.Sort((a, b) => a.UniqueKey().ToString().CompareTo(b.UniqueKey().ToString()));
        var destmodels = dest.Snapshots.GetRange(0, dest.Rooms.Count);
        destmodels.Sort((a, b) => a.UniqueKey().ToString().CompareTo(b.UniqueKey().ToString()));
        var result = diffs.Snapshots;
        while (srcmodels.Count > 0 && destmodels.Count > 0)
        {
            var srcmodel = srcmodels[0];
            var destmodel = destmodels[0];
            var delta = srcmodel.UniqueKey().ToString().CompareTo(destmodel.UniqueKey().ToString());
            if (delta < 0)
            {
                result.Add(new RemovedSnapshotDiff(srcmodel.Key, srcmodel.Type, srcmodel.Value));
                srcmodels.RemoveAt(0);
                continue;
            }
            else if (delta > 0)
            {
                result.Add(new SnapshotDiff(srcmodel));
                destmodels.RemoveAt(0);
                continue;

            }
            if (srcmodel.Encode() != destmodel.Encode())
            {
                result.Add(new SnapshotDiff(destmodel));
            }
            srcmodels.RemoveAt(0);
            destmodels.RemoveAt(0);
        }
        srcmodels.ForEach(r => result.Add(new RemovedSnapshotDiff(r.Key, r.Type, r.Value)));
        destmodels.ForEach(r => result.Add(new SnapshotDiff(r)));
        result.Sort((a, b) => a.DiffKey.CompareTo(b.DiffKey));
    }


    //根据选择的差异过滤差异
    //实现用户选择性的应用差异
    public static Diffs Filter(Diffs diff, SelectedDiffs selected)
    {
        var result = new Diffs();
        if (!selected.SkipRooms)
        {
            diff.Rooms.ForEach((d)
            =>
            {
                if (selected.Rooms.ContainsKey(d.DiffKey)) { result.Rooms.Add(d); }
            });
        }
        if (!selected.SkipMarkers)
        {
            diff.Markers.ForEach((d)
            =>
            {
                if (selected.Markers.ContainsKey(d.DiffKey)) { result.Markers.Add(d); }
            });
        }
        if (!selected.SkipRoutes)
        {
            diff.Routes.ForEach((d)
            =>
            {
                if (selected.Routes.ContainsKey(d.DiffKey)) { result.Routes.Add(d); }
            });
        }
        if (!selected.SkipTraces)
        {
            diff.Traces.ForEach((d)
            =>
            {
                if (selected.Traces.ContainsKey(d.DiffKey)) { result.Traces.Add(d); }
            });
        }
        if (!selected.SkipRegions)
        {
            diff.Regions.ForEach((d)
            =>
            {
                if (selected.Regions.ContainsKey(d.DiffKey)) { result.Regions.Add(d); }
            });
        }
        if (!selected.SkipRegions)
        {
            diff.Regions.ForEach((d)
            =>
            {
                if (selected.Regions.ContainsKey(d.DiffKey)) { result.Regions.Add(d); }
            });
        }
        if (!selected.SkipRegions)
        {
            diff.Regions.ForEach((d)
            =>
            {
                if (selected.Regions.ContainsKey(d.DiffKey)) { result.Regions.Add(d); }
            });
        }
        if (!selected.SkipLandmarks)
        {
            diff.Landmarks.ForEach((d)
            =>
            {
                if (selected.Landmarks.ContainsKey(d.DiffKey)) { result.Landmarks.Add(d); }
            });
        }
        if (!selected.SkipShortcuts)
        {
            diff.Shortcuts.ForEach((d)
            =>
            {
                if (selected.Shortcuts.ContainsKey(d.DiffKey)) { result.Shortcuts.Add(d); }
            });
        }
        if (!selected.SkipVariables)
        {
            diff.Variables.ForEach((d)
            =>
            {
                if (selected.Variables.ContainsKey(d.DiffKey)) { result.Variables.Add(d); }
            });
        }
        if (!selected.SkipSnapshots)
        {
            diff.Snapshots.ForEach((d)
            =>
            {
                if (selected.Snapshots.ContainsKey(d.DiffKey)) { result.Snapshots.Add(d); }
            });
        }

        return result;
    }
    //将差异应用到源地图文件上
    public static void Apply(Diffs diffs, MapFile source)
    {
        return;
    }
}