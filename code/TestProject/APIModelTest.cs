using HellMapManager.Services.API;
using HellMapManager.Models;
using System.Text.Json;
using Avalonia.Input;
namespace TestProject;

[Collection("Core")]
public class APIModelTest
{
    [Fact]
    public void KeyTypeValueTest()
    {
        var model = new KeyTypeValue()
        {
            Key = "TestKey",
            Type = "TestType",
            Value = "TestValue"
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.KeyTypeValue);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.KeyTypeValue);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Type, deserialized?.Type);
        Assert.Equal(model.Value, deserialized?.Value);
        var Raw = model.ToSnapshotKey();
        var FromRaw = KeyTypeValue.FromSnapshotKey(Raw);
        Assert.Equal(model.Key, FromRaw.Key);
        Assert.Equal(model.Type, FromRaw.Type);
        Assert.Equal(model.Value, FromRaw.Value);
        var list = KeyTypeValue.FromSnapshotKeyList(KeyTypeValue.ToSnapshotKeyList(new List<KeyTypeValue>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListKeyTypeValue);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListKeyTypeValue);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Type, deserializedList![0].Type);
        Assert.Equal(model.Value, deserializedList![0].Value);
    }
    [Fact]
    public void SnapshotKeyListTest()
    {
        var keylist = new SnapshotKeyList()
        {
            Keys = new List<KeyTypeValue>(){
                new()
                {
                    Key="Key1",
                    Type="Type1",
                    Value="Value1"
                },
                new()
                {
                    Key="Key2",
                    Type="Type2",
                    Value="Value2"
                },
                new()
                {
                    Key="Key3",
                    Type="Type3",
                    Value="Value3"
                }
        }
        };
        Assert.Null(SnapshotKeyList.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(keylist, APIJsonSerializerContext.Default.SnapshotKeyList);
        var deserialized = SnapshotKeyList.FromJSON(json);
        Assert.Equal(3, deserialized?.Keys.Count);
        for (int i = 0; i < 3; i++)
        {
            Assert.Equal(keylist.Keys[i].Key, deserialized?.Keys[i].Key);
            Assert.Equal(keylist.Keys[i].Type, deserialized?.Keys[i].Type);
            Assert.Equal(keylist.Keys[i].Value, deserialized?.Keys[i].Value);
        }
    }
    [Fact]
    public void KeyTypeTest()
    {
        var model = new KeyType()
        {
            Key = "TestKey",
            Type = "TestType"
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.KeyType);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.KeyType);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Type, deserialized?.Type);
        var Raw = model.ToLandmarkKey();
        var FromRaw = KeyType.FromLandmarkKey(Raw);
        Assert.Equal(model.Key, FromRaw.Key);
        Assert.Equal(model.Type, FromRaw.Type);
        var list = KeyType.FromLandmarkKeyList(KeyType.ToLandmarkKeyList(new List<KeyType>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListKeyType);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListKeyType);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Type, deserializedList![0].Type);
    }
    [Fact]
    public void LandmarkKeyListTest()
    {
        var keylist = new LandmarkKeyList()
        {
            LandmarkKeys = new List<KeyType>(){
                new()
                {
                    Key="Key1",
                    Type="Type1"
                },
                new()
                {
                    Key="Key2",
                    Type="Type2"
                },
                new()
                {
                    Key="Key3",
                    Type="Type3"
                }
        }
        };
        Assert.Null(LandmarkKeyList.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(keylist, APIJsonSerializerContext.Default.LandmarkKeyList);
        var deserialized = LandmarkKeyList.FromJSON(json);
        Assert.Equal(3, deserialized?.LandmarkKeys.Count);
        for (int i = 0; i < 3; i++)
        {
            Assert.Equal(keylist.LandmarkKeys[i].Key, deserialized?.LandmarkKeys[i].Key);
            Assert.Equal(keylist.LandmarkKeys[i].Type, deserialized?.LandmarkKeys[i].Type);
        }
    }
    [Fact]
    public void KeyListTest()
    {
        var keylist = new KeyList()
        {
            Keys = new List<string>(){
                "Key1",
                "Key2",
                "Key3"
        }
        };
        Assert.Null(KeyList.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(keylist, APIJsonSerializerContext.Default.KeyList);
        var deserialized = KeyList.FromJSON(json);
        Assert.Equal(3, deserialized?.Keys.Count);
        for (int i = 0; i < 3; i++)
        {
            Assert.Equal(keylist.Keys[i], deserialized?.Keys[i]);
        }
    }
    [Fact]
    public void InputListOptionTest()
    {
        var model = new InputListOption()
        {
            Keys = new List<string> { "Key1", "Key2", "Key3" },
            Groups = new List<string> { "Group1", "Group2" },
        };
        Assert.Null(InputListOption.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputListOption);
        var deserialized = InputListOption.FromJSON(json);
        Assert.Equal(model.Keys.Count, deserialized?.Keys!.Count);
        for (int i = 0; i < model.Keys.Count; i++)
        {
            Assert.Equal(model.Keys[i], deserialized!.Keys[i]);
        }
        Assert.Equal(model.Groups.Count, deserialized!.Groups!.Count);
        for (int i = 0; i < model.Groups.Count; i++)
        {
            Assert.Equal(model.Groups[i], deserialized!.Groups[i]);
        }
        var raw = model.To();
        var fromRaw = InputListOption.From(raw);
        Assert.Equal(model.Keys.Count, fromRaw.Keys!.Count);
        for (int i = 0; i < model.Keys.Count; i++)
        {
            Assert.Equal(model.Keys[i], fromRaw.Keys[i]);
        }
        Assert.Equal(model.Groups.Count, fromRaw.Groups!.Count);
        for (int i = 0; i < model.Groups.Count; i++)
        {
            Assert.Equal(model.Groups[i], fromRaw.Groups[i]);
        }
        var empty = new InputListOption();
        var emptyRaw = empty.To();
        Assert.NotNull(emptyRaw);
        Assert.Empty(emptyRaw.Keys());
        Assert.Empty(emptyRaw.Groups());
    }
    [Fact]
    public void APIResultInfoTest()
    {
        var model = new APIResultInfo()
        {
            Name = "name",
            Desc = "message"
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.APIResultInfo);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.APIResultInfo);
        Assert.Equal(model.Name, deserialized?.Name);
        Assert.Equal(model.Desc, deserialized?.Desc);
        var emptyModel = APIResultInfo.From(null);
        Assert.Null(emptyModel);
        var raw = new MapInfo();
        raw.Name = "mapname";
        raw.Desc = "mapdesc";
        var fromModel = APIResultInfo.From(raw);
        Assert.NotNull(fromModel);
        Assert.Equal(raw.Name, fromModel!.Name);
        Assert.Equal(raw.Desc, fromModel!.Desc);
    }
    [Fact]
    public void ValueTagModelTest()
    {
        var model = new ValueTagModel()
        {
            Key = "value",
            Value = 1
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.ValueTagModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.ValueTagModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Value, deserialized?.Value);
        var raw = model.ToValueTag();
        var fromRaw = ValueTagModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Value, fromRaw.Value);
        var list = ValueTagModel.FromList(ValueTagModel.ToValueTagList(new List<ValueTagModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListValueTagModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListValueTagModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Value, deserializedList![0].Value);
    }
    [Fact]
    public void DataModelTest()
    {
        var model = new DataModel()
        {
            Key = "dataKey",
            Value = "dataType",
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.DataModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.DataModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Value, deserialized?.Value);
        var raw = model.ToData();
        var fromRaw = DataModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Value, fromRaw.Value);
        var list = DataModel.FromList(DataModel.ToDataList(new List<DataModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListDataModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListDataModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Value, deserializedList![0].Value);
    }
    [Fact]
    public void ExitModelTest()
    {
        var model = new ExitModel()
        {
            Command = "exitCommand",
            To = "exitTo",
            Conditions = new(
                [new ValueConditionModel()
                {
                    Key="condKey",
                    Not=false,
                    Value=1
                }
                ]
            ),
            Cost = 10
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.ExitModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.ExitModel);
        Assert.Equal(model.Command, deserialized?.Command);
        Assert.Equal(model.To, deserialized?.To);
        Assert.Equal(model.Cost, deserialized?.Cost);
        Assert.Equal(model.Conditions.Count, deserialized?.Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, deserialized?.Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, deserialized?.Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, deserialized?.Conditions[i].Value);
        }
        var raw = model.ToExit();
        var fromRaw = ExitModel.From(raw);
        Assert.Equal(model.Command, fromRaw.Command);
        Assert.Equal(model.To, fromRaw.To);
        Assert.Equal(model.Cost, fromRaw.Cost);
        Assert.Equal(model.Conditions.Count, fromRaw.Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, fromRaw.Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, fromRaw.Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, fromRaw.Conditions[i].Value);
        }
        var list = ExitModel.FromList(ExitModel.ToExitList(new List<ExitModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListExitModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListExitModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Command, deserializedList![0].Command);
        Assert.Equal(model.To, deserializedList![0].To);
        Assert.Equal(model.Cost, deserializedList![0].Cost);
        Assert.Equal(model.Conditions.Count, deserializedList![0].Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, deserializedList![0].Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, deserializedList![0].Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, deserializedList![0].Conditions[i].Value);
        }
    }
    [Fact]
    public void ValueConditionModelTest()
    {
        var model = new ValueConditionModel()
        {
            Key = "condKey",
            Not = true,
            Value = 42
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.ValueConditionModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.ValueConditionModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Not, deserialized?.Not);
        Assert.Equal(model.Value, deserialized?.Value);
        var raw = model.ToValueCondition();
        var fromRaw = ValueConditionModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Not, fromRaw.Not);
        Assert.Equal(model.Value, fromRaw.Value);
        var list = ValueConditionModel.FromList(ValueConditionModel.ToValueConditionList(new List<ValueConditionModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListValueConditionModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListValueConditionModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Not, deserializedList![0].Not);
        Assert.Equal(model.Value, deserializedList![0].Value);
    }
    [Fact]
    public void RoomModelTest()
    {
        var model = new RoomModel()
        {
            Key = "roomKey",
            Name = "roomName",
            Desc = "roomDesc",
            Exits = new List<ExitModel>()
            {
                new ExitModel()
                {
                    Command="exitCmd",
                    To="exitTo",
                    Cost=5,
                    Conditions=new List<ValueConditionModel>()
                    {
                        new ValueConditionModel()
                        {
                            Key="condKey",
                            Not=false,
                            Value=3
                        }
                    }
                }
            }
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.RoomModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.RoomModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Name, deserialized?.Name);
        Assert.Equal(model.Desc, deserialized?.Desc);
        Assert.Equal(model.Exits.Count, deserialized?.Exits.Count);
        for (int i = 0; i < model.Exits.Count; i++)
        {
            Assert.Equal(model.Exits[i].Command, deserialized?.Exits[i].Command);
            Assert.Equal(model.Exits[i].To, deserialized?.Exits[i].To);
            Assert.Equal(model.Exits[i].Cost, deserialized?.Exits[i].Cost);
            Assert.Equal(model.Exits[i].Conditions.Count, deserialized?.Exits[i].Conditions.Count);
            for (int j = 0; j < model.Exits[i].Conditions.Count; j++)
            {
                Assert.Equal(model.Exits[i].Conditions[j].Key, deserialized?.Exits[i].Conditions[j].Key);
                Assert.Equal(model.Exits[i].Conditions[j].Not, deserialized?.Exits[i].Conditions[j].Not);
                Assert.Equal(model.Exits[i].Conditions[j].Value, deserialized?.Exits[i].Conditions[j].Value);
            }
        }
        var raw = model.ToRoom();
        var fromRaw = RoomModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Name, fromRaw.Name);
        Assert.Equal(model.Desc, fromRaw.Desc);
        Assert.Equal(model.Exits.Count, fromRaw.Exits.Count);
        for (int i = 0; i < model.Exits.Count; i++)
        {
            Assert.Equal(model.Exits[i].Command, fromRaw.Exits[i].Command);
            Assert.Equal(model.Exits[i].To, fromRaw.Exits[i].To);
            Assert.Equal(model.Exits[i].Cost, fromRaw.Exits[i].Cost);
            Assert.Equal(model.Exits[i].Conditions.Count, fromRaw.Exits[i].Conditions.Count);
            for (int j = 0; j < model.Exits[i].Conditions.Count; j++)
            {
                Assert.Equal(model.Exits[i].Conditions[j].Key, fromRaw.Exits[i].Conditions[j].Key);
                Assert.Equal(model.Exits[i].Conditions[j].Not, fromRaw.Exits[i].Conditions[j].Not);
                Assert.Equal(model.Exits[i].Conditions[j].Value, fromRaw.Exits[i].Conditions[j].Value);
            }
        }
        var list = RoomModel.FromList(RoomModel.ToRoomList(new List<RoomModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListRoomModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListRoomModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Name, deserializedList![0].Name);
        Assert.Equal(model.Desc, deserializedList![0].Desc);
        Assert.Equal(model.Exits.Count, deserializedList![0].Exits.Count);
        for (int i = 0; i < model.Exits.Count; i++)
        {
            Assert.Equal(model.Exits[i].Command, deserializedList![0].Exits[i].Command);
            Assert.Equal(model.Exits[i].To, deserializedList![0].Exits[i].To);
            Assert.Equal(model.Exits[i].Cost, deserializedList![0].Exits[i].Cost);
            Assert.Equal(model.Exits[i].Conditions.Count, deserializedList![0].Exits[i].Conditions.Count);
            for (int j = 0; j < model.Exits[i].Conditions.Count; j++)
            {
                Assert.Equal(model.Exits[i].Conditions[j].Key, deserializedList![0].Exits[i].Conditions[j].Key);
                Assert.Equal(model.Exits[i].Conditions[j].Not, deserializedList![0].Exits[i].Conditions[j].Not);
                Assert.Equal(model.Exits[i].Conditions[j].Value, deserializedList![0].Exits[i].Conditions[j].Value);
            }
        }
    }
    [Fact]
    public void InputRoomsTest()
    {
        var model = new InputRooms()
        {
            Rooms = new List<RoomModel>()
            {
                new RoomModel()
                {
                    Key = "roomKey",
                    Name = "roomName",
                    Desc = "roomDesc",
                    Exits = new List<ExitModel>()
                    {
                        new ExitModel()
                        {
                            Command="exitCmd",
                            To="exitTo",
                            Cost=5,
                            Conditions=new List<ValueConditionModel>()
                            {
                                new ValueConditionModel()
                                {
                                    Key="condKey",
                                    Not=false,
                                    Value=3
                                }
                            }
                        }
                    }
                }
            }
        };
        Assert.Null(InputRooms.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputRooms);
        var deserialized = InputRooms.FromJSON(json);
        Assert.Equal(model.Rooms.Count, deserialized?.Rooms.Count);
        for (int i = 0; i < model.Rooms.Count; i++)
        {
            Assert.Equal(model.Rooms[i].Key, deserialized?.Rooms[i].Key);
            Assert.Equal(model.Rooms[i].Name, deserialized?.Rooms[i].Name);
            Assert.Equal(model.Rooms[i].Desc, deserialized?.Rooms[i].Desc);
            Assert.Equal(model.Rooms[i].Exits.Count, deserialized?.Rooms[i].Exits.Count);
            for (int j = 0; j < model.Rooms[i].Exits.Count; j++)
            {
                Assert.Equal(model.Rooms[i].Exits[j].Command, deserialized?.Rooms[i].Exits[j].Command);
                Assert.Equal(model.Rooms[i].Exits[j].To, deserialized?.Rooms[i].Exits[j].To);
                Assert.Equal(model.Rooms[i].Exits[j].Cost, deserialized?.Rooms[i].Exits[j].Cost);
                Assert.Equal(model.Rooms[i].Exits[j].Conditions.Count, deserialized?.Rooms[i].Exits[j].Conditions.Count);
                for (int k = 0; k < model.Rooms[i].Exits[j].Conditions.Count; k++)
                {
                    Assert.Equal(model.Rooms[i].Exits[j].Conditions[k].Key, deserialized?.Rooms[i].Exits[j].Conditions[k].Key);
                    Assert.Equal(model.Rooms[i].Exits[j].Conditions[k].Not, deserialized?.Rooms[i].Exits[j].Conditions[k].Not);
                    Assert.Equal(model.Rooms[i].Exits[j].Conditions[k].Value, deserialized?.Rooms[i].Exits[j].Conditions[k].Value);
                }
            }
        }
    }
    [Fact]
    public void MarkerModelTest()
    {
        var model = new MarkerModel()
        {
            Key = "markerKey",
            Value = "markerValue",
            Group = "markergroup",
            Desc = "markerdesc",
            Message = "markermsg",
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.MarkerModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.MarkerModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Value, deserialized?.Value);
        Assert.Equal(model.Group, deserialized?.Group);
        Assert.Equal(model.Desc, deserialized?.Desc);
        Assert.Equal(model.Message, deserialized?.Message);
        var raw = model.ToMarker();
        var fromRaw = MarkerModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Value, fromRaw.Value);
        Assert.Equal(model.Group, fromRaw.Group);
        Assert.Equal(model.Desc, fromRaw.Desc);
        Assert.Equal(model.Message, fromRaw.Message);
        var list = MarkerModel.FromList(MarkerModel.ToMarkerList(new List<MarkerModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListMarkerModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListMarkerModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Value, deserializedList![0].Value);
        Assert.Equal(model.Group, deserializedList![0].Group);
        Assert.Equal(model.Desc, deserializedList![0].Desc);
        Assert.Equal(model.Message, deserializedList![0].Message);
    }
    [Fact]
    public void InputMarkersTest()
    {
        var model = new InputMarkers()
        {
            Markers = new List<MarkerModel>()
            {
                new MarkerModel()
                {
                    Key = "markerKey",
                    Value = "markerValue",
                    Group = "markergroup",
                    Desc = "markerdesc",
                    Message = "markermsg",
                }
            }
        };
        Assert.Null(InputMarkers.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputMarkers);
        var deserialized = InputMarkers.FromJSON(json);
        Assert.Equal(model.Markers.Count, deserialized?.Markers.Count);
        for (int i = 0; i < model.Markers.Count; i++)
        {
            Assert.Equal(model.Markers[i].Key, deserialized?.Markers[i].Key);
            Assert.Equal(model.Markers[i].Value, deserialized?.Markers[i].Value);
            Assert.Equal(model.Markers[i].Group, deserialized?.Markers[i].Group);
            Assert.Equal(model.Markers[i].Desc, deserialized?.Markers[i].Desc);
            Assert.Equal(model.Markers[i].Message, deserialized?.Markers[i].Message);
        }
    }
    [Fact]
    public void RouteModelTest()
    {
        var model = new RouteModel()
        {
            Key = "routeKey",
            Desc = "routeDesc",
            Group = "routeGroup",
            Message = "routeMsg",
            Rooms = ["room1", "room2", "room3"]
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.RouteModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.RouteModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Desc, deserialized?.Desc);
        Assert.Equal(model.Group, deserialized?.Group);
        Assert.Equal(model.Message, deserialized?.Message);
        Assert.Equal(model.Rooms.Count, deserialized?.Rooms.Count);
        for (int i = 0; i < model.Rooms.Count; i++)
        {
            Assert.Equal(model.Rooms[i], deserialized?.Rooms[i]);
        }
        var raw = model.ToRoute();
        var fromRaw = RouteModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Desc, fromRaw.Desc);
        Assert.Equal(model.Group, fromRaw.Group);
        Assert.Equal(model.Message, fromRaw.Message);
        Assert.Equal(model.Rooms.Count, fromRaw.Rooms.Count);
        for (int i = 0; i < model.Rooms.Count; i++)
        {
            Assert.Equal(model.Rooms[i], fromRaw.Rooms[i]);
        }
        var list = RouteModel.FromList(RouteModel.ToRouteList(new List<RouteModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListRouteModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListRouteModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Desc, deserializedList![0].Desc);
        Assert.Equal(model.Group, deserializedList![0].Group);
        Assert.Equal(model.Message, deserializedList![0].Message);
        Assert.Equal(model.Rooms.Count, deserializedList![0].Rooms.Count);
        for (int i = 0; i < model.Rooms.Count; i++)
        {
            Assert.Equal(model.Rooms[i], deserializedList![0].Rooms[i]);
        }
    }
    [Fact]
    public void InputRoutesTest()
    {
        var model = new InputRoutes()
        {
            Routes = new List<RouteModel>()
            {
                new RouteModel()
                {
                    Key = "routeKey",
                    Desc = "routeDesc",
                    Group = "routeGroup",
                    Message = "routeMsg",
                    Rooms = ["room1", "room2", "room3"]
                }
            }
        };
        Assert.Null(InputRoutes.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputRoutes);
        var deserialized = InputRoutes.FromJSON(json);
        Assert.Equal(model.Routes.Count, deserialized?.Routes.Count);
        for (int i = 0; i < model.Routes.Count; i++)
        {
            Assert.Equal(model.Routes[i].Key, deserialized?.Routes[i].Key);
            Assert.Equal(model.Routes[i].Desc, deserialized?.Routes[i].Desc);
            Assert.Equal(model.Routes[i].Group, deserialized?.Routes[i].Group);
            Assert.Equal(model.Routes[i].Message, deserialized?.Routes[i].Message);
            Assert.Equal(model.Routes[i].Rooms.Count, deserialized?.Routes[i].Rooms.Count);
            for (int j = 0; j < model.Routes[i].Rooms.Count; j++)
            {
                Assert.Equal(model.Routes[i].Rooms[j], deserialized?.Routes[i].Rooms[j]);
            }
        }
    }
    [Fact]
    public void TraceModelTest()
    {
        var model = new TraceModel()
        {
            Key = "traceKey",
            Desc = "traceDesc",
            Group = "traceGroup",
            Message = "traceMsg",
            Locations = ["step1", "step2", "step3"]
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.TraceModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.TraceModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Desc, deserialized?.Desc);
        Assert.Equal(model.Group, deserialized?.Group);
        Assert.Equal(model.Message, deserialized?.Message);
        Assert.Equal(model.Locations.Count, deserialized?.Locations.Count);
        for (int i = 0; i < model.Locations.Count; i++)
        {
            Assert.Equal(model.Locations[i], deserialized?.Locations[i]);
        }
        var raw = model.ToTrace();
        var fromRaw = TraceModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Desc, fromRaw.Desc);
        Assert.Equal(model.Group, fromRaw.Group);
        Assert.Equal(model.Message, fromRaw.Message);
        Assert.Equal(model.Locations.Count, fromRaw.Locations.Count);
        for (int i = 0; i < model.Locations.Count; i++)
        {
            Assert.Equal(model.Locations[i], fromRaw.Locations[i]);
        }
        var list = TraceModel.FromList(TraceModel.ToTraceList(new List<TraceModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListTraceModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListTraceModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Desc, deserializedList![0].Desc);
        Assert.Equal(model.Group, deserializedList![0].Group);
        Assert.Equal(model.Message, deserializedList![0].Message);
        Assert.Equal(model.Locations.Count, deserializedList![0].Locations.Count);
        for (int i = 0; i < model.Locations.Count; i++)
        {
            Assert.Equal(model.Locations[i], deserializedList![0].Locations[i]);
        }
    }
    [Fact]
    public void InputTracesTest()
    {
        var model = new InputTraces()
        {
            Traces = new List<TraceModel>()
            {
                new TraceModel()
                {
                    Key = "traceKey",
                    Desc = "traceDesc",
                    Group = "traceGroup",
                    Message = "traceMsg",
                    Locations = ["step1", "step2", "step3"]
                }
            }
        };
        Assert.Null(InputTraces.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputTraces);
        var deserialized = InputTraces.FromJSON(json);
        Assert.Equal(model.Traces.Count, deserialized?.Traces.Count);
        for (int i = 0; i < model.Traces.Count; i++)
        {
            Assert.Equal(model.Traces[i].Key, deserialized?.Traces[i].Key);
            Assert.Equal(model.Traces[i].Desc, deserialized?.Traces[i].Desc);
            Assert.Equal(model.Traces[i].Group, deserialized?.Traces[i].Group);
            Assert.Equal(model.Traces[i].Message, deserialized?.Traces[i].Message);
            Assert.Equal(model.Traces[i].Locations.Count, deserialized?.Traces[i].Locations.Count);
            for (int j = 0; j < model.Traces[i].Locations.Count; j++)
            {
                Assert.Equal(model.Traces[i].Locations[j], deserialized?.Traces[i].Locations[j]);
            }
        }
    }
    [Fact]
    public void RegionItemModelTest()
    {
        var model = new RegionItemModel()
        {
            Not = true,
            Type = "Zone",
            Value = "regionValue",
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.RegionItemModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.RegionItemModel);
        Assert.Equal(model.Not, deserialized?.Not);
        Assert.Equal(model.Type, deserialized?.Type);
        Assert.Equal(model.Value, deserialized?.Value);
        var raw = model.ToRegionItem();
        var fromRaw = RegionItemModel.From(raw);
        Assert.Equal(model.Not, fromRaw.Not);
        Assert.Equal(model.Type, fromRaw.Type);
        Assert.Equal(model.Value, fromRaw.Value);
        var list = RegionItemModel.FromList(RegionItemModel.ToRegionItemList(new List<RegionItemModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListRegionItemModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListRegionItemModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Not, deserializedList![0].Not);
        Assert.Equal(model.Type, deserializedList![0].Type);
        Assert.Equal(model.Value, deserializedList![0].Value);
    }
    [Fact]
    public void RegionModelTest()
    {
        var model = new RegionModel()
        {
            Key = "regionKey",
            Desc = "regionDesc",
            Items = new List<RegionItemModel>()
            {
                new RegionItemModel()
                {
                    Not =true,
                    Type = "Zone",
                    Value = "regionValue",
                }
            }
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.RegionModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.RegionModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Desc, deserialized?.Desc);
        Assert.Equal(model.Items.Count, deserialized?.Items.Count);
        for (int i = 0; i < model.Items.Count; i++)
        {
            Assert.Equal(model.Items[i].Not, deserialized?.Items[i].Not);
            Assert.Equal(model.Items[i].Type, deserialized?.Items[i].Type);
            Assert.Equal(model.Items[i].Value, deserialized?.Items[i].Value);
        }
        var raw = model.ToRegion();
        var fromRaw = RegionModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Desc, fromRaw.Desc);
        Assert.Equal(model.Items.Count, fromRaw.Items.Count);
        for (int i = 0; i < model.Items.Count; i++)
        {
            Assert.Equal(model.Items[i].Not, fromRaw.Items[i].Not);
            Assert.Equal(model.Items[i].Type, fromRaw.Items[i].Type);
            Assert.Equal(model.Items[i].Value, fromRaw.Items[i].Value);
        }
        var list = RegionModel.FromList(RegionModel.ToRegionList(new List<RegionModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListRegionModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListRegionModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Desc, deserializedList![0].Desc);
        Assert.Equal(model.Items.Count, deserializedList![0].Items.Count);
        for (int i = 0; i < model.Items.Count; i++)
        {
            Assert.Equal(model.Items[i].Not, deserializedList![0].Items[i].Not);
            Assert.Equal(model.Items[i].Type, deserializedList![0].Items[i].Type);
            Assert.Equal(model.Items[i].Value, deserializedList![0].Items[i].Value);
        }
    }
    [Fact]
    public void InputRegionsTest()
    {
        var model = new InputRegions()
        {
            Regions = new List<RegionModel>()
            {
                new RegionModel()
                {
                    Key = "regionKey",
                    Desc = "regionDesc",
                    Items = new List<RegionItemModel>()
                    {
                        new RegionItemModel()
                        {
                            Not =true,
                            Type = "Zone",
                            Value = "regionValue",
                        }
                    }
                }
            }
        };
        Assert.Null(InputRegions.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputRegions);
        var deserialized = InputRegions.FromJSON(json);
        Assert.Equal(model.Regions.Count, deserialized?.Regions.Count);
        for (int i = 0; i < model.Regions.Count; i++)
        {
            Assert.Equal(model.Regions[i].Key, deserialized?.Regions[i].Key);
            Assert.Equal(model.Regions[i].Desc, deserialized?.Regions[i].Desc);
            Assert.Equal(model.Regions[i].Items.Count, deserialized?.Regions[i].Items.Count);
            for (int j = 0; j < model.Regions[i].Items.Count; j++)
            {
                Assert.Equal(model.Regions[i].Items[j].Not, deserialized?.Regions[i].Items[j].Not);
                Assert.Equal(model.Regions[i].Items[j].Type, deserialized?.Regions[i].Items[j].Type);
                Assert.Equal(model.Regions[i].Items[j].Value, deserialized?.Regions[i].Items[j].Value);
            }
        }
    }
    [Fact]
    public void ShortcutModelTest()
    {
        var model = new ShortcutModel()
        {
            Key = "shortcutKey",
            Command = "shortcutCommand",
            To = "shortcutTo",
            RoomConditions = new List<ValueConditionModel>()
            {
                new ValueConditionModel()
                {
                    Key = "condKey",
                    Not = false,
                    Value = 7
                }
            },
            Conditions = new List<ValueConditionModel>()
            {
                new ValueConditionModel()
                {
                    Key = "condKey2",
                    Not = true,
                    Value = 9
                }
            },
            Cost = 15,
            Group = "shortcutGroup",
            Desc = "shortcutDesc",
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.ShortcutModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.ShortcutModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Command, deserialized?.Command);
        Assert.Equal(model.To, deserialized?.To);
        Assert.Equal(model.Cost, deserialized?.Cost);
        Assert.Equal(model.Group, deserialized?.Group);
        Assert.Equal(model.Desc, deserialized?.Desc);
        Assert.Equal(model.RoomConditions.Count, deserialized?.RoomConditions.Count);
        for (int i = 0; i < model.RoomConditions.Count; i++)
        {
            Assert.Equal(model.RoomConditions[i].Key, deserialized?.RoomConditions[i].Key);
            Assert.Equal(model.RoomConditions[i].Not, deserialized?.RoomConditions[i].Not);
            Assert.Equal(model.RoomConditions[i].Value, deserialized?.RoomConditions[i].Value);
        }
        Assert.Equal(model.Conditions.Count, deserialized?.Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, deserialized?.Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, deserialized?.Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, deserialized?.Conditions[i].Value);
        }
        var raw = model.ToShortcut();
        var fromRaw = ShortcutModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Command, fromRaw.Command);
        Assert.Equal(model.To, fromRaw.To);
        Assert.Equal(model.Cost, fromRaw.Cost);
        Assert.Equal(model.Group, fromRaw.Group);
        Assert.Equal(model.Desc, fromRaw.Desc);
        Assert.Equal(model.RoomConditions.Count, fromRaw.RoomConditions.Count);
        for (int i = 0; i < model.RoomConditions.Count; i++)
        {
            Assert.Equal(model.RoomConditions[i].Key, fromRaw.RoomConditions[i].Key);
            Assert.Equal(model.RoomConditions[i].Not, fromRaw.RoomConditions[i].Not);
            Assert.Equal(model.RoomConditions[i].Value, fromRaw.RoomConditions[i].Value);
        }
        Assert.Equal(model.Conditions.Count, fromRaw.Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, fromRaw.Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, fromRaw.Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, fromRaw.Conditions[i].Value);
        }
        var list = ShortcutModel.FromList(ShortcutModel.ToShortcutList(new List<ShortcutModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListShortcutModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListShortcutModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Command, deserializedList![0].Command);
        Assert.Equal(model.To, deserializedList![0].To);
        Assert.Equal(model.Cost, deserializedList![0].Cost);
        Assert.Equal(model.Group, deserializedList![0].Group);
        Assert.Equal(model.Desc, deserializedList![0].Desc);
        Assert.Equal(model.RoomConditions.Count, deserializedList![0].RoomConditions.Count);
        for (int i = 0; i < model.RoomConditions.Count; i++)
        {
            Assert.Equal(model.RoomConditions[i].Key, deserializedList![0].RoomConditions[i].Key);
            Assert.Equal(model.RoomConditions[i].Not, deserializedList![0].RoomConditions[i].Not);
            Assert.Equal(model.RoomConditions[i].Value, deserializedList![0].RoomConditions[i].Value);
        }
        Assert.Equal(model.Conditions.Count, deserializedList![0].Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, deserializedList![0].Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, deserializedList![0].Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, deserializedList![0].Conditions[i].Value);
        }
    }
    [Fact]
    public void InputShortcutsTest()
    {
        var model = new InputShortcuts()
        {
            Shortcuts = new List<ShortcutModel>()
            {
                new ShortcutModel()
                {
                    Key = "shortcutKey",
                    Command = "shortcutCommand",
                    To="shortcutTo",
                    RoomConditions= new List<ValueConditionModel>()
                    {
                        new ValueConditionModel()
                        {
                            Key = "condKey",
                            Not = false,
                            Value = 7
                        }
                    },
                    Conditions = new List<ValueConditionModel>()
                    {
                        new ValueConditionModel()
                        {
                            Key = "condKey2",
                            Not = true,
                            Value = 9
                        }
                    },
                    Cost = 15,
                    Group = "shortcutGroup",
                    Desc = "shortcutDesc",
                }
            }
        };
        Assert.Null(InputShortcuts.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputShortcuts);
        var deserialized = InputShortcuts.FromJSON(json);
        Assert.Equal(model.Shortcuts.Count, deserialized?.Shortcuts.Count);
        for (int i = 0; i < model.Shortcuts.Count; i++)
        {
            Assert.Equal(model.Shortcuts[i].Key, deserialized?.Shortcuts[i].Key);
            Assert.Equal(model.Shortcuts[i].Command, deserialized?.Shortcuts[i].Command);
            Assert.Equal(model.Shortcuts[i].To, deserialized?.Shortcuts[i].To);
            Assert.Equal(model.Shortcuts[i].Cost, deserialized?.Shortcuts[i].Cost);
            Assert.Equal(model.Shortcuts[i].Group, deserialized?.Shortcuts[i].Group);
            Assert.Equal(model.Shortcuts[i].Desc, deserialized?.Shortcuts[i].Desc);
            Assert.Equal(model.Shortcuts[i].RoomConditions.Count, deserialized?.Shortcuts[i].RoomConditions.Count);
            for (int j = 0; j < model.Shortcuts[i].RoomConditions.Count; j++)
            {
                Assert.Equal(model.Shortcuts[i].RoomConditions[j].Key, deserialized?.Shortcuts[i].RoomConditions[j].Key);
                Assert.Equal(model.Shortcuts[i].RoomConditions[j].Not, deserialized?.Shortcuts[i].RoomConditions[j].Not);
                Assert.Equal(model.Shortcuts[i].RoomConditions[j].Value, deserialized?.Shortcuts[i].RoomConditions[j].Value);
            }
            Assert.Equal(model.Shortcuts[i].Conditions.Count, deserialized?.Shortcuts[i].Conditions.Count);
            for (int j = 0; j < model.Shortcuts[i].Conditions.Count; j++)
            {
                Assert.Equal(model.Shortcuts[i].Conditions[j].Key, deserialized?.Shortcuts[i].Conditions[j].Key);
                Assert.Equal(model.Shortcuts[i].Conditions[j].Not, deserialized?.Shortcuts[i].Conditions[j].Not);
                Assert.Equal(model.Shortcuts[i].Conditions[j].Value, deserialized?.Shortcuts[i].Conditions[j].Value);
            }
        }
    }
    [Fact]
    public void VariableModelTest()
    {
        var model = new VariableModel()
        {
            Key = "varKey",
            Value = "varValue",
            Group = "varGroup",
            Desc = "varDesc",
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.VariableModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.VariableModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Value, deserialized?.Value);
        Assert.Equal(model.Group, deserialized?.Group);
        Assert.Equal(model.Desc, deserialized?.Desc);
        var raw = model.ToVariable();
        var fromRaw = VariableModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Value, fromRaw.Value);
        Assert.Equal(model.Group, fromRaw.Group);
        Assert.Equal(model.Desc, fromRaw.Desc);
        var list = VariableModel.FromList(VariableModel.ToVariableList(new List<VariableModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListVariableModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListVariableModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Value, deserializedList![0].Value);
        Assert.Equal(model.Group, deserializedList![0].Group);
        Assert.Equal(model.Desc, deserializedList![0].Desc);
    }
    [Fact]
    public void InputVariablesTest()
    {
        var model = new InputVariables()
        {
            Variables = new List<VariableModel>()
            {
                new VariableModel()
                {
                    Key = "varKey",
                    Value="varValue",
                    Group="varGroup",
                    Desc="varDesc",
                }
            }
        };
        Assert.Null(InputVariables.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputVariables);
        var deserialized = InputVariables.FromJSON(json);
        Assert.Equal(model.Variables.Count, deserialized?.Variables.Count);
        for (int i = 0; i < model.Variables.Count; i++)
        {
            Assert.Equal(model.Variables[i].Key, deserialized?.Variables[i].Key);
            Assert.Equal(model.Variables[i].Value, deserialized?.Variables[i].Value);
            Assert.Equal(model.Variables[i].Group, deserialized?.Variables[i].Group);
            Assert.Equal(model.Variables[i].Desc, deserialized?.Variables[i].Desc);
        }
    }
    [Fact]
    public void LandmarkModelTest()
    {
        var model = new LandmarkModel()
        {
            Key = "landmarkKey",
            Type = "landmarkType",
            Desc = "landmarkDesc",
            Value = "landmarkValue",
            Group = "landmarkGroup",
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.LandmarkModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.LandmarkModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Type, deserialized?.Type);
        Assert.Equal(model.Desc, deserialized?.Desc);
        Assert.Equal(model.Value, deserialized?.Value);
        Assert.Equal(model.Group, deserialized?.Group);
        var raw = model.ToLandmark();
        var fromRaw = LandmarkModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Type, fromRaw.Type);
        Assert.Equal(model.Desc, fromRaw.Desc);
        Assert.Equal(model.Value, fromRaw.Value);
        Assert.Equal(model.Group, fromRaw.Group);
        var list = LandmarkModel.FromList(LandmarkModel.ToLandmarkList(new List<LandmarkModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListLandmarkModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListLandmarkModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Type, deserializedList![0].Type);
        Assert.Equal(model.Desc, deserializedList![0].Desc);
        Assert.Equal(model.Value, deserializedList![0].Value);
        Assert.Equal(model.Group, deserializedList![0].Group);
    }
    [Fact]
    public void InputLandmarksTest()
    {
        var model = new InputLandmarks()
        {
            Landmarks = new List<LandmarkModel>()
            {
                new LandmarkModel()
                {
                    Key = "landmarkKey",
                    Type="landmarkType",
                    Desc = "landmarkDesc",
                    Value="landmarkValue",
                    Group = "landmarkGroup",
                }
            }
        };
        Assert.Null(InputLandmarks.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputLandmarks);
        var deserialized = InputLandmarks.FromJSON(json);
        Assert.Equal(model.Landmarks.Count, deserialized?.Landmarks.Count);
        for (int i = 0; i < model.Landmarks.Count; i++)
        {
            Assert.Equal(model.Landmarks[i].Key, deserialized?.Landmarks[i].Key);
            Assert.Equal(model.Landmarks[i].Type, deserialized?.Landmarks[i].Type);
            Assert.Equal(model.Landmarks[i].Desc, deserialized?.Landmarks[i].Desc);
            Assert.Equal(model.Landmarks[i].Value, deserialized?.Landmarks[i].Value);
            Assert.Equal(model.Landmarks[i].Group, deserialized?.Landmarks[i].Group);
        }
    }
    [Fact]
    public void SnapshotModelTest()
    {
        var model = new SnapshotModel()
        {
            Key = "snapshotKey",
            Timestamp = 1716234567,
            Group = "snapshotGroup",
            Type = "snapshotType",
            Count = 2,
            Value = "snapshotValue",
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.SnapshotModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.SnapshotModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Timestamp, deserialized?.Timestamp);
        Assert.Equal(model.Group, deserialized?.Group);
        Assert.Equal(model.Type, deserialized?.Type);
        Assert.Equal(model.Count, deserialized?.Count);
        Assert.Equal(model.Value, deserialized?.Value);
        var raw = model.ToSnapshot();
        var fromRaw = SnapshotModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Timestamp, fromRaw.Timestamp);
        Assert.Equal(model.Group, fromRaw.Group);
        Assert.Equal(model.Type, fromRaw.Type);
        Assert.Equal(model.Count, fromRaw.Count);
        Assert.Equal(model.Value, fromRaw.Value);
        var list = SnapshotModel.FromList(SnapshotModel.ToSnapshotList(new List<SnapshotModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListSnapshotModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListSnapshotModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Timestamp, deserializedList![0].Timestamp);
        Assert.Equal(model.Group, deserializedList![0].Group);
        Assert.Equal(model.Type, deserializedList![0].Type);
        Assert.Equal(model.Count, deserializedList![0].Count);
        Assert.Equal(model.Value, deserializedList![0].Value);
    }
    [Fact]
    public void InputSnapshotsTest()
    {
        var model = new InputSnapshots()
        {
            Snapshots = new List<SnapshotModel>()
            {
                new SnapshotModel()
                {
                    Key = "snapshotKey",
                    Timestamp = 1716234567,
                    Group = "snapshotGroup",
                    Type = "snapshotType",
                    Count = 2,
                    Value = "snapshotValue",
                }
            }
        };
        Assert.Null(InputSnapshots.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputSnapshots);
        var deserialized = InputSnapshots.FromJSON(json);
        Assert.Equal(model.Snapshots.Count, deserialized?.Snapshots.Count);
        for (int i = 0; i < model.Snapshots.Count; i++)
        {
            Assert.Equal(model.Snapshots[i].Key, deserialized?.Snapshots[i].Key);
            Assert.Equal(model.Snapshots[i].Timestamp, deserialized?.Snapshots[i].Timestamp);
            Assert.Equal(model.Snapshots[i].Group, deserialized?.Snapshots[i].Group);
            Assert.Equal(model.Snapshots[i].Type, deserialized?.Snapshots[i].Type);
            Assert.Equal(model.Snapshots[i].Count, deserialized?.Snapshots[i].Count);
            Assert.Equal(model.Snapshots[i].Value, deserialized?.Snapshots[i].Value);
        }
    }
    [Fact]
    public void StepModelTest()
    {
        var model = new StepModel()
        {
            Command = "stepCommand",
            Target = "stepTarget",
            Cost = 8,
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.StepModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.StepModel);
        Assert.Equal(model.Command, deserialized?.Command);
        Assert.Equal(model.Target, deserialized?.Target);
        Assert.Equal(model.Cost, deserialized?.Cost);
        var raw = model.ToStep();
        var fromRaw = StepModel.From(raw);
        Assert.Equal(model.Command, fromRaw.Command);
        Assert.Equal(model.Target, fromRaw.Target);
        Assert.Equal(model.Cost, fromRaw.Cost);
        var list = StepModel.FromList(StepModel.ToStepList(new List<StepModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListStepModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListStepModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Command, deserializedList![0].Command);
        Assert.Equal(model.Target, deserializedList![0].Target);
        Assert.Equal(model.Cost, deserializedList![0].Cost);
    }
    [Fact]
    public void QueryResultModelTest()
    {
        var model = new QueryResultModel()
        {
            From = "startPoint",
            To = "endPoint",
            Cost = 25,
            Steps = new([
                new StepModel()
                {
                    Command = "step1Command",
                    Target = "step1Target",
                    Cost = 10,
                },
                new StepModel()
                {
                    Command = "step2Command",
                    Target = "step2Target",
                    Cost = 15,
                }
            ]),
            Unvisited = ["roomA", "roomB"],
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.QueryResultModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.QueryResultModel);
        Assert.Equal(model.From, deserialized?.From);
        Assert.Equal(model.To, deserialized?.To);
        Assert.Equal(model.Cost, deserialized?.Cost);
        Assert.Equal(model.Steps.Count, deserialized?.Steps.Count);
        for (int i = 0; i < model.Steps.Count; i++)
        {
            Assert.Equal(model.Steps[i].Command, deserialized?.Steps[i].Command);
            Assert.Equal(model.Steps[i].Target, deserialized?.Steps[i].Target);
            Assert.Equal(model.Steps[i].Cost, deserialized?.Steps[i].Cost);
        }
        var raw = model.ToQueryResult();
        var fromRaw = QueryResultModel.FromQueryResult(raw);
        Assert.Equal(model.From, fromRaw.From);
        Assert.Equal(model.To, fromRaw.To);
        Assert.Equal(model.Cost, fromRaw.Cost);
        Assert.Equal(model.Steps.Count, fromRaw.Steps.Count);
        for (int i = 0; i < model.Steps.Count; i++)
        {
            Assert.Equal(model.Steps[i].Command, fromRaw.Steps[i].Command);
            Assert.Equal(model.Steps[i].Target, fromRaw.Steps[i].Target);
            Assert.Equal(model.Steps[i].Cost, fromRaw.Steps[i].Cost);
        }
        Assert.Equal(model.Unvisited.Count, fromRaw.Unvisited.Count);
        for (int i = 0; i < model.Unvisited.Count; i++)
        {
            Assert.Equal(model.Unvisited[i], fromRaw.Unvisited[i]);
        }
    }
    [Fact]
    public void PathModelTest()
    {
        var model = new PathModel()
        {
            From = "pathFrom",
            To = "pathTo",
            Command = "pathCommand",
            Conditions = new List<ValueConditionModel>()
            {
                new ValueConditionModel()
                {
                    Key = "condKey",
                    Not = false,
                    Value = 5
                }
            },
            Cost = 12,
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.PathModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.PathModel);
        Assert.Equal(model.From, deserialized?.From);
        Assert.Equal(model.To, deserialized?.To);
        Assert.Equal(model.Command, deserialized?.Command);
        Assert.Equal(model.Cost, deserialized?.Cost);
        Assert.Equal(model.Conditions.Count, deserialized?.Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, deserialized?.Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, deserialized?.Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, deserialized?.Conditions[i].Value);
        }
        var raw = model.ToPath();
        var fromRaw = PathModel.FromPath(raw);
        Assert.Equal(model.From, fromRaw.From);
        Assert.Equal(model.To, fromRaw.To);
        Assert.Equal(model.Command, fromRaw.Command);
        Assert.Equal(model.Cost, fromRaw.Cost);
        Assert.Equal(model.Conditions.Count, fromRaw.Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, fromRaw.Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, fromRaw.Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, fromRaw.Conditions[i].Value);
        }
        var list = PathModel.FromPathList(PathModel.ToPathList(new List<PathModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListPathModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListPathModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.From, deserializedList![0].From);
        Assert.Equal(model.To, deserializedList![0].To);
        Assert.Equal(model.Command, deserializedList![0].Command);
        Assert.Equal(model.Cost, deserializedList![0].Cost);
        Assert.Equal(model.Conditions.Count, deserializedList![0].Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, deserializedList![0].Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, deserializedList![0].Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, deserializedList![0].Conditions[i].Value);
        }
    }
    [Fact]
    public void RoomConditionExitModelTest()
    {
        var model = new RoomConditionExitModel()
        {
            Command = "north",
            To = "roomB",
            RoomConditions = new List<ValueConditionModel>()
            {
                new ValueConditionModel()
                {
                    Key = "condKey",
                    Not = true,
                    Value = 3
                }
            },
            Conditions = new List<ValueConditionModel>()
            {
                new ValueConditionModel()
                {
                    Key = "condKey",
                    Not = true,
                    Value = 3
                }
            },
            Cost = 5,
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.RoomConditionExitModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.RoomConditionExitModel);
        Assert.Equal(model.Command, deserialized?.Command);
        Assert.Equal(model.To, deserialized?.To);
        Assert.Equal(model.Cost, deserialized?.Cost);
        Assert.Equal(model.RoomConditions.Count, deserialized?.RoomConditions.Count);
        for (int i = 0; i < model.RoomConditions.Count; i++)
        {
            Assert.Equal(model.RoomConditions[i].Key, deserialized?.RoomConditions[i].Key);
            Assert.Equal(model.RoomConditions[i].Not, deserialized?.RoomConditions[i].Not);
            Assert.Equal(model.RoomConditions[i].Value, deserialized?.RoomConditions[i].Value);
        }
        Assert.Equal(model.Conditions.Count, deserialized?.Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, deserialized?.Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, deserialized?.Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, deserialized?.Conditions[i].Value);
        }
        var raw = model.ToRoomConditionExit();
        var fromRaw = RoomConditionExitModel.From(raw);
        Assert.Equal(model.Command, fromRaw.Command);
        Assert.Equal(model.To, fromRaw.To);
        Assert.Equal(model.Cost, fromRaw.Cost);
        Assert.Equal(model.RoomConditions.Count, fromRaw.RoomConditions.Count);
        for (int i = 0; i < model.RoomConditions.Count; i++)
        {
            Assert.Equal(model.RoomConditions[i].Key, fromRaw.RoomConditions[i].Key);
            Assert.Equal(model.RoomConditions[i].Not, fromRaw.RoomConditions[i].Not);
            Assert.Equal(model.RoomConditions[i].Value, fromRaw.RoomConditions[i].Value);
        }
        Assert.Equal(model.Conditions.Count, fromRaw.Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, fromRaw.Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, fromRaw.Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, fromRaw.Conditions[i].Value);
        }
        var list = RoomConditionExitModel.FromRoomConditionExitList(RoomConditionExitModel.ToRoomConditionExitList(new List<RoomConditionExitModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListRoomConditionExitModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListRoomConditionExitModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Command, deserializedList![0].Command);
        Assert.Equal(model.To, deserializedList![0].To);
        Assert.Equal(model.Cost, deserializedList![0].Cost);
        Assert.Equal(model.RoomConditions.Count, deserializedList![0].RoomConditions.Count);
        for (int i = 0; i < model.RoomConditions.Count; i++)
        {
            Assert.Equal(model.RoomConditions[i].Key, deserializedList![0].RoomConditions[i].Key);
            Assert.Equal(model.RoomConditions[i].Not, deserializedList![0].RoomConditions[i].Not);
            Assert.Equal(model.RoomConditions[i].Value, deserializedList![0].RoomConditions[i].Value);
        }
        Assert.Equal(model.Conditions.Count, deserializedList![0].Conditions.Count);
        for (int i = 0; i < model.Conditions.Count; i++)
        {
            Assert.Equal(model.Conditions[i].Key, deserializedList![0].Conditions[i].Key);
            Assert.Equal(model.Conditions[i].Not, deserializedList![0].Conditions[i].Not);
            Assert.Equal(model.Conditions[i].Value, deserializedList![0].Conditions[i].Value);
        }
    }
    [Fact]
    public void LinkModelTest()
    {
        var model = new LinkModel()
        {
            From = "linkFrom",
            To = "linkTo",
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.LinkModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.LinkModel);
        Assert.Equal(model.From, deserialized?.From);
        Assert.Equal(model.To, deserialized?.To);
        var raw = model.ToLink();
        var fromRaw = LinkModel.FromLink(raw);
        Assert.Equal(model.From, fromRaw.From);
        Assert.Equal(model.To, fromRaw.To);
        var list = LinkModel.FromLinkList(LinkModel.ToLinkList(new List<LinkModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListLinkModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListLinkModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.From, deserializedList![0].From);
        Assert.Equal(model.To, deserializedList![0].To);
    }
    [Fact]
    public void CommandCostModelTest()
    {
        var model = new CommandCostModel()
        {
            Command = "cmdTest",
            To = "toTest",
            Cost = 20,
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.CommandCostModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.CommandCostModel);
        Assert.Equal(model.Command, deserialized?.Command);
        Assert.Equal(model.To, deserialized?.To);
        Assert.Equal(model.Cost, deserialized?.Cost);
        var raw = model.ToCommandCost();
        var fromRaw = CommandCostModel.From(raw);
        Assert.Equal(model.Command, fromRaw.Command);
        Assert.Equal(model.To, fromRaw.To);
        Assert.Equal(model.Cost, fromRaw.Cost);
        var list = CommandCostModel.FromCommandCostList(CommandCostModel.ToCommandCostList(new List<CommandCostModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListCommandCostModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListCommandCostModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Command, deserializedList![0].Command);
        Assert.Equal(model.To, deserializedList![0].To);
        Assert.Equal(model.Cost, deserializedList![0].Cost);
    }
    [Fact]
    public void EnvironmentModelTest()
    {
        var model = new EnvironmentModel()
        {
            Tags = new([
                new ValueTagModel()
                {
                    Key = "tagKey",
                    Value = 1,
                }
            ]),
            RoomConditions = new([
                new ValueConditionModel()
                {
                    Key = "condKey",
                    Not = false,
                    Value = 4
                }
            ]),
            Rooms = new([
                new RoomModel()
                {
                    Key = "roomKey",
                    Desc = "roomDesc",
                    Group = "roomGroup",
                }
            ]),
            Paths = new([
                new PathModel()
                {
                    From = "pathFrom",
                    To = "pathTo",
                    Command = "pathCommand",
                    Cost = 10,
                }
            ]),
            Shortcuts = new([
                new RoomConditionExitModel()
                {
                    Command = "north",
                    To = "roomB",
                    Cost = 5,
                    RoomConditions = new List<ValueConditionModel>()
                    {
                        new ValueConditionModel()
                        {
                            Key = "condKey",
                            Not = true,
                            Value = 3
                        }
                    },
                }
            ]),
            Whitelist = ["room1", "room2"],
            Blacklist = ["room3", "room4"],
            BlockedLinks = new([
                new LinkModel()
                {
                    From = "linkFrom",
                    To = "linkTo",
                }
            ]),
            CommandCosts = new([
                new CommandCostModel()
                {
                    Command = "cmdTest",
                    To = "toTest",
                    Cost = 20,
                }
            ]),
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.EnvironmentModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.EnvironmentModel);
        Assert.Equal(model.Tags.Count, deserialized?.Tags.Count);
        for (int i = 0; i < model.Tags.Count; i++)
        {
            Assert.Equal(model.Tags[i].Key, deserialized?.Tags[i].Key);
            Assert.Equal(model.Tags[i].Value, deserialized?.Tags[i].Value);
        }
        Assert.Equal(model.RoomConditions.Count, deserialized?.RoomConditions.Count);
        for (int i = 0; i < model.RoomConditions.Count; i++)
        {
            Assert.Equal(model.RoomConditions[i].Key, deserialized?.RoomConditions[i].Key);
            Assert.Equal(model.RoomConditions[i].Not, deserialized?.RoomConditions[i].Not);
            Assert.Equal(model.RoomConditions[i].Value, deserialized?.RoomConditions[i].Value);
        }
        Assert.Equal(model.Rooms.Count, deserialized?.Rooms.Count);
        for (int i = 0; i < model.Rooms.Count; i++)
        {
            Assert.Equal(model.Rooms[i].Key, deserialized?.Rooms[i].Key);
            Assert.Equal(model.Rooms[i].Desc, deserialized?.Rooms[i].Desc);
            Assert.Equal(model.Rooms[i].Group, deserialized?.Rooms[i].Group);
        }
        Assert.Equal(model.Paths.Count, deserialized?.Paths.Count);
        for (int i = 0; i < model.Paths.Count; i++)
        {
            Assert.Equal(model.Paths[i].From, deserialized?.Paths[i].From);
            Assert.Equal(model.Paths[i].To, deserialized?.Paths[i].To);
            Assert.Equal(model.Paths[i].Command, deserialized?.Paths[i].Command);
            Assert.Equal(model.Paths[i].Cost, deserialized?.Paths[i].Cost);
        }
        Assert.Equal(model.Shortcuts.Count, deserialized?.Shortcuts.Count);
        for (int i = 0; i < model.Shortcuts.Count; i++)
        {
            Assert.Equal(model.Shortcuts[i].Command, deserialized?.Shortcuts[i].Command);
            Assert.Equal(model.Shortcuts[i].To, deserialized?.Shortcuts[i].To);
            Assert.Equal(model.Shortcuts[i].Cost, deserialized?.Shortcuts[i].Cost);
            Assert.Equal(model.Shortcuts[i].RoomConditions.Count, deserialized?.Shortcuts[i].RoomConditions.Count);
            for (int j = 0; j < model.Shortcuts[i].RoomConditions.Count; j++)
            {
                Assert.Equal(model.Shortcuts[i].RoomConditions[j].Key, deserialized?.Shortcuts[i].RoomConditions[j].Key);
                Assert.Equal(model.Shortcuts[i].RoomConditions[j].Not, deserialized?.Shortcuts[i].RoomConditions[j].Not);
                Assert.Equal(model.Shortcuts[i].RoomConditions[j].Value, deserialized?.Shortcuts[i].RoomConditions[j].Value);
            }
        }
        Assert.Equal(model.Whitelist.Count, deserialized?.Whitelist.Count);
        for (int i = 0; i < model.Whitelist.Count; i++)
        {
            Assert.Equal(model.Whitelist[i], deserialized?.Whitelist[i]);
        }
        Assert.Equal(model.Blacklist.Count, deserialized?.Blacklist.Count);
        for (int i = 0; i < model.Blacklist.Count; i++)
        {
            Assert.Equal(model.Blacklist[i], deserialized?.Blacklist[i]);
        }
        Assert.Equal(model.BlockedLinks.Count, deserialized?.BlockedLinks.Count);
        for (int i = 0; i < model.BlockedLinks.Count; i++)
        {
            Assert.Equal(model.BlockedLinks[i].From, deserialized?.BlockedLinks[i].From);
            Assert.Equal(model.BlockedLinks[i].To, deserialized?.BlockedLinks[i].To);
        }
        Assert.Equal(model.CommandCosts.Count, deserialized?.CommandCosts.Count);
        for (int i = 0; i < model.CommandCosts.Count; i++)
        {
            Assert.Equal(model.CommandCosts[i].Command, deserialized?.CommandCosts[i].Command);
            Assert.Equal(model.CommandCosts[i].To, deserialized?.CommandCosts[i].To);
            Assert.Equal(model.CommandCosts[i].Cost, deserialized?.CommandCosts[i].Cost);
        }
        var emptyModel = new EnvironmentModel();
        var fromEmpty = EnvironmentModel.From(emptyModel.ToEnvironment());
        Assert.Empty(fromEmpty.Tags);
        Assert.Empty(fromEmpty.RoomConditions);
        Assert.Empty(fromEmpty.Rooms);
        Assert.Empty(fromEmpty.Paths);
        Assert.Empty(fromEmpty.Shortcuts);
        Assert.Empty(fromEmpty.Whitelist);
        Assert.Empty(fromEmpty.Blacklist);
        Assert.Empty(fromEmpty.BlockedLinks);
        Assert.Empty(fromEmpty.CommandCosts);
    }
    [Fact]
    public void MapperOptionsModelTest()
    {
        var model = new MapperOptionsModel()
        {
            MaxExitCost = 500,
            MaxTotalCost = 2000,
            DisableShortcuts = true,
            CommandWhitelist=["cmd1","cmd2"]
        };
        model.CommandWhitelist.Sort();
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.MapperOptionsModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.MapperOptionsModel);
        Assert.Equal(model.MaxExitCost, deserialized?.MaxExitCost);
        Assert.Equal(model.MaxTotalCost, deserialized?.MaxTotalCost);
        Assert.Equal(model.DisableShortcuts, deserialized?.DisableShortcuts);
        deserialized?.CommandWhitelist.Sort();
        Assert.Equal(model.CommandWhitelist, deserialized?.CommandWhitelist);
        var raw = model.ToMapperOptions();
        var fromRaw = MapperOptionsModel.From(raw);
        Assert.Equal(model.MaxExitCost, fromRaw.MaxExitCost);
        Assert.Equal(model.MaxTotalCost, fromRaw.MaxTotalCost);
        Assert.Equal(model.DisableShortcuts, fromRaw.DisableShortcuts);
        fromRaw.CommandWhitelist.Sort();
        Assert.Equal(model.CommandWhitelist, fromRaw.CommandWhitelist);
        var emptyModel = new MapperOptionsModel();
        var fromEmpty = MapperOptionsModel.From(emptyModel.ToMapperOptions());
        Assert.Equal(0, fromEmpty.MaxExitCost);
        Assert.Equal(0, fromEmpty.MaxTotalCost);
        Assert.False(fromEmpty.DisableShortcuts);
        Assert.Empty(fromEmpty.CommandWhitelist);
    }
    [Fact]
    public void InputQueryPathAnyTest()
    {
        var model = new InputQueryPathAny()
        {
            From = ["pointA", "pointB", "pointC"],
            Target = ["targetX", "targetY"],
            Environment = new EnvironmentModel(),
            Options = new MapperOptionsModel(),
        };
        Assert.Null(InputQueryPathAny.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputQueryPathAny);
        var deserialized = InputQueryPathAny.FromJSON(json);
        Assert.Equal(model.From.Count, deserialized?.From.Count);
        for (int i = 0; i < model.From.Count; i++)
        {
            Assert.Equal(model.From[i], deserialized?.From[i]);
        }
        Assert.Equal(model.Target.Count, deserialized?.Target.Count);
        for (int i = 0; i < model.Target.Count; i++)
        {
            Assert.Equal(model.Target[i], deserialized?.Target[i]);
        }
        Assert.Empty(model.Environment.Tags);
        Assert.Empty(model.Environment.RoomConditions);
        Assert.Empty(model.Environment.Rooms);
        Assert.Empty(model.Environment.Paths);
        Assert.Empty(model.Environment.Shortcuts);
        Assert.Empty(model.Environment.Whitelist);
        Assert.Empty(model.Environment.Blacklist);
        Assert.Empty(model.Environment.BlockedLinks);
        Assert.Empty(model.Environment.CommandCosts);
        Assert.Equal(0, model.Options.MaxExitCost);
        Assert.Equal(0, model.Options.MaxTotalCost);
        Assert.False(model.Options.DisableShortcuts);
    }
    [Fact]
    public void InputQueryPathTest()
    {
        var model = new InputQueryPath()
        {
            Start = "startPoint",
            Target = ["endPoint"],
            Environment = new EnvironmentModel(),
            Options = new MapperOptionsModel(),
        };
        Assert.Null(InputQueryPath.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputQueryPath);
        var deserialized = InputQueryPath.FromJSON(json);
        Assert.Equal(model.Start, deserialized?.Start);
        Assert.Equal(model.Target.Count, deserialized?.Target.Count);
        for (int i = 0; i < model.Target.Count; i++)
        {
            Assert.Equal(model.Target[i], deserialized?.Target[i]);
        }
        Assert.Empty(model.Environment.Tags);
        Assert.Empty(model.Environment.RoomConditions);
        Assert.Empty(model.Environment.Rooms);
        Assert.Empty(model.Environment.Paths);
        Assert.Empty(model.Environment.Shortcuts);
        Assert.Empty(model.Environment.Whitelist);
        Assert.Empty(model.Environment.Blacklist);
        Assert.Empty(model.Environment.BlockedLinks);
        Assert.Empty(model.Environment.CommandCosts);
        Assert.Equal(0, model.Options.MaxExitCost);
        Assert.Equal(0, model.Options.MaxTotalCost);
        Assert.False(model.Options.DisableShortcuts);
    }
    [Fact]
    public void InputDilateTest()
    {
        var model = new InputDilate()
        {
            Source = ["room1", "room2"],
            Iterations = 3,
            Environment = new EnvironmentModel(),
            Options = new MapperOptionsModel(),
        };
        Assert.Null(InputDilate.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputDilate);
        var deserialized = InputDilate.FromJSON(json);
        Assert.Equal(model.Source.Count, deserialized?.Source.Count);
        for (int i = 0; i < model.Source.Count; i++)
        {
            Assert.Equal(model.Source[i], deserialized?.Source[i]);
        }
        Assert.Equal(model.Iterations, deserialized?.Iterations);
        Assert.Empty(model.Environment.Tags);
        Assert.Empty(model.Environment.RoomConditions);
        Assert.Empty(model.Environment.Rooms);
        Assert.Empty(model.Environment.Paths);
        Assert.Empty(model.Environment.Shortcuts);
        Assert.Empty(model.Environment.Whitelist);
        Assert.Empty(model.Environment.Blacklist);
        Assert.Empty(model.Environment.BlockedLinks);
        Assert.Empty(model.Environment.CommandCosts);
        Assert.Equal(0, model.Options.MaxExitCost);
        Assert.Equal(0, model.Options.MaxTotalCost);
        Assert.False(model.Options.DisableShortcuts);
    }
    [Fact]
    public void InputTrackExitTest()
    {
        var model = new InputTrackExit()
        {
            Start = "startRoom",
            Command = "exitCommand",
            Environment = new EnvironmentModel(),
            Options = new MapperOptionsModel(),
        };
        Assert.Null(InputTrackExit.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputTrackExit);
        var deserialized = InputTrackExit.FromJSON(json);
        Assert.Equal(model.Start, deserialized?.Start);
        Assert.Equal(model.Command, deserialized?.Command);
        Assert.Empty(model.Environment.Tags);
        Assert.Empty(model.Environment.RoomConditions);
        Assert.Empty(model.Environment.Rooms);
        Assert.Empty(model.Environment.Paths);
        Assert.Empty(model.Environment.Shortcuts);
        Assert.Empty(model.Environment.Whitelist);
        Assert.Empty(model.Environment.Blacklist);
        Assert.Empty(model.Environment.BlockedLinks);
        Assert.Empty(model.Environment.CommandCosts);
        Assert.Equal(0, model.Options.MaxExitCost);
        Assert.Equal(0, model.Options.MaxTotalCost);
        Assert.False(model.Options.DisableShortcuts);
    }
    [Fact]
    public void InputKeyTest()
    {
        var model = new InputKey()
        {
            Key = "testKeyValue"
        };
        Assert.Null(InputKey.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputKey);
        var deserialized = InputKey.FromJSON(json);
        Assert.Equal(model.Key, deserialized?.Key);
    }
    [Fact]
    public void InputGetRoomTest()
    {
        var model = new InputGetRoom()
        {
            Key = "roomKeyValue",
            Environment = new(),
            Options = new(),
        };
        Assert.Null(InputGetRoom.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputGetRoom);
        var deserialized = InputGetRoom.FromJSON(json);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Empty(model.Environment.Tags);
        Assert.Empty(model.Environment.RoomConditions);
        Assert.Empty(model.Environment.Rooms);
        Assert.Empty(model.Environment.Paths);
        Assert.Empty(model.Environment.Shortcuts);
        Assert.Empty(model.Environment.Whitelist);
        Assert.Empty(model.Environment.Blacklist);
        Assert.Empty(model.Environment.BlockedLinks);
        Assert.Empty(model.Environment.CommandCosts);
        Assert.Equal(0, model.Options.MaxExitCost);
        Assert.Equal(0, model.Options.MaxTotalCost);
        Assert.False(model.Options.DisableShortcuts);
    }
    [Fact]
    public void InputSnapshotFilterTest()
    {
        var model = new InputSnapshotFilter()
        {
            Key = "filterKey",
            Type = "filterType",
            Group = "filterGroup",
            MaxCount = 10,
        };
        Assert.Null(InputSnapshotFilter.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputSnapshotFilter);
        var deserialized = InputSnapshotFilter.FromJSON(json);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Type, deserialized?.Type);
        Assert.Equal(model.Group, deserialized?.Group);
        Assert.Equal(model.MaxCount, deserialized?.MaxCount);
        var raw = model.ToSnapshotFilter();
        var fromRaw = InputSnapshotFilter.From(raw); ;
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Type, fromRaw.Type);
        Assert.Equal(model.Group, fromRaw.Group);
        Assert.Equal(model.MaxCount, fromRaw.MaxCount);
    }

    [Fact]
    public void InputTakeSnapshotTest()
    {
        var model = new InputTakeSnapshot()
        {
            Key = "snapshotKey",
            Group = "snapshotGroup",
            Type = "snapshotType",
            Value = "snapshotValue",
        };
        Assert.Null(InputTakeSnapshot.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputTakeSnapshot);
        var deserialized = InputTakeSnapshot.FromJSON(json);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Group, deserialized?.Group);
        Assert.Equal(model.Type, deserialized?.Type);
        Assert.Equal(model.Value, deserialized?.Value);
    }
    [Fact]
    public void InputSnapshotSearchModel()
    {
        var model = new SnapshotSearchModel()
        {
            Group = "searchGroup",
            Type = "searchType",
            Keywords = ["key1", "key2", "key3"],
            PartialMatch = true,
            Any = true,
        };
        Assert.Null(SnapshotSearchModel.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.SnapshotSearchModel);
        var deserialized = SnapshotSearchModel.FromJSON(json);
        Assert.Equal(model.Group, deserialized?.Group);
        Assert.Equal(model.Type, deserialized?.Type);
        Assert.Equal(model.Keywords.Count, deserialized?.Keywords.Count);
        for (int i = 0; i < model.Keywords.Count; i++)
        {
            Assert.Equal(model.Keywords[i], deserialized?.Keywords[i]);
        }
        Assert.Equal(model.PartialMatch, deserialized?.PartialMatch);
        Assert.Equal(model.Any, deserialized?.Any);
        var raw = model.ToSnapshotSearch();
        var fromRaw = SnapshotSearchModel.From(raw);
        Assert.Equal(model.Group, fromRaw.Group);
        Assert.Equal(model.Type, fromRaw.Type);
        Assert.Equal(model.Keywords.Count, fromRaw.Keywords.Count);
        for (int i = 0; i < model.Keywords.Count; i++)
        {
            Assert.Equal(model.Keywords[i], fromRaw.Keywords[i]);
        }
    }
    [Fact]
    public void SnapshotSearchResultModelTest()
    {
        var model = new SnapshotSearchResultModel()
        {
            Key = "resultKey",
            Sum = 42,
            Count = 5,
            Items = new([
                new SnapshotModel()
                {
                    Key = "itemKey",
                    Timestamp = 170000000,
                    Group = "itemGroup",
                    Type = "itemType",
                    Count = 5,
                    Value = "itemValue",
                }
            ])
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.SnapshotSearchResultModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.SnapshotSearchResultModel);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Sum, deserialized?.Sum);
        Assert.Equal(model.Count, deserialized?.Count);
        Assert.Equal(model.Items.Count, deserialized?.Items.Count);
        for (int i = 0; i < model.Items.Count; i++)
        {
            Assert.Equal(model.Items[i].Key, deserialized?.Items[i].Key);
            Assert.Equal(model.Items[i].Timestamp, deserialized?.Items[i].Timestamp);
            Assert.Equal(model.Items[i].Group, deserialized?.Items[i].Group);
            Assert.Equal(model.Items[i].Type, deserialized?.Items[i].Type);
            Assert.Equal(model.Items[i].Count, deserialized?.Items[i].Count);
            Assert.Equal(model.Items[i].Value, deserialized?.Items[i].Value);
        }
        var raw = model.ToResult();
        var fromRaw = SnapshotSearchResultModel.From(raw);
        Assert.Equal(model.Key, fromRaw.Key);
        Assert.Equal(model.Sum, fromRaw.Sum);
        Assert.Equal(model.Count, fromRaw.Count);
        Assert.Equal(model.Items.Count, fromRaw.Items.Count);
        for (int i = 0; i < model.Items.Count; i++)
        {
            Assert.Equal(model.Items[i].Key, fromRaw.Items[i].Key);
            Assert.Equal(model.Items[i].Timestamp, fromRaw.Items[i].Timestamp);
            Assert.Equal(model.Items[i].Group, fromRaw.Items[i].Group);
            Assert.Equal(model.Items[i].Type, fromRaw.Items[i].Type);
            Assert.Equal(model.Items[i].Count, fromRaw.Items[i].Count);
            Assert.Equal(model.Items[i].Value, fromRaw.Items[i].Value);
        }
        var list = SnapshotSearchResultModel.FromList(SnapshotSearchResultModel.ToResultList(new List<SnapshotSearchResultModel>() { model }));
        var listJson = JsonSerializer.Serialize(list, APIJsonSerializerContext.Default.ListSnapshotSearchResultModel);
        var deserializedList = JsonSerializer.Deserialize(listJson, APIJsonSerializerContext.Default.ListSnapshotSearchResultModel);
        Assert.Single(deserializedList!);
        Assert.Equal(model.Key, deserializedList![0].Key);
        Assert.Equal(model.Sum, deserializedList![0].Sum);
        Assert.Equal(model.Count, deserializedList![0].Count);
        Assert.Equal(model.Items.Count, deserializedList![0].Items.Count);
        for (int i = 0; i < model.Items.Count; i++)
        {
            Assert.Equal(model.Items[i].Key, deserializedList![0].Items[i].Key);
            Assert.Equal(model.Items[i].Timestamp, deserializedList![0].Items[i].Timestamp);
            Assert.Equal(model.Items[i].Group, deserializedList![0].Items[i].Group);
            Assert.Equal(model.Items[i].Type, deserializedList![0].Items[i].Type);
            Assert.Equal(model.Items[i].Count, deserializedList![0].Items[i].Count);
            Assert.Equal(model.Items[i].Value, deserializedList![0].Items[i].Value);
        }
    }
    [Fact]
    public void RoomFilterModelTest()
    {
        var model = new RoomFilterModel()
        {
            RoomConditions = new([
                new ValueConditionModel()
                {
                    Key = "condKey",
                    Not = false,
                    Value = 7
                }
            ]),
            HasAnyExitTo = ["roomA", "roomB"],
            HasAnyData = new([new DataModel()
            {
                Key = "dataKey",
                Value = "dataValue",
            }]),
            HasAnyName = ["name1", "name2"],
            HasAnyGroup = ["group1", "group2"],
            ContainsAnyData = new([new DataModel()
            {
                Key = "dataKey2",
                Value = "dataValue2",
            }]),
            ContainsAnyName = ["name3", "name4"],
            ContainsAnyKey = ["key1", "key2"],
        };
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.RoomFilterModel);
        var deserialized = JsonSerializer.Deserialize(json, APIJsonSerializerContext.Default.RoomFilterModel);
        Assert.Equal(model.RoomConditions.Count, deserialized?.RoomConditions.Count);
        for (int i = 0; i < model.RoomConditions.Count; i++)
        {
            Assert.Equal(model.RoomConditions[i].Key, deserialized?.RoomConditions[i].Key);
            Assert.Equal(model.RoomConditions[i].Not, deserialized?.RoomConditions[i].Not);
            Assert.Equal(model.RoomConditions[i].Value, deserialized?.RoomConditions[i].Value);
        }
        Assert.Equal(model.HasAnyExitTo.Count, deserialized?.HasAnyExitTo.Count);
        for (int i = 0; i < model.HasAnyExitTo.Count; i++)
        {
            Assert.Equal(model.HasAnyExitTo[i], deserialized?.HasAnyExitTo[i]);
        }
        Assert.Equal(model.HasAnyData.Count, deserialized?.HasAnyData.Count);
        for (int i = 0; i < model.HasAnyData.Count; i++)
        {
            Assert.Equal(model.HasAnyData[i].Key, deserialized?.HasAnyData[i].Key);
            Assert.Equal(model.HasAnyData[i].Value, deserialized?.HasAnyData[i].Value);
        }
        Assert.Equal(model.HasAnyName.Count, deserialized?.HasAnyName.Count);
        for (int i = 0; i < model.HasAnyName.Count; i++)
        {
            Assert.Equal(model.HasAnyName[i], deserialized?.HasAnyName[i]);
        }
        Assert.Equal(model.HasAnyGroup.Count, deserialized?.HasAnyGroup.Count);
        for (int i = 0; i < model.HasAnyGroup.Count; i++)
        {
            Assert.Equal(model.HasAnyGroup[i], deserialized?.HasAnyGroup[i]);
        }
        Assert.Equal(model.ContainsAnyData.Count, deserialized?.ContainsAnyData.Count);
        for (int i = 0; i < model.ContainsAnyData.Count; i++)
        {
            Assert.Equal(model.ContainsAnyData[i].Key, deserialized?.ContainsAnyData[i].Key);
            Assert.Equal(model.ContainsAnyData[i].Value, deserialized?.ContainsAnyData[i].Value);
        }
        Assert.Equal(model.ContainsAnyName.Count, deserialized?.ContainsAnyName.Count);
        for (int i = 0; i < model.ContainsAnyName.Count; i++)
        {
            Assert.Equal(model.ContainsAnyName[i], deserialized?.ContainsAnyName[i]);
        }
        Assert.Equal(model.ContainsAnyKey.Count, deserialized?.ContainsAnyKey.Count);
        for (int i = 0; i < model.ContainsAnyKey.Count; i++)
        {
            Assert.Equal(model.ContainsAnyKey[i], deserialized?.ContainsAnyKey[i]);
        }
        var raw = model.ToRoomFilter();
        var fromRaw = RoomFilterModel.From(raw);
        Assert.Equal(model.RoomConditions.Count, fromRaw.RoomConditions.Count);
        for (int i = 0; i < model.RoomConditions.Count; i++)
        {
            Assert.Equal(model.RoomConditions[i].Key, fromRaw.RoomConditions[i].Key);
            Assert.Equal(model.RoomConditions[i].Not, fromRaw.RoomConditions[i].Not);
            Assert.Equal(model.RoomConditions[i].Value, fromRaw.RoomConditions[i].Value);
        }
        Assert.Equal(model.HasAnyExitTo.Count, fromRaw.HasAnyExitTo.Count);
        for (int i = 0; i < model.HasAnyExitTo.Count; i++)
        {
            Assert.Equal(model.HasAnyExitTo[i], fromRaw.HasAnyExitTo[i]);
        }
        Assert.Equal(model.HasAnyData.Count, fromRaw.HasAnyData.Count);
        for (int i = 0; i < model.HasAnyData.Count; i++)
        {
            Assert.Equal(model.HasAnyData[i].Key, fromRaw.HasAnyData[i].Key);
            Assert.Equal(model.HasAnyData[i].Value, fromRaw.HasAnyData[i].Value);
        }
        Assert.Equal(model.HasAnyName.Count, fromRaw.HasAnyName.Count);
        for (int i = 0; i < model.HasAnyName.Count; i++)
        {
            Assert.Equal(model.HasAnyName[i], fromRaw.HasAnyName[i]);
        }
        Assert.Equal(model.HasAnyGroup.Count, fromRaw.HasAnyGroup.Count);
        for (int i = 0; i < model.HasAnyGroup.Count; i++)
        {
            Assert.Equal(model.HasAnyGroup[i], fromRaw.HasAnyGroup[i]);
        }
        Assert.Equal(model.ContainsAnyData.Count, fromRaw.ContainsAnyData.Count);
        for (int i = 0; i < model.ContainsAnyData.Count; i++)
        {
            Assert.Equal(model.ContainsAnyData[i].Key, fromRaw.ContainsAnyData[i].Key);
            Assert.Equal(model.ContainsAnyData[i].Value, fromRaw.ContainsAnyData[i].Value);
        }
        Assert.Equal(model.ContainsAnyName.Count, fromRaw.ContainsAnyName.Count);
        for (int i = 0; i < model.ContainsAnyName.Count; i++)
        {
            Assert.Equal(model.ContainsAnyName[i], fromRaw.ContainsAnyName[i]);
        }
        Assert.Equal(model.ContainsAnyKey.Count, fromRaw.ContainsAnyKey.Count);
        for (int i = 0; i < model.ContainsAnyKey.Count; i++)
        {
            Assert.Equal(model.ContainsAnyKey[i], fromRaw.ContainsAnyKey[i]);
        }
    }
    [Fact]
    public void InputSearchRoomsTest()
    {
        var model = new InputSearchRooms()
        {
            Filter = new RoomFilterModel()
            {
                RoomConditions = new([
                new ValueConditionModel()
                {
                    Key = "condKey",
                    Not = false,
                    Value = 7
                }
            ]),
                HasAnyExitTo = ["roomA", "roomB"],
                HasAnyData = new([new DataModel()
            {
                Key = "dataKey",
                Value = "dataValue",
            }]),
                HasAnyName = ["name1", "name2"],
                HasAnyGroup = ["group1", "group2"],
                ContainsAnyData = new([new DataModel()
            {
                Key = "dataKey2",
                Value = "dataValue2",
            }]),
                ContainsAnyName = ["name3", "name4"],
                ContainsAnyKey = ["key1", "key2"],
            },
        };
        Assert.Null(InputSearchRooms.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputSearchRooms);
        var deserialized = InputSearchRooms.FromJSON(json);
        Assert.NotNull(deserialized?.Filter);
        var filter = deserialized!.Filter;
        Assert.Equal(model.Filter.RoomConditions.Count, filter.RoomConditions.Count);
        for (int i = 0; i < model.Filter.RoomConditions.Count; i++)
        {
            Assert.Equal(model.Filter.RoomConditions[i].Key, filter.RoomConditions[i].Key);
            Assert.Equal(model.Filter.RoomConditions[i].Not, filter.RoomConditions[i].Not);
            Assert.Equal(model.Filter.RoomConditions[i].Value, filter.RoomConditions[i].Value);
        }
        Assert.Equal(model.Filter.HasAnyExitTo.Count, filter.HasAnyExitTo.Count);
        for (int i = 0; i < model.Filter.HasAnyExitTo.Count; i++)
        {
            Assert.Equal(model.Filter.HasAnyExitTo[i], filter.HasAnyExitTo[i]);
        }
        Assert.Equal(model.Filter.HasAnyData.Count, filter.HasAnyData.Count);
        for (int i = 0; i < model.Filter.HasAnyData.Count; i++)
        {
            Assert.Equal(model.Filter.HasAnyData[i].Key, filter.HasAnyData[i].Key);
            Assert.Equal(model.Filter.HasAnyData[i].Value, filter.HasAnyData[i].Value);
        }
        Assert.Equal(model.Filter.HasAnyName.Count, filter.HasAnyName.Count);
        for (int i = 0; i < model.Filter.HasAnyName.Count; i++)
        {
            Assert.Equal(model.Filter.HasAnyName[i], filter.HasAnyName[i]);
        }
        Assert.Equal(model.Filter.HasAnyGroup.Count, filter.HasAnyGroup.Count);
        for (int i = 0; i < model.Filter.HasAnyGroup.Count; i++)
        {
            Assert.Equal(model.Filter.HasAnyGroup[i], filter.HasAnyGroup[i]);
        }
        Assert.Equal(model.Filter.ContainsAnyData.Count, filter.ContainsAnyData.Count);
        for (int i = 0; i < model.Filter.ContainsAnyData.Count; i++)
        {
            Assert.Equal(model.Filter.ContainsAnyData[i].Key, filter.ContainsAnyData[i].Key);
            Assert.Equal(model.Filter.ContainsAnyData[i].Value, filter.ContainsAnyData[i].Value);
        }
        Assert.Equal(model.Filter.ContainsAnyName.Count, filter.ContainsAnyName.Count);
        for (int i = 0; i < model.Filter.ContainsAnyName.Count; i++)
        {
            Assert.Equal(model.Filter.ContainsAnyName[i], filter.ContainsAnyName[i]);
        }
        Assert.Equal(model.Filter.ContainsAnyKey.Count, filter.ContainsAnyKey.Count);
        for (int i = 0; i < model.Filter.ContainsAnyKey.Count; i++)
        {
            Assert.Equal(model.Filter.ContainsAnyKey[i], filter.ContainsAnyKey[i]);
        }
    }
    [Fact]
    public void InputFilterRoomsTest()
    {
        var model = new InputFilterRooms()
        {
            Source = ["room1", "room2", "room3"],
            Filter = new RoomFilterModel()
            {
                RoomConditions = new([
                new ValueConditionModel()
                {
                    Key = "condKey",
                    Not = false,
                    Value = 7
                }
            ]),
                HasAnyExitTo = ["roomA", "roomB"],
                HasAnyData = new([new DataModel()
            {
                Key = "dataKey",
                Value = "dataValue",
            }]),
                HasAnyName = ["name1", "name2"],
                HasAnyGroup = ["group1", "group2"],
                ContainsAnyData = new([new DataModel()
            {
                Key = "dataKey2",
                Value = "dataValue2",
            }]),
                ContainsAnyName = ["name3", "name4"],
                ContainsAnyKey = ["key1", "key2"],
            },
        };
        Assert.Null(InputFilterRooms.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputFilterRooms);
        var deserialized = InputFilterRooms.FromJSON(json);
        Assert.NotNull(deserialized?.Filter);
        var filter = deserialized!.Filter;
        Assert.Equal(model.Source.Count, deserialized.Source.Count);
        for (int i = 0; i < model.Source.Count; i++)
        {
            Assert.Equal(model.Source[i], deserialized.Source[i]);
        }
        Assert.Equal(model.Filter.RoomConditions.Count, filter.RoomConditions.Count);
        for (int i = 0; i < model.Filter.RoomConditions.Count; i++)
        {
            Assert.Equal(model.Filter.RoomConditions[i].Key, filter.RoomConditions[i].Key);
            Assert.Equal(model.Filter.RoomConditions[i].Not, filter.RoomConditions[i].Not);
            Assert.Equal(model.Filter.RoomConditions[i].Value, filter.RoomConditions[i].Value);
        }
        Assert.Equal(model.Filter.HasAnyExitTo.Count, filter.HasAnyExitTo.Count);
        for (int i = 0; i < model.Filter.HasAnyExitTo.Count; i++)
        {
            Assert.Equal(model.Filter.HasAnyExitTo[i], filter.HasAnyExitTo[i]);
        }
        Assert.Equal(model.Filter.HasAnyData.Count, filter.HasAnyData.Count);
        for (int i = 0; i < model.Filter.HasAnyData.Count; i++)
        {
            Assert.Equal(model.Filter.HasAnyData[i].Key, filter.HasAnyData[i].Key);
            Assert.Equal(model.Filter.HasAnyData[i].Value, filter.HasAnyData[i].Value);
        }
        Assert.Equal(model.Filter.HasAnyName.Count, filter.HasAnyName.Count);
        for (int i = 0; i < model.Filter.HasAnyName.Count; i++)
        {
            Assert.Equal(model.Filter.HasAnyName[i], filter.HasAnyName[i]);
        }
        Assert.Equal(model.Filter.HasAnyGroup.Count, filter.HasAnyGroup.Count);
        for (int i = 0; i < model.Filter.HasAnyGroup.Count; i++)
        {
            Assert.Equal(model.Filter.HasAnyGroup[i], filter.HasAnyGroup[i]);
        }
        Assert.Equal(model.Filter.ContainsAnyData.Count, filter.ContainsAnyData.Count);
        for (int i = 0; i < model.Filter.ContainsAnyData.Count; i++)
        {
            Assert.Equal(model.Filter.ContainsAnyData[i].Key, filter.ContainsAnyData[i].Key);
            Assert.Equal(model.Filter.ContainsAnyData[i].Value, filter.ContainsAnyData[i].Value);
        }
        Assert.Equal(model.Filter.ContainsAnyName.Count, filter.ContainsAnyName.Count);
        for (int i = 0; i < model.Filter.ContainsAnyName.Count; i++)
        {
            Assert.Equal(model.Filter.ContainsAnyName[i], filter.ContainsAnyName[i]);
        }
        Assert.Equal(model.Filter.ContainsAnyKey.Count, filter.ContainsAnyKey.Count);
        for (int i = 0; i < model.Filter.ContainsAnyKey.Count; i++)
        {
            Assert.Equal(model.Filter.ContainsAnyKey[i], filter.ContainsAnyKey[i]);
        }
    }
    [Fact]
    public void InputGroupRoomTest()
    {
        var model = new InputGroupRoom()
        {
            Room = "roomKey",
            Group = "roomGroup",
        };
        Assert.Null(InputGroupRoom.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputGroupRoom);
        var deserialized = InputGroupRoom.FromJSON(json);
        Assert.Equal(model.Room, deserialized?.Room);
        Assert.Equal(model.Group, deserialized?.Group);
    }
    [Fact]
    public void InputTagRoomTest()
    {
        var model = new InputTagRoom()
        {
            Room = "roomKey",
            Tag = "roomTag",
            Value = 1,
        };
        Assert.Null(InputTagRoom.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputTagRoom);
        var deserialized = InputTagRoom.FromJSON(json);
        Assert.Equal(model.Room, deserialized?.Room);
        Assert.Equal(model.Tag, deserialized?.Tag);
        Assert.Equal(model.Value, deserialized?.Value);
    }
    [Fact]
    public void InputSetRoomDataTest()
    {
        var model = new InputSetRoomData()
        {
            Room = "roomKey",
            Key = "dataKey",
            Value = "dataValue",
        };
        Assert.Null(InputSetRoomData.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputSetRoomData);
        var deserialized = InputSetRoomData.FromJSON(json);
        Assert.Equal(model.Room, deserialized?.Room);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Value, deserialized?.Value);
    }
    [Fact]
    public void InputTraceLocationTest()
    {
        var model = new InputTraceLocation()
        {
            Key = "traceKey",
            Location = "traceLocation",
        };
        Assert.Null(InputTraceLocation.FromJSON("wrong json"));
        var json = JsonSerializer.Serialize(model, APIJsonSerializerContext.Default.InputTraceLocation);
        var deserialized = InputTraceLocation.FromJSON(json);
        Assert.Equal(model.Key, deserialized?.Key);
        Assert.Equal(model.Location, deserialized?.Location);
    }
}
