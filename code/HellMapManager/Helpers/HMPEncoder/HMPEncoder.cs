using HellMapManager.Models;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace HellMapManager.Helpers.HMPEncoder;

public class MapPatchHeadData
{
    public const string CurrentFormat = "HMP1.0";

    public string FileFormat = "";
    public bool Validated()
    {
        return FileFormat == CurrentFormat;
    }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, CurrentFormat, "");
    }
    public static MapPatchHeadData Decode(string val)
    {
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, val);
        return new MapPatchHeadData
        {
            FileFormat = kv.Key,
        };
    }
}
public class HMPEncoder
{
    public static byte[] Encode(Diffs diffs)
    {
        var head = new MapPatchHeadData
        {
            FileFormat = MapPatchHeadData.CurrentFormat,
        };
        var results = new List<string> {
            head.Encode(),
        };
        diffs.Rooms.ForEach(r => { results.Add(r.Encode()); });
        diffs.Markers.ForEach(r => { results.Add(r.Encode()); });
        diffs.Routes.ForEach(r => { results.Add(r.Encode()); });
        diffs.Traces.ForEach(r => { results.Add(r.Encode()); });
        diffs.Regions.ForEach(r => { results.Add(r.Encode()); });
        diffs.Landmarks.ForEach(r => { results.Add(r.Encode()); });
        diffs.Shortcuts.ForEach(r => { results.Add(r.Encode()); });
        diffs.Variables.ForEach(r => { results.Add(r.Encode()); });
        diffs.Snapshots.ForEach(r => { results.Add(r.Encode()); });
        return Encoding.UTF8.GetBytes(HMMFormatter.Escaper.Pack(string.Join("\n", results)));
    }
    public static Diffs? Decode(byte[] body)
    {
        using var ms = new MemoryStream(body);
        var result = new Diffs();
        using var sr = new StreamReader(ms, Encoding.UTF8);
        string? data;
        var line = sr.ReadLine();
        if (line is not null)
        {
            line = HMMFormatter.Escaper.Unpack(line);
            var head = MapPatchHeadData.Decode(line);
            if (head.Validated())
            {
                while ((data = sr.ReadLine()) != null)
                {
                    data = HMMFormatter.Escaper.Unpack(data);
                    var key = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, data);
                    switch (key.Key)
                    {
                        case Room.EncodeKey:
                            {
                                var room = Room.Decode(data);
                                if (room is not null)
                                {
                                    result.Rooms.Add(new RoomDiff(room));
                                }
                                break;
                            }
                        case Marker.EncodeKey:
                            {
                                var marker = Marker.Decode(data);
                                if (marker is not null)
                                {
                                    result.Markers.Add(new MarkerDiff(marker));
                                }
                                break;
                            }
                        case Route.EncodeKey:
                            {
                                var route = Route.Decode(data);
                                if (route is not null)
                                {
                                    result.Routes.Add(new RouteDiff(route));
                                }
                                break;
                            }
                        case Trace.EncodeKey:
                            {
                                var trace = Trace.Decode(data);
                                if (trace is not null)
                                {
                                    result.Traces.Add(new TraceDiff(trace));
                                }
                                break;
                            }
                        case Region.EncodeKey:
                            {
                                var region = Region.Decode(data);
                                if (region is not null)
                                {
                                    result.Regions.Add(new RegionDiff(region));
                                }
                                break;
                            }
                        case Landmark.EncodeKey:
                            {
                                var landmark = Landmark.Decode(data);
                                if (landmark is not null)
                                {
                                    result.Landmarks.Add(new LandmarkDiff(landmark));
                                }
                                break;
                            }
                        case Shortcut.EncodeKey:
                            {
                                var shortcut = Shortcut.Decode(data);
                                if (shortcut is not null)
                                {
                                    result.Shortcuts.Add(new ShortcutDiff(shortcut));
                                }
                                break;
                            }
                        case Variable.EncodeKey:
                            {
                                var variable = Variable.Decode(data);
                                if (variable is not null)
                                {
                                    result.Variables.Add(new VariableDiff(variable));
                                }
                                break;
                            }
                        case Snapshot.EncodeKey:
                            {
                                var snapshot = Snapshot.Decode(data);
                                if (snapshot is not null)
                                {
                                    result.Snapshots.Add(new SnapshotDiff(snapshot));
                                }
                                break;
                            }
                        case RemovedRoomDiff.EncodeKey:
                            {
                                var removedRoom = RemovedRoomDiff.Decode(data);
                                if (removedRoom is not null)
                                {
                                    result.Rooms.Add(removedRoom);
                                }
                                break;
                            }
                        case RemovedMarkerDiff.EncodeKey:
                            {
                                var removedMarker = RemovedMarkerDiff.Decode(data);
                                if (removedMarker is not null)
                                {
                                    result.Markers.Add(removedMarker);
                                }
                                break;
                            }
                        case RemovedRouteDiff.EncodeKey:
                            {
                                var removedRoute = RemovedRouteDiff.Decode(data);
                                if (removedRoute is not null)
                                {
                                    result.Routes.Add(removedRoute);
                                }
                                break;
                            }
                        case RemovedTraceDiff.EncodeKey:
                            {
                                var removedTrace = RemovedTraceDiff.Decode(data);
                                if (removedTrace is not null)
                                {
                                    result.Traces.Add(removedTrace);
                                }
                                break;
                            }
                        case RemovedRegionDiff.EncodeKey:
                            {
                                var removedRegion = RemovedRegionDiff.Decode(data);
                                if (removedRegion is not null)
                                {
                                    result.Regions.Add(removedRegion);
                                }
                                break;
                            }
                        case RemovedLandmarkDiff.EncodeKey:
                            {
                                var removedLandmark = RemovedLandmarkDiff.Decode(data);
                                if (removedLandmark is not null)
                                {
                                    result.Landmarks.Add(removedLandmark);
                                }
                                break;
                            }
                        case RemovedShortcutDiff.EncodeKey:
                            {
                                var removedShortcut = RemovedShortcutDiff.Decode(data);
                                if (removedShortcut is not null)
                                {
                                    result.Shortcuts.Add(removedShortcut);
                                }
                                break;
                            }
                        case RemovedVariableDiff.EncodeKey:
                            {
                                var removedVariable = RemovedVariableDiff.Decode(data);
                                if (removedVariable is not null)
                                {
                                    result.Variables.Add(removedVariable);
                                }
                                break;
                            }
                        case RemovedSnapshotDiff.EncodeKey:
                            {
                                var removedSnapshot = RemovedSnapshotDiff.Decode(data);
                                if (removedSnapshot is not null)
                                {
                                    result.Snapshots.Add(removedSnapshot);
                                }
                                break;
                            }
                    }
                }
                return result;
            }
        }
        return null;
    }
}