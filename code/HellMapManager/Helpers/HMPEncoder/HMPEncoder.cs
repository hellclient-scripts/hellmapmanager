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
        diffs.Items.ForEach(r => { results.Add(r.Encode()); });
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
                                if (room.Validated())
                                {
                                    result.Items.Add(new RoomDiff(room));
                                }
                                break;
                            }
                        case Marker.EncodeKey:
                            {
                                var marker = Marker.Decode(data);
                                if (marker.Validated())
                                {
                                    result.Items.Add(new MarkerDiff(marker));
                                }
                                break;
                            }
                        case Route.EncodeKey:
                            {
                                var route = Route.Decode(data);
                                if (route.Validated())
                                {
                                    result.Items.Add(new RouteDiff(route));
                                }
                                break;
                            }
                        case Trace.EncodeKey:
                            {
                                var trace = Trace.Decode(data);
                                if (trace.Validated())
                                {
                                    result.Items.Add(new TraceDiff(trace));
                                }
                                break;
                            }
                        case Region.EncodeKey:
                            {
                                var region = Region.Decode(data);
                                if (region.Validated())
                                {
                                    result.Items.Add(new RegionDiff(region));
                                }
                                break;
                            }
                        case Landmark.EncodeKey:
                            {
                                var landmark = Landmark.Decode(data);
                                if (landmark.Validated())
                                {
                                    result.Items.Add(new LandmarkDiff(landmark));
                                }
                                break;
                            }
                        case Shortcut.EncodeKey:
                            {
                                var shortcut = Shortcut.Decode(data);
                                if (shortcut.Validated())
                                {
                                    result.Items.Add(new ShortcutDiff(shortcut));
                                }
                                break;
                            }
                        case Variable.EncodeKey:
                            {
                                var variable = Variable.Decode(data);
                                if (variable.Validated())
                                {
                                    result.Items.Add(new VariableDiff(variable));
                                }
                                break;
                            }
                        case Snapshot.EncodeKey:
                            {
                                var snapshot = Snapshot.Decode(data);
                                if (snapshot.Validated())
                                {
                                    result.Items.Add(new SnapshotDiff(snapshot));
                                }
                                break;
                            }
                        case RemovedRoomDiff.EncodeKey:
                            {
                                var removedRoom = RemovedRoomDiff.Decode(data);
                                if (removedRoom.Validated())
                                {
                                    result.Items.Add(removedRoom);
                                }
                                break;
                            }
                        case RemovedMarkerDiff.EncodeKey:
                            {
                                var removedMarker = RemovedMarkerDiff.Decode(data);
                                if (removedMarker.Validated())
                                {
                                    result.Items.Add(removedMarker);
                                }
                                break;
                            }
                        case RemovedRouteDiff.EncodeKey:
                            {
                                var removedRoute = RemovedRouteDiff.Decode(data);
                                if (removedRoute.Validated())
                                {
                                    result.Items.Add(removedRoute);
                                }
                                break;
                            }
                        case RemovedTraceDiff.EncodeKey:
                            {
                                var removedTrace = RemovedTraceDiff.Decode(data);
                                if (removedTrace.Validated())
                                {
                                    result.Items.Add(removedTrace);
                                }
                                break;
                            }
                        case RemovedRegionDiff.EncodeKey:
                            {
                                var removedRegion = RemovedRegionDiff.Decode(data);
                                if (removedRegion.Validated())
                                {
                                    result.Items.Add(removedRegion);
                                }
                                break;
                            }
                        case RemovedLandmarkDiff.EncodeKey:
                            {
                                var removedLandmark = RemovedLandmarkDiff.Decode(data);
                                if (removedLandmark.Validated())
                                {
                                    result.Items.Add(removedLandmark);
                                }
                                break;
                            }
                        case RemovedShortcutDiff.EncodeKey:
                            {
                                var removedShortcut = RemovedShortcutDiff.Decode(data);
                                if (removedShortcut.Validated())
                                {
                                    result.Items.Add(removedShortcut);
                                }
                                break;
                            }
                        case RemovedVariableDiff.EncodeKey:
                            {
                                var removedVariable = RemovedVariableDiff.Decode(data);
                                if (removedVariable.Validated())
                                {
                                    result.Items.Add(removedVariable);
                                }
                                break;
                            }
                        case RemovedSnapshotDiff.EncodeKey:
                            {
                                var removedSnapshot = RemovedSnapshotDiff.Decode(data);
                                if (removedSnapshot.Validated())
                                {
                                    result.Items.Add(removedSnapshot);
                                }
                                break;
                            }
                    }
                }
                result.Arrange();
                return result;
            }
        }
        return null;
    }
}