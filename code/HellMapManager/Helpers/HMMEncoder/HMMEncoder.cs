using System.Collections.Generic;
using System.IO;
using System.Text;
using HellMapManager.Models;

namespace HellMapManager.Helpers.HMMEncoder;

public class DefaultHmmEncoderHooks
{
    public static Room? RoomHook(Room model)
    {
        return model;
    }
    public static Shortcut? ShortcutHook(Shortcut model)
    {
        return model;
    }
}

public delegate Room? RoomHook(Room model);
public delegate Shortcut? ShortcutHook(Shortcut model);

public class MapHeadData
{
    public const string CurrentFormat = "HMM1.0";

    public string FileFormat = "";
    public MapEncoding Encoding { get; set; } = MapEncoding.Default;
    public bool Validated()
    {
        return FileFormat == CurrentFormat;
    }
    public static string EncodeEncoding(MapEncoding e)
    {
        return e == MapEncoding.GB18030 ? "GB18030" : "UTF8";
    }
    public static MapEncoding DecodeEncoding(string val)
    {
        return val == "GB18030" ? MapEncoding.GB18030 : MapEncoding.Default;
    }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, CurrentFormat, HMMFormatter.Escape(EncodeEncoding(Encoding)));
    }
    public static MapHeadData Decode(string val)
    {
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, val);
        return new MapHeadData
        {
            FileFormat = kv.Key,
            Encoding = DecodeEncoding(kv.UnescapeValue()),
        };
    }
}
public class HMMEncoder
{
    public static RoomHook DecodeRoomHook { get; set; } = DefaultHmmEncoderHooks.RoomHook;
    public static RoomHook EncodeRoomHook { get; set; } = DefaultHmmEncoderHooks.RoomHook;
    public static ShortcutHook DecodeShortcutHook { get; set; } = DefaultHmmEncoderHooks.ShortcutHook;
    public static ShortcutHook EncodeShortcutHook { get; set; } = DefaultHmmEncoderHooks.ShortcutHook;
    public static void ResetHooks()
    {
        DecodeRoomHook = DefaultHmmEncoderHooks.RoomHook;
        EncodeRoomHook = DefaultHmmEncoderHooks.RoomHook;
        DecodeShortcutHook = DefaultHmmEncoderHooks.ShortcutHook;
        EncodeShortcutHook = DefaultHmmEncoderHooks.ShortcutHook;
    }

    public static byte[] Encode(MapFile mf)
    {
        var head = new MapHeadData
        {
            Encoding = mf.Map.Encoding,
        };
        var results = new List<string> {
            head.Encode(),
            mf.Map.Info.Encode(),
        };
        mf.Map.Arrange();
        mf.Map.Rooms.ForEach(d =>
        {
            if (EncodeRoomHook(d) is Room room)
            {
                results.Add(room.Encode());
            }
        });
        mf.Map.Markers.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Landmarks.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Variables.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Routes.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Regions.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Traces.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Shortcuts.ForEach(r =>
        {
            if (EncodeShortcutHook(r) is Shortcut shortcut)
            {
                results.Add(shortcut.Encode());
            }
        });
        mf.Map.Snapshots.ForEach(r => { results.Add(r.Encode()); });

        return GetEncoding(head.Encoding).GetBytes(HMMFormatter.Escaper.Pack(string.Join("\n", results)));
    }
    public static Encoding GetEncoding(MapEncoding me)
    {
        switch (me)
        {
            case MapEncoding.GB18030:
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                return Encoding.GetEncoding("GB18030");

            default:
                return Encoding.UTF8;
        }

    }
    public static MapFile? Decode(byte[] body)
    {
        Encoding encoding;
        using var ms = new MemoryStream(body);
        using var sr = new StreamReader(ms);
        var line = sr.ReadLine();
        if (line is not null)
        {
            line = HMMFormatter.Escaper.Unpack(line);
            var head = MapHeadData.Decode(line);
            if (head.Validated())
            {
                encoding = GetEncoding(head.Encoding);
                return DecodeWithEncoding(body, head.Encoding, encoding);
            }
        }
        return null;
    }
    private static MapFile? DecodeWithEncoding(byte[] body, MapEncoding mapEncoding, Encoding encoding)
    {

        using var ms = new MemoryStream(body);
        var mf = MapFile.Create("", "");
        mf.Map.Encoding = mapEncoding;
        using var sr = new StreamReader(ms, encoding);
        string? data;
        while ((data = sr.ReadLine()) != null)
        {
            data = HMMFormatter.Escaper.Unpack(data);
            var key = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, data);
            switch (key.Key)
            {
                case MapInfo.EncodeKey:
                    {
                        var model = MapInfo.Decode(data);
                        if (model.Validated()) { mf.Map.Info = model; }
                    }
                    break;
                case Room.EncodeKey:
                    {
                        var model = Room.Decode(data);
                        if (model.Validated())
                        {
                            if (DecodeRoomHook(model) is Room room)
                            {
                                mf.InsertRoom(room);
                            }
                        }
                    }
                    break;
                case Marker.EncodeKey:
                    {
                        var model = Marker.Decode(data);
                        if (model.Validated()) { mf.InsertMarker(model); }
                    }
                    break;
                case Landmark.EncodeKey:
                    {
                        var model = Landmark.Decode(data);
                        if (model.Validated()) { mf.InsertLandmark(model); }
                    }
                    break;
                case Variable.EncodeKey:
                    {
                        var model = Variable.Decode(data);
                        if (model.Validated()) { mf.InsertVariable(model); }
                    }
                    break;
                case Route.EncodeKey:
                    {
                        var model = Route.Decode(data);
                        if (model.Validated()) { mf.InsertRoute(model); }
                    }
                    break;
                case Region.EncodeKey:
                    {
                        var model = Region.Decode(data);
                        if (model.Validated()) { mf.InsertRegion(model); }
                    }
                    break;
                case Trace.EncodeKey:
                    {
                        var model = Trace.Decode(data);
                        if (model.Validated()) { mf.InsertTrace(model); }
                    }
                    break;
                case Shortcut.EncodeKey:
                    {
                        var model = Shortcut.Decode(data);
                        if (model.Validated())
                        {
                            if (DecodeShortcutHook(model) is Shortcut shortcut)
                            {
                                mf.InsertShortcut(shortcut);
                            }
                        }
                    }
                    break;
                case Snapshot.EncodeKey:
                    {
                        var model = Snapshot.Decode(data);
                        if (model.Validated()) { mf.InsertSnapshot(model); }
                    }
                    break;
            }
        }
        return mf;
    }
}