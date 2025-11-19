

using HellMapManager.Models;

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
    public static Diffs ApplyPatch(Patch patch)
    {
        var result = new Diffs();
        if (!patch.SkipRooms)
        {
            foreach (var i in patch.Rooms)
            {
                if (i.Selected) { result.Rooms.Add((i.Raw as IRoomDiff)!); }
            }
        }
        if (!patch.SkipMarkers)
        {
            foreach (var i in patch.Markers)
            {
                if (i.Selected) { result.Markers.Add((i.Raw as IMarkerDiff)!); }
            }
        }
        if (!patch.SkipRoutes)
        {
            foreach (var i in patch.Routes)
            {
                if (i.Selected) { result.Routes.Add((i.Raw as IRouteDiff)!); }
            }
        }
        if (!patch.SkipTraces)
        {
            foreach (var i in patch.Traces)
            {
                if (i.Selected) { result.Traces.Add((i.Raw as ITraceDiff)!); }
            }
        }
        if (!patch.SkipRegions)
        {
            foreach (var i in patch.Regions)
            {
                if (i.Selected) { result.Regions.Add((i.Raw as IRegionDiff)!); }
            }
        }
        if (!patch.SkipLandmarks)
        {
            foreach (var i in patch.Landmarks)
            {
                if (i.Selected) { result.Landmarks.Add((i.Raw as ILandmarkDiff)!); }
            }
        }
        if (!patch.SkipShortcuts)
        {
            foreach (var i in patch.Shortcuts)
            {
                if (i.Selected) { result.Shortcuts.Add((i.Raw as IShortcutDiff)!); }
            }
        }
        if (!patch.SkipVariables)
        {
            foreach (var i in patch.Variables)
            {
                if (i.Selected) { result.Variables.Add((i.Raw as IVariableDiff)!); }
            }
        }
        if (!patch.SkipSnapshots)
        {
            foreach (var i in patch.Snapshots)
            {
                if (i.Selected) { result.Snapshots.Add((i.Raw as ISnapshotDiff)!); }
            }
        }
        return result;
    }
    public static Patch CreatePatch(MapFile mf, Diffs diffs, bool defaultSelected)
    {
        var patch = new Patch();
        foreach (var model in diffs.Rooms)
        {
            if (mf.Cache.Rooms.ContainsKey(model.DiffKey))
            {
                if (model.Data == null)
                {
                    patch.Rooms.Add(new PatchItem(new RoomDiff(mf.Cache.Rooms[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                }
                else if (!mf.Cache.Rooms[model.DiffKey].Equal(model.Data))
                {
                    patch.Rooms.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                }
            }
            else if (model.Data is not null)
            {
                patch.Rooms.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
            }
        }
        foreach (var model in diffs.Markers)
        {
            if (mf.Cache.Markers.ContainsKey(model.DiffKey))
            {
                if (model.Data == null)
                {
                    patch.Markers.Add(new PatchItem(new MarkerDiff(mf.Cache.Markers[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                }
                else if (!mf.Cache.Markers[model.DiffKey].Equal(model.Data))
                {
                    patch.Markers.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                }
            }
            else if (model.Data is not null)
            {
                patch.Markers.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
            }
        }
        foreach (var model in diffs.Routes)
        {
            if (mf.Cache.Routes.ContainsKey(model.DiffKey))
            {
                if (model.Data == null)
                {
                    patch.Routes.Add(new PatchItem(new RouteDiff(mf.Cache.Routes[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                }
                else if (!mf.Cache.Routes[model.DiffKey].Equal(model.Data))
                {
                    patch.Routes.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                }
            }
            else if (model.Data is not null)
            {
                patch.Routes.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
            }
        }
        foreach (var model in diffs.Traces)
        {
            if (mf.Cache.Traces.ContainsKey(model.DiffKey))
            {
                if (model.Data == null)
                {
                    patch.Traces.Add(new PatchItem(new TraceDiff(mf.Cache.Traces[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                }
                else if (!mf.Cache.Traces[model.DiffKey].Equal(model.Data))
                {
                    patch.Traces.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                }
            }
            else if (model.Data is not null)
            {
                patch.Traces.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
            }
        }
        foreach (var model in diffs.Regions)
        {
            if (mf.Cache.Regions.ContainsKey(model.DiffKey))
            {
                if (model.Data == null)
                {
                    patch.Regions.Add(new PatchItem(new RegionDiff(mf.Cache.Regions[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                }
                else if (!mf.Cache.Regions[model.DiffKey].Equal(model.Data))
                {
                    patch.Regions.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                }
            }
            else if (model.Data is not null)
            {
                patch.Regions.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
            }
        }
        foreach (var model in diffs.Landmarks)
        {
            if (mf.Cache.Landmarks.ContainsKey(model.DiffKey))
            {
                if (model.Data == null)
                {
                    patch.Landmarks.Add(new PatchItem(new LandmarkDiff(mf.Cache.Landmarks[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                }
                else if (!mf.Cache.Landmarks[model.DiffKey].Equal(model.Data))
                {
                    patch.Landmarks.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                }
            }
            else if (model.Data is not null)
            {
                patch.Landmarks.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
            }
        }
        foreach (var model in diffs.Shortcuts)
        {
            if (mf.Cache.Shortcuts.ContainsKey(model.DiffKey))
            {
                if (model.Data == null)
                {
                    patch.Shortcuts.Add(new PatchItem(new ShortcutDiff(mf.Cache.Shortcuts[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                }
                else if (!mf.Cache.Shortcuts[model.DiffKey].Equal(model.Data))
                {
                    patch.Shortcuts.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                }
            }
            else if (model.Data is not null)
            {
                patch.Shortcuts.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
            }
        }
        foreach (var model in diffs.Variables)
        {
            if (mf.Cache.Variables.ContainsKey(model.DiffKey))
            {
                if (model.Data == null)
                {
                    patch.Variables.Add(new PatchItem(new VariableDiff(mf.Cache.Variables[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                }
                else if (!mf.Cache.Variables[model.DiffKey].Equal(model.Data))
                {
                    patch.Variables.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                }
            }
            else if (model.Data is not null)
            {
                patch.Variables.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
            }
        }
        foreach (var model in diffs.Snapshots)
        {
            if (mf.Cache.Snapshots.ContainsKey(model.DiffKey))
            {
                if (model.Data == null)
                {
                    patch.Snapshots.Add(new PatchItem(new SnapshotDiff(mf.Cache.Snapshots[model.DiffKey]), model, DiffMode.Removed, defaultSelected));
                }
                else if (!mf.Cache.Snapshots[model.DiffKey].Equal(model.Data))
                {
                    patch.Snapshots.Add(new PatchItem(model, model, DiffMode.Normal, defaultSelected));
                }
            }
            else if (model.Data is not null)
            {
                patch.Snapshots.Add(new PatchItem(model, model, DiffMode.New, defaultSelected));
            }
        }
        return patch;
    }

    //将差异应用到源地图文件上
    public static void Apply(Diffs diffs, MapFile source)
    {
        diffs.Rooms.ForEach(d => ApplyDiff(d, source));
        diffs.Markers.ForEach(d => ApplyDiff(d, source));
        diffs.Routes.ForEach(d => ApplyDiff(d, source));
        diffs.Traces.ForEach(d => ApplyDiff(d, source));
        diffs.Regions.ForEach(d => ApplyDiff(d, source));
        diffs.Landmarks.ForEach(d => ApplyDiff(d, source));
        diffs.Shortcuts.ForEach(d => ApplyDiff(d, source));
        diffs.Variables.ForEach(d => ApplyDiff(d, source));
        diffs.Snapshots.ForEach(d => ApplyDiff(d, source));
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