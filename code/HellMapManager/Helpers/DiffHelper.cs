

using System.Collections.Generic;
using HellMapManager.Models;

namespace HellMapManager.Helpers;

public class DiffHelper
{
    //从source到dest的差异
    //统计出来的差异是相对source的，也就是说应用这些差异会把source变成dest
    public static Diffs Diff(Map source, Map dest)
    {
        var result = new Diffs();
        DiffRooms(source, dest, result.Items);
        DiffMarkers(source, dest, result.Items);
        DiffRoutes(source, dest, result.Items);
        DiffTraces(source, dest, result.Items);
        DiffRegions(source, dest, result.Items);
        DiffLandmarks(source, dest, result.Items);
        DiffShortcuts(source, dest, result.Items);
        DiffVariables(source, dest, result.Items);
        DiffSnapshots(source, dest, result.Items);
        return result;
    }
    // 无法确定各元素是否最后依赖保持共性，所以不使用范型，方便调整
    private static void DiffRooms(Map source, Map dest, List<IDiffItem> result)
    {
        var srcmodels = source.Rooms.GetRange(0, source.Rooms.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Rooms.GetRange(0, dest.Rooms.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
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
                result.Add(new RoomDiff(destmodel));
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
        
    }
    private static void DiffMarkers(Map source, Map dest, List<IDiffItem> result)
    {
        var srcmodels = source.Markers.GetRange(0, source.Markers.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Markers.GetRange(0, dest.Markers.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
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
                result.Add(new MarkerDiff(destmodel));
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
        
    }
    private static void DiffRoutes(Map source, Map dest, List<IDiffItem> result)
    {
        var srcmodels = source.Routes.GetRange(0, source.Routes.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Routes.GetRange(0, dest.Routes.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
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
                result.Add(new RouteDiff(destmodel));
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
        
    }
    private static void DiffTraces(Map source, Map dest, List<IDiffItem> result)
    {
        var srcmodels = source.Traces.GetRange(0, source.Traces.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Traces.GetRange(0, dest.Traces.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
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
                result.Add(new TraceDiff(destmodel));
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
        
    }
    private static void DiffRegions(Map source, Map dest, List<IDiffItem> result)
    {
        var srcmodels = source.Regions.GetRange(0, source.Regions.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Regions.GetRange(0, dest.Regions.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
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
                result.Add(new RegionDiff(destmodel));
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
        
    }
    private static void DiffLandmarks(Map source, Map dest, List<IDiffItem> result)
    {
        var srcmodels = source.Landmarks.GetRange(0, source.Landmarks.Count);
        srcmodels.Sort((a, b) => a.UniqueKey().ToString().CompareTo(b.UniqueKey().ToString()));
        var destmodels = dest.Landmarks.GetRange(0, dest.Landmarks.Count);
        destmodels.Sort((a, b) => a.UniqueKey().ToString().CompareTo(b.UniqueKey().ToString()));
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
                result.Add(new LandmarkDiff(destmodel));
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
        
    }

    private static void DiffShortcuts(Map source, Map dest, List<IDiffItem> result)
    {
        var srcmodels = source.Shortcuts.GetRange(0, source.Shortcuts.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Shortcuts.GetRange(0, dest.Shortcuts.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
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
                result.Add(new ShortcutDiff(destmodel));
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
        
    }

    private static void DiffVariables(Map source, Map dest, List<IDiffItem> result)
    {
        var srcmodels = source.Variables.GetRange(0, source.Variables.Count);
        srcmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
        var destmodels = dest.Variables.GetRange(0, dest.Variables.Count);
        destmodels.Sort((a, b) => a.Key.CompareTo(b.Key));
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
                result.Add(new VariableDiff(destmodel));
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
        
    }
    private static void DiffSnapshots(Map source, Map dest, List<IDiffItem> result)
    {
        var srcmodels = source.Snapshots.GetRange(0, source.Snapshots.Count);
        srcmodels.Sort((a, b) => a.UniqueKey().ToString().CompareTo(b.UniqueKey().ToString()));
        var destmodels = dest.Snapshots.GetRange(0, dest.Snapshots.Count);
        destmodels.Sort((a, b) => a.UniqueKey().ToString().CompareTo(b.UniqueKey().ToString()));
        
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
                result.Add(new SnapshotDiff(destmodel));
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
        
    }

    //将差异应用到源地图文件上
    public static void Apply(Diffs diffs, MapFile source)
    {
        diffs.Items.ForEach(d => ApplyDiff(d, source));
        return;
    }
    private static void ApplyDiff(IDiffItem diff, MapFile source)
    {
        switch (diff)
        {
            case RoomDiff md:
                source.InsertRoom(md.Model);
                break;
            case RemovedRoomDiff rmd:
                source.RemoveRoom(rmd.Key);
                break;
            case MarkerDiff md:
                source.InsertMarker(md.Model);
                break;
            case RemovedMarkerDiff rmd:
                source.RemoveMarker(rmd.Key);
                break;
            case RouteDiff md:
                source.InsertRoute(md.Model);
                break;
            case RemovedRouteDiff rmd:
                source.RemoveRoute(rmd.Key);
                break;
            case TraceDiff md:
                source.InsertTrace(md.Model);
                break;
            case RemovedTraceDiff rmd:
                source.RemoveTrace(rmd.Key);
                break;
            case RegionDiff md:
                source.InsertRegion(md.Model);
                break;
            case RemovedRegionDiff rmd:
                source.RemoveRegion(rmd.Key);
                break;
            case LandmarkDiff md:
                source.InsertLandmark(md.Model);
                break;
            case RemovedLandmarkDiff rmd:
                source.RemoveLandmark(new LandmarkKey(rmd.LandmarkKey, rmd.LandmarkType));
                break;
            case ShortcutDiff md:
                source.InsertShortcut(md.Model);
                break;
            case RemovedShortcutDiff rmd:
                source.RemoveShortcut(rmd.Key);
                break;
            case VariableDiff md:
                source.InsertVariable(md.Model);
                break;
            case RemovedVariableDiff rmd:
                source.RemoveVariable(rmd.Key);
                break;
            case SnapshotDiff md:
                source.InsertSnapshot(md.Model); ;
                break;
            case RemovedSnapshotDiff rmd:
                source.RemoveSnapshot(new SnapshotKey(rmd.SnapshotKey, rmd.SnapshotType, rmd.SnapshotValue));
                break;
        }
    }
}