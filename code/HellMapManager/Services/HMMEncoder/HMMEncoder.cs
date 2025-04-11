using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia.Input;
using HellMapManager.Models;
using HellMapManager.Utils.Formatter;
namespace HellMapManager.Services.HMMEncoder;

public class HMMEncoder
{
    public static byte[] Encode(MapFile mf)
    {
        var results = new List<string> { };
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

        return Encoding.UTF8.GetBytes(string.Join("\n", results));
    }
    public static MapFile? Decode(byte[] body)
    {
        using (var ms = new MemoryStream(body))
        {
            var mf = MapFile.Empty("", "");
            using (var sr = new StreamReader(ms))
            {
                string? data;
                while ((data = sr.ReadLine()) != null)
                {
                    var key = HMMFormatter.DecodeKeyValue1(data);
                    switch (key.Key)
                    {
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