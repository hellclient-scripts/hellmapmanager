using System.Collections.Generic;
using HellMapManager.Cores;
using System.Text.Json.Serialization;
using HellMapManager.Models;
using System;
using System.Threading.Tasks;
using Avalonia.Input;

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
    public static List<DataModel> FromList(List<Data> datas)
    {
        var list = new List<DataModel>();
        foreach (var data in datas)
        {
            list.Add(From(data));
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
    public static List<ExitModel> FromList(List<Exit> exits)
    {
        var list = new List<ExitModel>();
        foreach (var exit in exits)
        {
            list.Add(From(exit));
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
    public static List<ValueConditionModel> FromList(List<ValueCondition> conditions)
    {
        var list = new List<ValueConditionModel>();
        foreach (var condition in conditions)
        {
            list.Add(From(condition));
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
    public static List<RoomModel> FromList(List<Room> rooms)
    {
        var list = new List<RoomModel>();
        foreach (var room in rooms)
        {
            list.Add(From(room));
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

[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(APIResultInfo))]
[JsonSerializable(typeof(InputListOption))]
[JsonSerializable(typeof(ValueTagModel))]
[JsonSerializable(typeof(RoomModel))]
[JsonSerializable(typeof(List<RoomModel>))]
[JsonSerializable(typeof(DataModel))]
[JsonSerializable(typeof(List<DataModel>))]
[JsonSerializable(typeof(ValueConditionModel))]
[JsonSerializable(typeof(List<ValueConditionModel>))]
[JsonSerializable(typeof(ExitModel))]
[JsonSerializable(typeof(List<ExitModel>))]
[JsonSerializable(typeof(KeyList))]

public partial class APIJsonSerializerContext : JsonSerializerContext { }
