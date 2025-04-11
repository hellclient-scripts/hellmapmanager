using System.Collections.Generic;
using System.IO;
using System.Text;
using HellMapManager.Models;
using HellMapManager.Utils.Formatter;
namespace HellMapManager.Services.HMMEncoder;
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
        return HMMFormatter.EncodeKeyValue1(CurrentFormat, HMMFormatter.Escape(EncodeEncoding(Encoding)));
    }
    public static MapHeadData Decode(string val)
    {
        var kv = HMMFormatter.DecodeKeyValue1(val);
        return new MapHeadData
        {
            FileFormat = kv.Key,
            Encoding = DecodeEncoding(kv.UnescapeValue()),
        };
    }
}
public class HMMEncoder
{
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

        mf.Map.Rooms.ForEach(d => { results.Add(d.Encode()); });
        mf.Map.Aliases.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Landmarks.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Variables.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Routes.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Regions.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Traces.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Shortcuts.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Snapshots.ForEach(r => { results.Add(r.Encode()); });
        mf.Map.Querys.ForEach(r => { results.Add(r.Encode()); });

        return GetEncoding(head.Encoding).GetBytes(string.Join("\n", results));
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
        using (var ms = new MemoryStream(body))
        {
            using (var sr = new StreamReader(ms))
            {
                var line = sr.ReadLine();
                if (line is not null)
                {
                    var head = MapHeadData.Decode(line);
                    if (head.Validated())
                    {
                        encoding = GetEncoding(head.Encoding);
                        return DecodeWithEncoding(body, encoding);
                    }
                }
            }

        }
        return null;
    }
    private static MapFile? DecodeWithEncoding(byte[] body, Encoding encoding)
    {

        using (var ms = new MemoryStream(body))
        {
            var mf = MapFile.Empty("", "");
            using (var sr = new StreamReader(ms, encoding))
            {
                string? data;
                while ((data = sr.ReadLine()) != null)
                {
                    var key = HMMFormatter.DecodeKeyValue1(data);
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
                                if (model.Validated()) { mf.Map.Rooms.Add(model); }
                            }
                            break;
                        case Alias.EncodeKey:
                            {
                                var model = Alias.Decode(data);
                                if (model.Validated()) { mf.Map.Aliases.Add(model); }
                            }
                            break;
                        case Landmark.EncodeKey:
                            {
                                var model = Landmark.Decode(data);
                                if (model.Validated()) { mf.Map.Landmarks.Add(model); }
                            }
                            break;
                        case Variable.EncodeKey:
                            {
                                var model = Variable.Decode(data);
                                if (model.Validated()) { mf.Map.Variables.Add(model); }
                            }
                            break;
                        case Route.EncodeKey:
                            {
                                var model = Route.Decode(data);
                                if (model.Validated()) { mf.Map.Routes.Add(model); }
                            }
                            break;
                        case Region.EncodeKey:
                            {
                                var model = Region.Decode(data);
                                if (model.Validated()) { mf.Map.Regions.Add(model); }
                            }
                            break;
                        case Trace.EncodeKey:
                            {
                                var model = Trace.Decode(data);
                                if (model.Validated()) { mf.Map.Traces.Add(model); }
                            }
                            break;
                        case Shortcut.EncodeKey:
                            {
                                var model = Shortcut.Decode(data);
                                if (model.Validated()) { mf.Map.Shortcuts.Add(model); }
                            }
                            break;
                        case Snapshot.EncodeKey:
                            {
                                var model = Snapshot.Decode(data);
                                if (model.Validated()) { mf.Map.Snapshots.Add(model); }
                            }
                            break;
                        case Query.EncodeKey:
                            {
                                var model = Query.Decode(data);
                                if (model.Validated()) { mf.Map.Querys.Add(model); }
                            }
                            break;
                    }
                }
                return mf;
            }
        }
    }
}