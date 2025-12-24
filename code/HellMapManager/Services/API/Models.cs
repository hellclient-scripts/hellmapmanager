using System.Collections.Generic;
using HellMapManager.Cores;
using System.Text.Json.Serialization;
using HellMapManager.Models;
using System;
using System.Linq;

namespace HellMapManager.Services.API;

public class KeyTypeValue()
{
    public static KeyTypeValue FromSnapshotKey(SnapshotKey key)
    {
        return new KeyTypeValue()
        {
            Key = key.Key,
            Type = key.Type,
            Value = key.Value,
        };
    }
    public SnapshotKey ToSnapshotKey()
    {
        return new SnapshotKey(Key, Type, Value);
    }
    public static List<KeyTypeValue> FromSnapshotKeyList(List<SnapshotKey> keys)
    {
        var list = new List<KeyTypeValue>();
        foreach (var key in keys.Where(x => x is not null))
        {
            list.Add(FromSnapshotKey(key));
        }
        return list;
    }
    public static List<SnapshotKey> ToSnapshotKeyList(List<KeyTypeValue> keyTypes)
    {
        var list = new List<SnapshotKey>();
        foreach (var keyType in keyTypes.Where(x => x is not null))
        {
            list.Add(keyType.ToSnapshotKey());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public string Type { get; set; } = "";
    public string Value { get; set; } = "";
}

public class SnapshotKeyList()
{
    public static SnapshotKeyList? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(SnapshotKeyList), APIJsonSerializerContext.Default) is SnapshotKeyList list)
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
    public List<KeyTypeValue> Keys { get; set; } = [];
}
public class KeyType
{
    public static KeyType FromLandmarkKey(LandmarkKey key)
    {
        return new KeyType()
        {
            Key = key.Key,
            Type = key.Type,
        };
    }
    public LandmarkKey ToLandmarkKey()
    {
        return new LandmarkKey(Key, Type);
    }
    public static List<KeyType> FromLandmarkKeyList(List<LandmarkKey> keys)
    {
        var list = new List<KeyType>();
        foreach (var key in keys.Where(x => x is not null))
        {
            list.Add(FromLandmarkKey(key));
        }
        return list;
    }
    public static List<LandmarkKey> ToLandmarkKeyList(List<KeyType> keyTypes)
    {
        var list = new List<LandmarkKey>();
        foreach (var keyType in keyTypes.Where(x => x is not null))
        {
            list.Add(keyType.ToLandmarkKey());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public string Type { get; set; } = "";
}

public class LandmarkKeyList()
{
    public static LandmarkKeyList? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(LandmarkKeyList), APIJsonSerializerContext.Default) is LandmarkKeyList list)
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
    public List<KeyType> LandmarkKeys { get; set; } = [];
}
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
        foreach (var tag in tags.Where(x => x is not null))
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
        foreach (var tagModel in tagModels.Where(x => x is not null))
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
        foreach (var data in datas.Where(x => x is not null))
        {
            list.Add(From(data));
        }
        return list;
    }
    public static List<Data> ToDataList(List<DataModel> dataModels)
    {
        var list = new List<Data>();
        foreach (var dataModel in dataModels.Where(x => x is not null))
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
        foreach (var exit in exits.Where(x => x is not null))
        {
            list.Add(From(exit));
        }
        return list;
    }
    public static List<Exit> ToExitList(List<ExitModel> exitModels)
    {
        var list = new List<Exit>();
        foreach (var exitModel in exitModels.Where(x => x is not null))
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
        foreach (var condition in conditions.Where(x => x is not null))
        {
            list.Add(From(condition));
        }
        return list;
    }
    public static List<ValueCondition> ToValueConditionList(List<ValueConditionModel> conditionModels)
    {
        var list = new List<ValueCondition>();
        foreach (var conditionModel in conditionModels.Where(x => x is not null))
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
    public static RoomModel? From(Room? room)
    {
        if (room is null)
        {
            return null;
        }
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
        foreach (var room in rooms.Where(x => x is not null))
        {
            list.Add(From(room)!);
        }
        return list;
    }
    public static List<Room> ToRoomList(List<RoomModel> roomModels)
    {
        var list = new List<Room>();
        foreach (var roomModel in roomModels.Where(x => x is not null))
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
        foreach (var markerModel in markerModels.Where(x => x is not null))
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
        foreach (var route in routes.Where(x => x is not null))
        {
            list.Add(From(route));
        }
        return list;
    }
    public static List<Route> ToRouteList(List<RouteModel> routeModels)
    {
        var list = new List<Route>();
        foreach (var routeModel in routeModels.Where(x => x is not null))
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
        foreach (var trace in traces.Where(x => x is not null))
        {
            list.Add(From(trace));
        }
        return list;
    }
    public static List<Trace> ToTraceList(List<TraceModel> traceModels)
    {
        var list = new List<Trace>();
        foreach (var traceModel in traceModels.Where(x => x is not null))
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
        foreach (var item in items.Where(x => x is not null))
        {
            list.Add(From(item));
        }
        return list;
    }
    public static List<RegionItem> ToRegionItemList(List<RegionItemModel> itemModels)
    {
        var list = new List<RegionItem>();
        foreach (var itemModel in itemModels.Where(x => x is not null))
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
        foreach (var regionModel in regionModels.Where(x => x is not null))
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
        foreach (var shortcut in shortcuts.Where(x => x is not null))
        {
            list.Add(From(shortcut));
        }
        return list;
    }
    public static List<Shortcut> ToShortcutList(List<ShortcutModel> shortcutModels)
    {
        var list = new List<Shortcut>();
        foreach (var shortcutModel in shortcutModels.Where(x => x is not null))
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

public class VariableModel()
{
    public static VariableModel From(Variable variable)
    {
        return new VariableModel()
        {
            Key = variable.Key,
            Value = variable.Value,
            Group = variable.Group,
            Desc = variable.Desc,
        };
    }
    public Variable ToVariable()
    {
        return new Variable()
        {
            Key = Key,
            Value = Value,
            Group = Group,
            Desc = Desc,
        };
    }
    public static List<VariableModel> FromList(List<Variable> variables)
    {
        var list = new List<VariableModel>();
        foreach (var variable in variables.Where(x => x is not null))
        {
            list.Add(From(variable));
        }
        return list;
    }
    public static List<Variable> ToVariableList(List<VariableModel> variableModels)
    {
        var list = new List<Variable>();
        foreach (var variableModel in variableModels.Where(x => x is not null))
        {
            list.Add(variableModel.ToVariable());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

}

public class InputVariables()
{
    public static InputVariables? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputVariables), APIJsonSerializerContext.Default) is InputVariables variables)
            {
                return variables;
            }
        }
        catch
        {
        }
        return null;
    }
    public List<VariableModel> Variables { get; set; } = [];
}

public class LandmarkModel()
{
    public static LandmarkModel From(Landmark landmark)
    {
        return new LandmarkModel()
        {
            Key = landmark.Key,
            Type = landmark.Type,
            Value = landmark.Value,
            Group = landmark.Group,
            Desc = landmark.Desc,
        };
    }
    public Landmark ToLandmark()
    {
        return new Landmark()
        {
            Key = Key,
            Type = Type,
            Value = Value,
            Group = Group,
            Desc = Desc,
        };
    }
    public static List<LandmarkModel> FromList(List<Landmark> landmarks)
    {
        var list = new List<LandmarkModel>();
        foreach (var landmark in landmarks.Where(x => x is not null))
        {
            list.Add(From(landmark));
        }
        return list;
    }
    public static List<Landmark> ToLandmarkList(List<LandmarkModel> landmarkModels)
    {
        var list = new List<Landmark>();
        foreach (var landmarkModel in landmarkModels.Where(x => x is not null))
        {
            list.Add(landmarkModel.ToLandmark());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public string Type { get; set; } = "";
    public string Value { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

}

public class InputLandmarks()
{
    public static InputLandmarks? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputLandmarks), APIJsonSerializerContext.Default) is InputLandmarks landmarks)
            {
                return landmarks;
            }
        }
        catch
        {
        }
        return null;
    }
    public List<LandmarkModel> Landmarks { get; set; } = [];
}
public class SnapshotModel()
{
    public static SnapshotModel From(Snapshot snapshot)
    {
        return new SnapshotModel()
        {
            Key = snapshot.Key,
            Timestamp = snapshot.Timestamp,
            Group = snapshot.Group,
            Type = snapshot.Type,
            Count = snapshot.Count,
            Value = snapshot.Value,
        };
    }
    public Snapshot ToSnapshot()
    {
        return new Snapshot()
        {
            Key = Key,
            Timestamp = Timestamp,
            Group = Group,
            Type = Type,
            Count = Count,
            Value = Value,
        };
    }
    public static List<SnapshotModel> FromList(List<Snapshot> snapshots)
    {
        var list = new List<SnapshotModel>();
        foreach (var snapshot in snapshots.Where(x => x is not null))
        {
            list.Add(From(snapshot));
        }
        return list;
    }
    public static List<Snapshot> ToSnapshotList(List<SnapshotModel> snapshotModels)
    {
        var list = new List<Snapshot>();
        foreach (var snapshotModel in snapshotModels.Where(x => x is not null))
        {
            list.Add(snapshotModel.ToSnapshot());
        }
        return list;
    }
    public string Key { get; set; } = "";
    public int Timestamp { get; set; } = 0;
    public string Group { get; set; } = "";
    public string Type { get; set; } = "";
    public int Count { get; set; } = 1;
    public string Value { get; set; } = "";
}

public class InputSnapshots()
{
    public static InputSnapshots? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputSnapshots), APIJsonSerializerContext.Default) is InputSnapshots snapshots)
            {
                return snapshots;
            }
        }
        catch
        {
        }
        return null;
    }
    public List<SnapshotModel> Snapshots { get; set; } = [];
}


public class StepModel()
{
    public string Command { get; set; } = "";
    public string Target { get; set; } = "";
    public int Cost { get; set; } = 0;
    public static StepModel From(Step step)
    {
        return new StepModel()
        {
            Command = step.Command,
            Target = step.Target,
            Cost = step.Cost,
        };
    }
    public Step ToStep()
    {
        return new Step(Command, Target, Cost);
    }
    public static List<StepModel> FromList(List<Step> steps)
    {
        var list = new List<StepModel>();
        foreach (var step in steps.Where(x => x is not null))
        {
            list.Add(From(step));
        }
        return list;
    }
    public static List<Step> ToStepList(List<StepModel> stepModels)
    {
        var list = new List<Step>();
        foreach (var stepModel in stepModels.Where(x => x is not null))
        {
            list.Add(stepModel.ToStep());
        }
        return list;
    }
}

public class QueryResultModel()
{
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public int Cost { get; set; } = 0;
    public List<StepModel> Steps { get; set; } = [];
    public List<string> Unvisited { get; set; } = [];
    public static QueryResultModel? FromQueryResult(QueryResult? result)
    {
        if (result == null)
        {
            return null;
        }
        return new QueryResultModel()
        {
            From = result.From,
            To = result.To,
            Cost = result.Cost,
            Steps = StepModel.FromList(result.Steps),
            Unvisited = [.. result.Unvisited],
        };
    }
    public QueryResult ToQueryResult()
    {
        return new QueryResult()
        {
            From = From,
            To = To,
            Cost = Cost,
            Steps = StepModel.ToStepList(Steps),
            Unvisited = [.. Unvisited],
        };
    }
}


public class PathModel()
{
    public static PathModel FromPath(Path path)
    {
        return new PathModel()
        {
            From = path.From,
            Command = path.Command,
            To = path.To,
            Conditions = ValueConditionModel.FromList(path.Conditions),
            Cost = path.Cost,
        };
    }
    public Path ToPath()
    {
        return new Path()
        {
            From = From,
            Command = Command,
            To = To,
            Conditions = ValueConditionModel.ToValueConditionList(Conditions),
            Cost = Cost,
        };
    }
    public static List<PathModel> FromPathList(List<Path> paths)
    {
        var list = new List<PathModel>();
        foreach (var path in paths.Where(x => x is not null))
        {
            list.Add(FromPath(path));
        }
        return list;
    }
    public static List<Path> ToPathList(List<PathModel> pathModels)
    {
        var list = new List<Path>();
        foreach (var pathModel in pathModels.Where(x => x is not null))
        {
            list.Add(pathModel.ToPath());
        }
        return list;
    }
    public string From { get; set; } = "";
    public string Command { get; set; } = "";
    public string To { get; set; } = "";
    public List<ValueConditionModel> Conditions { get; set; } = [];
    public int Cost { get; set; } = 1;

}
public class RoomConditionExitModel()
{
    public static RoomConditionExitModel From(RoomConditionExit exit)
    {
        return new RoomConditionExitModel()
        {
            RoomConditions = ValueConditionModel.FromList(exit.RoomConditions),
            Command = exit.Command,
            To = exit.To,
            Conditions = ValueConditionModel.FromList(exit.Conditions),
            Cost = exit.Cost,
        };
    }
    public RoomConditionExit ToRoomConditionExit()
    {
        return new RoomConditionExit()
        {
            RoomConditions = ValueConditionModel.ToValueConditionList(RoomConditions),
            Command = Command,
            To = To,
            Conditions = ValueConditionModel.ToValueConditionList(Conditions),
            Cost = Cost,
        };
    }
    public static List<RoomConditionExitModel> FromRoomConditionExitList(List<RoomConditionExit> exits)
    {
        var list = new List<RoomConditionExitModel>();
        foreach (var exit in exits.Where(x => x is not null))
        {
            list.Add(From(exit));
        }
        return list;
    }
    public static List<RoomConditionExit> ToRoomConditionExitList(List<RoomConditionExitModel> exitModels)
    {
        var list = new List<RoomConditionExit>();
        foreach (var exitModel in exitModels.Where(x => x is not null))
        {
            list.Add(exitModel.ToRoomConditionExit());
        }
        return list;
    }
    public List<ValueConditionModel> RoomConditions { get; set; } = [];
    public string Command { get; set; } = "";
    public string To { get; set; } = "";
    public List<ValueConditionModel> Conditions { get; set; } = [];
    public int Cost { get; set; } = 1;
}
public class LinkModel()
{

    public static LinkModel FromLink(Link link)
    {
        return new LinkModel()
        {
            From = link.From,
            To = link.To,
        };
    }
    public Link ToLink()
    {
        return new Link(From, To);
    }
    public static List<LinkModel> FromLinkList(List<Link> links)
    {
        var list = new List<LinkModel>();
        foreach (var link in links.Where(x => x is not null))
        {
            list.Add(FromLink(link));
        }
        return list;
    }
    public static List<Link> ToLinkList(List<LinkModel> linkModels)
    {
        var list = new List<Link>();
        foreach (var linkModel in linkModels.Where(x => x is not null))
        {
            list.Add(linkModel.ToLink());
        }
        return list;
    }
    public string From { get; set; } = "";
    public string To { get; set; } = "";
}
public class CommandCostModel()
{
    public static CommandCostModel From(CommandCost commandCost)
    {
        return new CommandCostModel()
        {
            Command = commandCost.Command,
            To = commandCost.To,
            Cost = commandCost.Cost,
        };
    }
    public CommandCost ToCommandCost()
    {
        return new CommandCost(Command, To, Cost);
    }
    public static List<CommandCostModel> FromCommandCostList(List<CommandCost> commandCosts)
    {
        var list = new List<CommandCostModel>();
        foreach (var commandCost in commandCosts.Where(x => x is not null))
        {
            list.Add(From(commandCost));
        }
        return list;
    }
    public static List<CommandCost> ToCommandCostList(List<CommandCostModel> commandCostModels)
    {
        var list = new List<CommandCost>();
        foreach (var commandCostModel in commandCostModels.Where(x => x is not null))
        {
            list.Add(commandCostModel.ToCommandCost());
        }
        return list;
    }
    public string Command { get; set; } = "";
    public string To { get; set; } = "";
    public int Cost { get; set; } = 0;

}
public class EnvironmentModel()
{
    public static EnvironmentModel From(Models.Environment data)
    {
        return new EnvironmentModel()
        {
            Tags = ValueTagModel.FromList(data.Tags),
            RoomConditions = ValueConditionModel.FromList(data.RoomConditions),
            Rooms = RoomModel.FromList(data.Rooms),
            Paths = PathModel.FromPathList(data.Paths),
            Shortcuts = RoomConditionExitModel.FromRoomConditionExitList(data.Shortcuts),
            Whitelist = [.. data.Whitelist],
            Blacklist = [.. data.Blacklist],
            BlockedLinks = LinkModel.FromLinkList(data.BlockedLinks),
            CommandCosts = CommandCostModel.FromCommandCostList(data.CommandCosts),
        };
    }
    public Models.Environment ToEnvironment()
    {
        return new Models.Environment()
        {
            Tags = ValueTagModel.ToValueTagList(Tags ?? []),
            RoomConditions = ValueConditionModel.ToValueConditionList(RoomConditions ?? []),
            Rooms = RoomModel.ToRoomList(Rooms ?? []),
            Paths = PathModel.ToPathList(Paths ?? []),
            Shortcuts = RoomConditionExitModel.ToRoomConditionExitList(Shortcuts ?? []),
            Whitelist = [.. Whitelist ?? []],
            Blacklist = [.. Blacklist ?? []],
            BlockedLinks = LinkModel.ToLinkList(BlockedLinks ?? []),
            CommandCosts = CommandCostModel.ToCommandCostList(CommandCosts ?? []),
        };
    }
    public List<ValueTagModel>? Tags { get; set; } = [];
    public List<ValueConditionModel>? RoomConditions { get; set; } = [];
    public List<RoomModel>? Rooms { get; set; } = [];
    public List<PathModel>? Paths { get; set; } = [];
    public List<RoomConditionExitModel>? Shortcuts { get; set; } = [];
    public List<string>? Whitelist { get; set; } = [];
    public List<string>? Blacklist { get; set; } = [];
    public List<LinkModel>? BlockedLinks { get; set; } = [];
    public List<CommandCostModel>? CommandCosts { get; set; } = [];

}
public class MapperOptionsModel
{
    public static MapperOptionsModel From(MapperOptions options)
    {
        return new MapperOptionsModel()
        {
            MaxExitCost = options.MaxExitCost,
            MaxTotalCost = options.MaxTotalCost,
            DisableShortcuts = options.DisableShortcuts,
            CommandWhitelist = [.. options.CommandWhitelist.Keys],
        };
    }
    public MapperOptions ToMapperOptions()
    {
        return new MapperOptions()
        {
            MaxExitCost = MaxExitCost ?? 0,
            MaxTotalCost = MaxTotalCost ?? 0,
            DisableShortcuts = DisableShortcuts ?? false,
        }.WithCommandWhitelist(CommandWhitelist);
    }
    public int? MaxExitCost { get; set; } = 0;
    public int? MaxTotalCost { get; set; } = 0;
    public bool? DisableShortcuts { get; set; } = false;

    public List<string> CommandWhitelist { get; set; } = [];
}

public class InputQueryPathAny()
{

    public static InputQueryPathAny? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputQueryPathAny), APIJsonSerializerContext.Default) is InputQueryPathAny query)
            {
                return query;
            }
        }
        catch
        {
        }
        return null;
    }
    public List<string> From { get; set; } = [];
    public List<string> Target { get; set; } = [];
    public EnvironmentModel Environment { get; set; } = new();
    public MapperOptionsModel Options { get; set; } = new();
}

public class InputQueryPath()
{

    public static InputQueryPath? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputQueryPath), APIJsonSerializerContext.Default) is InputQueryPath query)
            {
                return query;
            }
        }
        catch
        {
        }
        return null;
    }
    public string Start { get; set; } = "";
    public List<string> Target { get; set; } = [];
    public EnvironmentModel Environment { get; set; } = new();
    public MapperOptionsModel Options { get; set; } = new();
}

public class InputDilate()
{
    public static InputDilate? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputDilate), APIJsonSerializerContext.Default) is InputDilate dilate)
            {
                return dilate;
            }
        }
        catch
        {
        }
        return null;
    }
    public List<string> Source { get; set; } = [];
    public int Iterations { get; set; } = 1;
    public EnvironmentModel Environment { get; set; } = new();
    public MapperOptionsModel Options { get; set; } = new();
}
public class InputTrackExit()
{
    public static InputTrackExit? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputTrackExit), APIJsonSerializerContext.Default) is InputTrackExit track)
            {
                return track;
            }
        }
        catch
        {
        }
        return null;
    }
    public string Start { get; set; } = "";
    public string Command { get; set; } = "";
    public EnvironmentModel Environment { get; set; } = new();
    public MapperOptionsModel Options { get; set; } = new();
}
public class InputKey()
{
    public static InputKey? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputKey), APIJsonSerializerContext.Default) is InputKey key)
            {
                return key;
            }
        }
        catch
        {
        }
        return null;
    }
    public string Key { get; set; } = "";
}

public class InputGetRoom()
{
    public static InputGetRoom? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputGetRoom), APIJsonSerializerContext.Default) is InputGetRoom getRoom)
            {
                return getRoom;
            }
        }
        catch
        {
        }
        return null;
    }
    public string Key { get; set; } = "";
    public EnvironmentModel Environment { get; set; } = new();
    public MapperOptionsModel Options { get; set; } = new();

}

public class InputSnapshotFilter()
{
    public static InputSnapshotFilter? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputSnapshotFilter), APIJsonSerializerContext.Default) is InputSnapshotFilter search)
            {
                return search;
            }
        }
        catch
        {
        }
        return null;
    }
    public static InputSnapshotFilter From(SnapshotFilter filter)
    {
        return new InputSnapshotFilter()
        {
            Key = filter.Key,
            Type = filter.Type,
            Group = filter.Group,
            MaxCount = filter.MaxCount,
        };
    }
    public SnapshotFilter ToSnapshotFilter()
    {
        return new SnapshotFilter(Key, Type, Group).WithMaxCount(MaxCount);
    }
    public string? Key { get; set; } = null;
    public string? Type { get; set; } = null;
    public string? Group { get; set; } = null;

    public int MaxCount { get; set; } = 0;

}
public class InputTakeSnapshot()
{
    public static InputTakeSnapshot? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputTakeSnapshot), APIJsonSerializerContext.Default) is InputTakeSnapshot snapshot)
            {
                return snapshot;
            }
        }
        catch
        {
        }
        return null;
    }
    public string Key { get; set; } = "";
    public string Type { get; set; } = "";
    public string Value { get; set; } = "";
    public string Group { get; set; } = "";
}

public class SnapshotSearchModel()
{
    public static SnapshotSearchModel? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(SnapshotSearchModel), APIJsonSerializerContext.Default) is SnapshotSearchModel search)
            {
                return search;
            }
        }
        catch
        {
        }
        return null;
    }
    public static SnapshotSearchModel From(SnapshotSearch search)
    {
        return new SnapshotSearchModel()
        {
            Type = search.Type,
            Group = search.Group,
            Keywords = [.. search.Keywords],
            PartialMatch = search.PartialMatch,
            Any = search.Any,
        };
    }
    public SnapshotSearch ToSnapshotSearch()
    {
        return new SnapshotSearch()
        {
            Type = Type,
            Group = Group,
            Keywords = Keywords ?? [],
            PartialMatch = PartialMatch ?? true,
            Any = Any ?? false,
        };
    }
    public string? Type { get; set; }
    public string? Group { get; set; }
    public List<string>? Keywords { get; set; } = [];
    public bool? PartialMatch { get; set; } = true;
    public bool? Any { get; set; } = false;
}

public class SnapshotSearchResultModel
{
    public static List<SnapshotSearchResultModel> FromList(List<SnapshotSearchResult> results)
    {
        var list = new List<SnapshotSearchResultModel>();
        foreach (var result in results)
        {
            list.Add(From(result));
        }
        return list;
    }
    public static List<SnapshotSearchResult> ToResultList(List<SnapshotSearchResultModel> resultModels)
    {
        var list = new List<SnapshotSearchResult>();
        foreach (var resultModel in resultModels)
        {
            list.Add(resultModel.ToResult());
        }
        return list;
    }
    public SnapshotSearchResult ToResult()
    {
        return new SnapshotSearchResult()
        {
            Key = Key,
            Sum = Sum,
            Count = Count,
            Items = SnapshotModel.ToSnapshotList(Items),
        };
    }
    public static SnapshotSearchResultModel From(SnapshotSearchResult result)
    {
        return new SnapshotSearchResultModel()
        {
            Key = result.Key,
            Sum = result.Sum,
            Count = result.Count,
            Items = SnapshotModel.FromList(result.Items),
        };
    }
    public string Key { get; set; } = "";
    public int Sum { get; set; } = 0;
    public int Count { get; set; } = 0;
    public List<SnapshotModel> Items { get; set; } = [];
}
public class RoomFilterModel
{
    public static RoomFilterModel From(RoomFilter filter)
    {
        return new RoomFilterModel()
        {
            RoomConditions = ValueConditionModel.FromList(filter.RoomConditions),
            HasAnyExitTo = [.. filter.HasAnyExitTo],
            HasAnyData = DataModel.FromList(filter.HasAnyData),
            HasAnyName = [.. filter.HasAnyName],
            HasAnyGroup = [.. filter.HasAnyGroup],
            ContainsAnyData = DataModel.FromList(filter.ContainsAnyData),
            ContainsAnyName = [.. filter.ContainsAnyName],
            ContainsAnyKey = [.. filter.ContainsAnyKey],
        };
    }
    public RoomFilter ToRoomFilter()
    {
        return new RoomFilter()
        {
            RoomConditions = ValueConditionModel.ToValueConditionList(RoomConditions ?? []),
            HasAnyExitTo = [.. HasAnyExitTo ?? []],
            HasAnyData = DataModel.ToDataList(HasAnyData ?? []),
            HasAnyName = [.. HasAnyName ?? []],
            HasAnyGroup = [.. HasAnyGroup ?? []],
            ContainsAnyData = DataModel.ToDataList(ContainsAnyData ?? []),
            ContainsAnyName = [.. ContainsAnyName ?? []],
            ContainsAnyKey = [.. ContainsAnyKey ?? []],
        };
    }
    public List<ValueConditionModel>? RoomConditions { get; set; } = [];
    public List<string>? HasAnyExitTo { get; set; } = [];
    public List<DataModel>? HasAnyData { get; set; } = [];
    public List<string>? HasAnyName { get; set; } = [];
    public List<string>? HasAnyGroup { get; set; } = [];

    public List<DataModel>? ContainsAnyData { get; set; } = [];
    public List<string>? ContainsAnyName { get; set; } = [];
    public List<string>? ContainsAnyKey { get; set; } = [];
}

public class InputSearchRooms
{
    public static InputSearchRooms? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputSearchRooms), APIJsonSerializerContext.Default) is InputSearchRooms search)
            {
                return search;
            }
        }
        catch
        {
        }
        return null;
    }
    public RoomFilterModel Filter { get; set; } = new RoomFilterModel();
}

public class InputFilterRooms
{
    public static InputFilterRooms? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputFilterRooms), APIJsonSerializerContext.Default) is InputFilterRooms filter)
            {
                return filter;
            }
        }
        catch
        {
        }
        return null;
    }
    public RoomFilterModel Filter { get; set; } = new RoomFilterModel();
    public List<string> Source { get; set; } = [];
}

public class InputGroupRoom
{
    public static InputGroupRoom? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputGroupRoom), APIJsonSerializerContext.Default) is InputGroupRoom group)
            {
                return group;
            }
        }
        catch
        {
        }
        return null;
    }
    public string Room { get; set; } = "";
    public string Group { get; set; } = "";
}
public class InputTagRoom
{
    public static InputTagRoom? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputTagRoom), APIJsonSerializerContext.Default) is InputTagRoom tag)
            {
                return tag;
            }
        }
        catch
        {
        }
        return null;
    }
    public string Room { get; set; } = "";
    public string Tag { get; set; } = "";
    public int Value { get; set; } = 0;
}
public class InputSetRoomData
{
    public static InputSetRoomData? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputSetRoomData), APIJsonSerializerContext.Default) is InputSetRoomData setData)
            {
                return setData;
            }
        }
        catch
        {
        }
        return null;
    }
    public string Room { get; set; } = "";
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
}
public class InputTraceLocation
{
    public static InputTraceLocation? FromJSON(string data)
    {
        try
        {
            if (System.Text.Json.JsonSerializer.Deserialize(data, typeof(InputTraceLocation), APIJsonSerializerContext.Default) is InputTraceLocation location)
            {
                return location;
            }
        }
        catch
        {
        }
        return null;
    }
    public string Key { get; set; } = "";
    public string Location { get; set; } = "";
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
[JsonSerializable(typeof(KeyType))]
[JsonSerializable(typeof(List<KeyType>))]
[JsonSerializable(typeof(LandmarkKeyList))]
[JsonSerializable(typeof(KeyTypeValue))]
[JsonSerializable(typeof(List<KeyTypeValue>))]
[JsonSerializable(typeof(SnapshotKeyList))]
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
[JsonSerializable(typeof(VariableModel))]
[JsonSerializable(typeof(List<VariableModel>))]
[JsonSerializable(typeof(InputVariables))]
[JsonSerializable(typeof(LandmarkModel))]
[JsonSerializable(typeof(List<LandmarkModel>))]
[JsonSerializable(typeof(InputLandmarks))]
[JsonSerializable(typeof(SnapshotModel))]
[JsonSerializable(typeof(List<SnapshotModel>))]
[JsonSerializable(typeof(InputSnapshots))]
[JsonSerializable(typeof(StepModel))]
[JsonSerializable(typeof(List<StepModel>))]
[JsonSerializable(typeof(QueryResultModel))]
[JsonSerializable(typeof(PathModel))]
[JsonSerializable(typeof(List<PathModel>))]
[JsonSerializable(typeof(RoomConditionExitModel))]
[JsonSerializable(typeof(List<RoomConditionExitModel>))]
[JsonSerializable(typeof(LinkModel))]
[JsonSerializable(typeof(List<LinkModel>))]
[JsonSerializable(typeof(CommandCostModel))]
[JsonSerializable(typeof(List<CommandCostModel>))]
[JsonSerializable(typeof(EnvironmentModel))]
[JsonSerializable(typeof(MapperOptionsModel))]
[JsonSerializable(typeof(InputQueryPathAny))]
[JsonSerializable(typeof(InputQueryPath))]
[JsonSerializable(typeof(InputDilate))]
[JsonSerializable(typeof(InputTrackExit))]
[JsonSerializable(typeof(InputKey))]
[JsonSerializable(typeof(InputGetRoom))]
[JsonSerializable(typeof(InputSnapshotFilter))]
[JsonSerializable(typeof(InputTakeSnapshot))]
[JsonSerializable(typeof(SnapshotSearchModel))]
[JsonSerializable(typeof(SnapshotSearchResultModel))]
[JsonSerializable(typeof(List<SnapshotSearchResultModel>))]
[JsonSerializable(typeof(RoomFilterModel))]
[JsonSerializable(typeof(InputSearchRooms))]
[JsonSerializable(typeof(InputFilterRooms))]
[JsonSerializable(typeof(InputGroupRoom))]
[JsonSerializable(typeof(InputTagRoom))]
[JsonSerializable(typeof(InputSetRoomData))]
[JsonSerializable(typeof(InputTraceLocation))]

public partial class APIJsonSerializerContext : JsonSerializerContext { }
