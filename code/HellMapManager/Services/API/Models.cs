using System.Collections.Generic;
using HellMapManager.Cores;
using System.Text.Json.Serialization;
using HellMapManager.Models;

namespace HellMapManager.Services.API;

public class KeyList
{
    public static KeyList? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(KeyList), APIJsonSerializerContext.Default) is KeyList list)
            {
                return list;
            }
            return null;
        }
        catch
        {
            return null;
        }

    }
    public List<string> Keys { get; set; } = [];

}
public class InputListOption
{
    public static InputListOption From(APIListOption option)
    {
        return new InputListOption()
        {
            Keys = option.Keys(),
            Groups = option.Groups(),
        };
    }
    public APIListOption To()
    {
        return new APIListOption().WithKeys(Keys ?? []).WithGroups(Groups ?? []);
    }
    public static InputListOption? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputListOption), APIJsonSerializerContext.Default) is InputListOption option)
            {
                return option;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
    public List<string>? Keys { get; set; } = [];
    public List<string>? Groups { get; set; } = [];
}
public class APIResultInfo
{
    public static APIResultInfo? From(MapInfo? info)
    {
        if (info == null)
        {
            return null;
        }
        return new APIResultInfo()
        {
            Name = info.Name,
            Desc = info.Desc,
        };
    }
    public string Name { get; set; } = "";
    public string Desc { get; set; } = "";
}

public class ValueTagModel
{
    public static ValueTagModel From(ValueTag tag)
    {
        return new ValueTagModel()
        {
            Key = tag.Key,
            Value = tag.Value,
        };
    }
    public static List<ValueTagModel> FromList(List<ValueTag> tags)
    {
        var list = new List<ValueTagModel>();
        foreach (var tag in tags)
        {
            list.Add(From(tag));
        }
        return list;
    }
    public ValueTag ToValueTag()
    {
        return new ValueTag(Key, Value);
    }
    public static List<ValueTag> ToValueTagList(List<ValueTagModel> tagModels)
    {
        var list = new List<ValueTag>();
        foreach (var tagModel in tagModels)
        {
            list.Add(tagModel.ToValueTag());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public int Value { get; set; } = 0;

}
public class DataModel
{
    public static DataModel From(Data data)
    {
        return new DataModel()
        {
            Key = data.Key,
            Value = data.Value,
        };
    }
    public Data ToData()
    {
        return new Data(Key, Value);
    }
    public static List<DataModel> FromList(List<Data> datas)
    {
        var list = new List<DataModel>();
        foreach (var data in datas)
        {
            list.Add(From(data));
        }
        return list;
    }
    public static List<Data> ToDataList(List<DataModel> dataModels)
    {
        var list = new List<Data>();
        foreach (var dataModel in dataModels)
        {
            list.Add(dataModel.ToData());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";

}

public class ExitModel
{
    public static ExitModel From(Exit exit)
    {
        return new ExitModel()
        {
            Command = exit.Command,
            To = exit.To,
            Conditions = ValueConditionModel.FromList(exit.Conditions),
            Cost = exit.Cost,
        };
    }
    public Exit ToExit()
    {
        return new Exit()
        {
            Command = Command,
            To = To,
            Conditions = ValueConditionModel.ToValueConditionList(Conditions),
            Cost = Cost,
        };
    }
    public static List<ExitModel> FromList(List<Exit> exits)
    {
        var list = new List<ExitModel>();
        foreach (var exit in exits)
        {
            list.Add(From(exit));
        }
        return list;
    }
    public static List<Exit> ToExitList(List<ExitModel> exitModels)
    {
        var list = new List<Exit>();
        foreach (var exitModel in exitModels)
        {
            list.Add(exitModel.ToExit());
        }
        return list;
    }
    public string Command { get; set; } = "";
    public string To { get; set; } = "";
    public List<ValueConditionModel> Conditions { get; set; } = [];
    public int Cost { get; set; } = 1;

}
public class ValueConditionModel
{
    public static ValueConditionModel From(ValueCondition condition)
    {
        return new ValueConditionModel()
        {
            Key = condition.Key,
            Not = condition.Not,
            Value = condition.Value,
        };
    }
    public ValueCondition ToValueCondition()
    {
        return new ValueCondition(Key, Value, Not);
    }
    public static List<ValueConditionModel> FromList(List<ValueCondition> conditions)
    {
        var list = new List<ValueConditionModel>();
        foreach (var condition in conditions)
        {
            list.Add(From(condition));
        }
        return list;
    }
    public static List<ValueCondition> ToValueConditionList(List<ValueConditionModel> conditionModels)
    {
        var list = new List<ValueCondition>();
        foreach (var conditionModel in conditionModels)
        {
            list.Add(conditionModel.ToValueCondition());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public bool Not { get; set; } = false;
    public int Value { get; set; } = 0;
}
public class RoomModel
{
    public static RoomModel From(Room room)
    {
        var rm = new RoomModel()
        {
            Key = room.Key,
            Name = room.Name,
            Desc = room.Desc,
            Group = room.Group,
        };
        rm.Tags = ValueTagModel.FromList(room.Tags);
        rm.Exits = ExitModel.FromList(room.Exits);
        rm.Data = DataModel.FromList(room.Data);
        return rm;
    }
    public Room ToRoom()
    {
        var room = new Room()
        {
            Key = Key,
            Name = Name,
            Desc = Desc,
            Group = Group,
        };
        room.Tags = ValueTagModel.ToValueTagList(Tags);
        room.Exits = ExitModel.ToExitList(Exits);
        room.Data = DataModel.ToDataList(Data);
        return room;
    }
    public static List<RoomModel> FromList(List<Room> rooms)
    {
        var list = new List<RoomModel>();
        foreach (var room in rooms)
        {
            list.Add(From(room));
        }
        return list;
    }
    public static List<Room> ToRoomList(List<RoomModel> roomModels)
    {
        var list = new List<Room>();
        foreach (var roomModel in roomModels)
        {
            list.Add(roomModel.ToRoom());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public string Name { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Group { get; set; } = "";
    public List<ValueTagModel> Tags { get; set; } = [];
    public List<ExitModel> Exits { get; set; } = [];
    public List<DataModel> Data { get; set; } = [];

}
public class InputRooms()
{
    public static InputRooms? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputRooms), APIJsonSerializerContext.Default) is InputRooms rooms)
            {
                return rooms;
            }
        }
        catch
        {
        }
        return null;
    }
    public List<RoomModel> Rooms { get; set; } = [];
}

public class MarkerModel()
{
    public static MarkerModel From(Marker marker)
    {
        return new MarkerModel()
        {
            Key = marker.Key,
            Value = marker.Value,
            Group = marker.Group,
            Desc = marker.Desc,
            Message = marker.Message,
        };

    }
    public Marker ToMarker()
    {
        return new Marker()
        {
            Key = Key,
            Value = Value,
            Group = Group,
            Desc = Desc,
            Message = Message,
        };
    }
    public static List<MarkerModel> FromList(List<Marker> markers)
    {
        var list = new List<MarkerModel>();
        foreach (var marker in markers)
        {
            list.Add(From(marker));
        }
        return list;
    }
    public static List<Marker> ToMarkerList(List<MarkerModel> markerModels)
    {
        var list = new List<Marker>();
        foreach (var markerModel in markerModels)
        {
            list.Add(markerModel.ToMarker());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Message { get; set; } = "";
}
public class InputMarkers()
{
    public static InputMarkers? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputMarkers), APIJsonSerializerContext.Default) is InputMarkers markers)
            {
                return markers;
            }
        }
        catch
        {
        }
        return null;
    }
    public List<MarkerModel> Markers { get; set; } = [];
}

public class RouteModel
{
    public static RouteModel From(Route route)
    {
        return new RouteModel()
        {
            Key = route.Key,
            Desc = route.Desc,
            Group = route.Group,
            Message = route.Message,
            Rooms = route.Rooms,
        };
    }
    public Route ToRoute()
    {
        return new Route()
        {
            Key = Key,
            Desc = Desc,
            Group = Group,
            Message = Message,
            Rooms = Rooms,
        };
    }
    public static List<RouteModel> FromList(List<Route> routes)
    {
        var list = new List<RouteModel>();
        foreach (var route in routes)
        {
            list.Add(From(route));
        }
        return list;
    }
    public static List<Route> ToRouteList(List<RouteModel> routeModels)
    {
        var list = new List<Route>();
        foreach (var routeModel in routeModels)
        {
            list.Add(routeModel.ToRoute());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Group { get; set; } = "";
    public string Message { get; set; } = "";
    public List<string> Rooms { get; set; } = [];
}
public class InputRoutes()
{
    public static InputRoutes? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputRoutes), APIJsonSerializerContext.Default) is InputRoutes routes)
            {
                return routes;
            }
        }
        catch
        {
        }
        return null;
    }
    public List<RouteModel> Routes { get; set; } = [];
}

public class TraceModel()
{
    public static TraceModel From(Trace trace)
    {
        return new TraceModel()
        {
            Key = trace.Key,
            Group = trace.Group,
            Desc = trace.Desc,
            Message = trace.Message,
            Locations = [.. trace.Locations],
        };
    }
    public Trace ToTrace()
    {
        return new Trace()
        {
            Key = Key,
            Group = Group,
            Desc = Desc,
            Message = Message,
            Locations = [.. Locations],
        };
    }
    public static List<TraceModel> FromList(List<Trace> traces)
    {
        var list = new List<TraceModel>();
        foreach (var trace in traces)
        {
            list.Add(From(trace));
        }
        return list;
    }
    public static List<Trace> ToTraceList(List<TraceModel> traceModels)
    {
        var list = new List<Trace>();
        foreach (var traceModel in traceModels)
        {
            list.Add(traceModel.ToTrace());
        }
        return list;
    }

    public string Key { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Message { get; set; } = "";
    public List<string> Locations { get; set; } = [];
}
public class InputTraces()
{
    public static InputTraces? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputTraces), APIJsonSerializerContext.Default) is InputTraces traces)
            {
                return traces;
            }
        }
        catch
        {
        }
        return null;
    }
    public List<TraceModel> Traces { get; set; } = [];
}

public class RegionItemModel
{
    public static RegionItemModel From(RegionItem item)
    {
        return new RegionItemModel()
        {
            Not = item.Not,
            Type = item.Type == RegionItemType.Room ? "Room" : "Zone",
            Value = item.Value,
        };
    }
    public RegionItem ToRegionItem()
    {
        return new RegionItem(Type == "Room" ? RegionItemType.Room : RegionItemType.Zone, Value, Not);
    }
    public static List<RegionItemModel> FromList(List<RegionItem> items)
    {
        var list = new List<RegionItemModel>();
        foreach (var item in items)
        {
            list.Add(From(item));
        }
        return list;
    }
    public static List<RegionItem> ToRegionItemList(List<RegionItemModel> itemModels)
    {
        var list = new List<RegionItem>();
        foreach (var itemModel in itemModels)
        {
            list.Add(itemModel.ToRegionItem());
        }
        return list;
    }
    public bool Not { get; set; } = false;
    public string Type { get; set; } = "";
    public string Value { get; set; } = "";
}

public class RegionModel()
{
    public static RegionModel From(Region region)
    {
        return new RegionModel()
        {
            Key = region.Key,
            Group = region.Group,
            Desc = region.Desc,
            Message = region.Message,
            Items = RegionItemModel.FromList(region.Items),
        };
    }
    public Region ToRegion()
    {
        return new Region()
        {
            Key = Key,
            Group = Group,
            Desc = Desc,
            Message = Message,
            Items = RegionItemModel.ToRegionItemList(Items),
        };
    }
    public static List<RegionModel> FromList(List<Region> regions)
    {
        var list = new List<RegionModel>();
        foreach (var region in regions)
        {
            list.Add(From(region));
        }
        return list;
    }
    public static List<Region> ToRegionList(List<RegionModel> regionModels)
    {
        var list = new List<Region>();
        foreach (var regionModel in regionModels)
        {
            list.Add(regionModel.ToRegion());
        }
        return list;
    }
    public string Key { get; set; } = "";

    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

    public string Message { get; set; } = "";
    public List<RegionItemModel> Items { get; set; } = [];
}

public class InputRegions()
{
    public static InputRegions? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputRegions), APIJsonSerializerContext.Default) is InputRegions regions)
            {
                return regions;
            }
        }
        catch
        {
        }
        return null;
    }
    public List<RegionModel> Regions { get; set; } = [];
}
public class ShortcutModel()
{
    public static ShortcutModel From(Shortcut shortcut)
    {
        return new ShortcutModel()
        {
            Key = shortcut.Key,
            Command = shortcut.Command,
            To = shortcut.To,
            RoomConditions = ValueConditionModel.FromList(shortcut.RoomConditions),
            Conditions = ValueConditionModel.FromList(shortcut.Conditions),
            Cost = shortcut.Cost,
            Group = shortcut.Group,
            Desc = shortcut.Desc,
        };
    }
    public Shortcut ToShortcut()
    {
        return new Shortcut()
        {
            Key = Key,
            Command = Command,
            To = To,
            RoomConditions = ValueConditionModel.ToValueConditionList(RoomConditions),
            Conditions = ValueConditionModel.ToValueConditionList(Conditions),
            Cost = Cost,
            Group = Group,
            Desc = Desc,
        };
    }
    public static List<ShortcutModel> FromList(List<Shortcut> shortcuts)
    {
        var list = new List<ShortcutModel>();
        foreach (var shortcut in shortcuts)
        {
            list.Add(From(shortcut));
        }
        return list;
    }
    public static List<Shortcut> ToShortcutList(List<ShortcutModel> shortcutModels)
    {
        var list = new List<Shortcut>();
        foreach (var shortcutModel in shortcutModels)
        {
            list.Add(shortcutModel.ToShortcut());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public string Command { get; set; } = "";
    public string To { get; set; } = "";
    public List<ValueConditionModel> RoomConditions { get; set; } = [];
    public List<ValueConditionModel> Conditions { get; set; } = [];
    public int Cost { get; set; } = 0;
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
}

public class InputShortcuts()
{
    public static InputShortcuts? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputShortcuts), APIJsonSerializerContext.Default) is InputShortcuts shortcuts)
            {
                return shortcuts;
            }
        }
        catch
        {
        }
        return null;
    }
    public List<ShortcutModel> Shortcuts { get; set; } = [];
}

[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(APIResultInfo))]
[JsonSerializable(typeof(InputListOption))]
[JsonSerializable(typeof(ValueTagModel))]
[JsonSerializable(typeof(DataModel))]
[JsonSerializable(typeof(List<DataModel>))]
[JsonSerializable(typeof(ValueConditionModel))]
[JsonSerializable(typeof(List<ValueConditionModel>))]
[JsonSerializable(typeof(ExitModel))]
[JsonSerializable(typeof(List<ExitModel>))]
[JsonSerializable(typeof(KeyList))]
[JsonSerializable(typeof(RoomModel))]
[JsonSerializable(typeof(RegionItemModel))]
[JsonSerializable(typeof(List<RegionItemModel>))]
[JsonSerializable(typeof(List<RoomModel>))]
[JsonSerializable(typeof(InputRooms))]
[JsonSerializable(typeof(MarkerModel))]
[JsonSerializable(typeof(List<MarkerModel>))]
[JsonSerializable(typeof(InputMarkers))]
[JsonSerializable(typeof(RouteModel))]
[JsonSerializable(typeof(List<RouteModel>))]
[JsonSerializable(typeof(InputRoutes))]
[JsonSerializable(typeof(TraceModel))]
[JsonSerializable(typeof(List<TraceModel>))]
[JsonSerializable(typeof(InputTraces))]
[JsonSerializable(typeof(RegionModel))]
[JsonSerializable(typeof(List<RegionModel>))]
[JsonSerializable(typeof(InputRegions))]
[JsonSerializable(typeof(ShortcutModel))]
[JsonSerializable(typeof(List<ShortcutModel>))]
[JsonSerializable(typeof(InputShortcuts))]


public partial class APIJsonSerializerContext : JsonSerializerContext { }
