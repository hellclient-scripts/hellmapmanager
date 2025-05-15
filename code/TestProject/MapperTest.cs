using HellMapManager.Models;
using HellMapManager.Helpers;
using HellMapManager.Cores;
using Avalonia.Diagnostics;
using Avalonia.Remote.Protocol.Viewport;
using System.Net.Mail;
using Tmds.DBus.Protocol;
using Avalonia.Platform;

namespace TestProject;

public class MapperTest()
{
    [Fact]
    public void TestGetRoom()
    {
        var md = new MapDatabase();
        md.NewMap();
        var room = new Room()
        {
            Key = "key1",
        };
        var room2 = new Room()
        {
            Key = "key2",
        };
        var room3 = new Room()
        {
            Key = "key2",
        };
        var room4 = new Room()
        {
            Key = "key3",
        };
        md.APIInsertRooms([room, room2]);
        var ctx = new Context().WithRooms([
            room3,room4
        ]);
        var mapper = new Mapper(md.Current!, ctx, new MapperOptions());

        Assert.Null(mapper.GetRoom("keynotfound"));
        Assert.Equal(room, mapper.GetRoom("key1"));
        Assert.Equal(room3, mapper.GetRoom("key2"));
        Assert.Equal(room4, mapper.GetRoom("key3"));
    }
    [Fact]
    public void TestGetExitCost()
    {
        var md = new MapDatabase();
        md.NewMap();
        var ctx = new Context();
        ctx.WithCommandCosts([new CommandCost(){
            Command = "cmd2",
            To="to1",
            Cost = 20,
        },
        new CommandCost(){
            Command = "cmd2",
            To="to2",
            Cost = 20,
        },
        ]);
        var mapper = new Mapper(md.Current!, ctx, new MapperOptions());
        var exit = new Exit()
        {
            To = "to1",
            Command = "cmd1",
            Cost = 10,
        };
        var exit2 = new Exit()
        {
            To = "to1",
            Command = "cmd2",
            Cost = 10,
        };
        var exit3 = new Exit()
        {
            To = "to3",
            Command = "cmd2",
            Cost = 10,
        };

        Assert.Equal(10, mapper.GetExitCost(exit));
        Assert.Equal(20, mapper.GetExitCost(exit2));
        Assert.Equal(10, mapper.GetExitCost(exit3));
    }
    [Fact]
    public void TestValidateRoom()
    {
        var md = new MapDatabase();
        md.NewMap();
        var ctx = new Context();
        var room = new Room()
        {
            Key = "key1",
            Name = "name1",
            Desc = "desc1",
            Tags = [
                new ValueTag("tag1",10)
            ],
            Group = "group1",
        };
        var mapper = new Mapper(md.Current!, ctx, new MapperOptions());
        Assert.True(mapper.ValidateRoom(room));
        ctx.WithBlacklist(["key1"]);
        Assert.False(mapper.ValidateRoom(room));
        ctx.ClearBlacklist();
        ctx.WithWhitelist(["key2", "key3"]);
        Assert.False(mapper.ValidateRoom(room));
        ctx.WithWhitelist(["key1", "key3"]);
        Assert.True(mapper.ValidateRoom(room));
        ctx.ClearWhitelist();
        ctx.WithRoomConditions([new ValueCondition("tag1", 11, false)]);
        Assert.False(mapper.ValidateRoom(room));
        ctx.ClearRoomConditions();
        ctx.WithRoomConditions([new ValueCondition("tag1", 11, true)]);
        Assert.True(mapper.ValidateRoom(room));
        ctx.WithRoomConditions([new ValueCondition("tag1", 5, false)]);
        Assert.True(mapper.ValidateRoom(room));
    }
    [Fact]
    public void TestValidateExit()
    {
        var md = new MapDatabase();
        md.NewMap();
        var ctx = new Context();
        var room = new Room()
        {
            Key = "key2",
            Name = "name1",
            Desc = "desc1",
            Tags = [
                new ValueTag("tag1",10)
            ],
            Group = "group1",
        };
        md.APIInsertRooms([room]);
        var exit = new Exit()
        {
            To = "key2",
            Command = "cmd1",
            Conditions = [
                new ValueCondition("etag1", 1, true)
            ],
            Cost = 10,
        };
        var opt = new MapperOptions();
        var mapper = new Mapper(md.Current!, ctx, opt);
        Assert.True(mapper.ValidateExit("key1", exit, 10));
        var exit2 = exit.Clone();
        exit2.To = "notfound";
        Assert.False(mapper.ValidateExit("key1", exit2, 10));
        ctx.WithBlacklist(["key2"]);
        Assert.False(mapper.ValidateExit("key1", exit, 10));
        ctx.ClearBlacklist();
        ctx.WithBlockedLinks([new Link("key1", "key2")]);
        Assert.False(mapper.ValidateExit("key1", exit, 10));
        ctx.ClearBlockedLinks();
        ctx.WithBlockedLinks([new Link("key2", "key1")]);
        Assert.True(mapper.ValidateExit("key1", exit, 10));
        ctx.ClearBlockedLinks();
        Assert.True(mapper.ValidateExit("key1", exit, 10));
        opt.MaxExitCost = 5;
        Assert.False(mapper.ValidateExit("key1", exit, 10));
        opt.MaxExitCost = 0;
        Assert.True(mapper.ValidateExit("key1", exit, 10));
        opt.MaxTotalCost = 5;
        Assert.False(mapper.ValidateExit("key1", exit, 10));
        opt.MaxTotalCost = 0;
        Assert.True(mapper.ValidateExit("key1", exit, 10));
        ctx.WithTags([new ValueTag("etag1", 5)]);
        Assert.False(mapper.ValidateExit("key1", exit, 10));
        ctx.ClearTags();
        Assert.True(mapper.ValidatePath("key1", exit));
        ctx.WithBlockedLinks([new("key1", "key2")]);
        Assert.False(mapper.ValidatePath("key1", exit));
        ctx.ClearBlockedLinks();
        opt.MaxExitCost = 20;
        Assert.True(mapper.ValidatePath("key1", exit));
        ctx.WithCommandCosts([new CommandCost()
        {
            Command = "cmd1",
            To = "key2",
            Cost = 50,
        }]);
        Assert.False(mapper.ValidatePath("key1", exit));
    }
    [Fact]
    public void TestWalkingStep()
    {
        var exit = new Exit()
        {
            To = "key2",
            Command = "cmd1",
            Conditions = [
                new ValueCondition("etag1", 1, true)
            ],
            Cost = 150,
        };
        var ws2 = new WalkingStep();
        Assert.Null(ws2.Prev);
        Assert.Equal("", ws2.From);
        Assert.Equal("", ws2.To);
        Assert.Equal("", ws2.Command);
        Assert.Equal(0, ws2.Cost);
        Assert.Equal(0, ws2.TotalCost);
        Assert.Equal(0, ws2.Remain);


        var ws = WalkingStep.FromExit(null, "key1", exit, 10, 20);
        Assert.NotNull(ws);
        Assert.Null(ws2.Prev);
        Assert.Equal("key1", ws.From);
        Assert.Equal("key2", ws.To);
        Assert.Equal("cmd1", ws.Command);
        Assert.Equal(10, ws.Cost);
        Assert.Equal(30, ws.TotalCost);
        Assert.Equal(9, ws.Remain);

        exit = new Exit()
        {
            To = "key3",
            Command = "cmd2",
            Cost = 20,
        };
        var ws3 = WalkingStep.FromExit(ws, "key2", exit, 12, 20);
        Assert.NotNull(ws);
        Assert.Equal(ws, ws3.Prev);
        Assert.Equal("key2", ws3.From);
        Assert.Equal("key3", ws3.To);
        Assert.Equal("cmd2", ws3.Command);
        Assert.Equal(12, ws3.Cost);
        Assert.Equal(32, ws3.TotalCost);
        Assert.Equal(11, ws3.Remain);
        var step = ws3.ToStep();
        Assert.Equal("cmd2", step.Command);
        Assert.Equal("key3", step.Target);
        Assert.Equal(12, step.Cost);
    }
    [Fact]
    public void TestGetRoomExits()
    {
        var room = new Room()
        {
            Key = "key1",
            Tags = [
                new ValueTag("tag1", 10)
            ],
            Exits = [
                new Exit()
                {
                    To = "key2",
                    Command = "cmd1",
                    Conditions = [
                        new ValueCondition("etag1", 1, true)
                    ],
                    Cost = 10,
                },
                new Exit()
                {
                    To = "key3",
                    Command = "cmd2",
                    Cost = 20,
                },
            ],
        };
        var md = new MapDatabase();
        md.NewMap();
        var ctx = new Context();
        var opt = new MapperOptions();
        var mapper = new Mapper(md.Current!, ctx, opt);
        var exits = mapper.GetRoomExits(room);
        Assert.Equal(2, exits.Count);
        md.APIInsertShortcuts([new Shortcut()
        {
            Key="shortcut1",
            Command="cmd1",
            To="key3",
            Cost=1,
        },
        ]);
        exits = mapper.GetRoomExits(room);
        Assert.Equal(3, exits.Count);
        md.APIInsertShortcuts([new Shortcut()
        {
            Key="shortcut2",
            Command="cmd2",
            To="key4",
            RoomConditions=[
                new ValueCondition("tag1", 1, true)
            ],
            Cost=1,
        },
        ]);
        exits = mapper.GetRoomExits(room);
        Assert.Equal(3, exits.Count);
        ctx.WithShortcuts([
            new RoomConditionExit(){
                To="key4",
                Command="cmd2",
                RoomConditions=[
                    new ValueCondition("tag1", 1, false)
                ],
                Cost=1,
            },
        ]);
        exits = mapper.GetRoomExits(room);
        Assert.Equal(4, exits.Count);
        ctx.WithShortcuts([
                new RoomConditionExit(){
                To="key5",
                Command="cmd3",
                RoomConditions=[
                    new ValueCondition("tag1", 99, false)
                ],
                Cost=1,
            },
        ]);
        exits = mapper.GetRoomExits(room);
        Assert.Equal(4, exits.Count);
        opt.WithDisableShortcuts(true);
        exits = mapper.GetRoomExits(room);
        Assert.Equal(2, exits.Count);
        ctx.WithPaths([new HellMapManager.Models.Path(){
            From="key1",
            To="key6",
            Command="cmd4",
            Cost=10,
        }]);
        exits = mapper.GetRoomExits(room);
        Assert.Equal(3, exits.Count);
    }
    [Fact]
    public void TestValidateToWalkingStep()
    {
        var md = new MapDatabase();
        md.NewMap();
        var ctx = new Context();
        var opt = new MapperOptions();
        var mapper = new Mapper(md.Current!, ctx, opt);
        md.APIInsertRooms([new Room()
        {
            Key = "key2",
            Tags = [
                new ValueTag("etag1", 1)
            ],
        }]);
        var exit = new Exit()
        {
            To = "key2",
            Command = "cmd1",
            Conditions = [
                new ValueCondition("etag1", 1, true)
            ],
            Cost = 10,
        };
        var wsprev = new WalkingStep()
        {
        };
        var ws = mapper.ValidateToWalkingStep(wsprev, "key1", exit, 10);
        Assert.NotNull(ws);
        Assert.Equal(wsprev, ws.Prev);
        Assert.Equal("key1", ws.From);
        Assert.Equal("key2", ws.To);
        Assert.Equal("cmd1", ws.Command);
        Assert.Equal(10, ws.Cost);
        Assert.Equal(20, ws.TotalCost);
        Assert.Equal(9, ws.Remain);
        var exit2 = exit.Clone();
        ws = mapper.ValidateToWalkingStep(wsprev, "key1", exit2, 10);
        Assert.NotNull(ws);
        exit2.To = "";
        ws = mapper.ValidateToWalkingStep(wsprev, "key1", exit2, 10);
        Assert.Null(ws);
        exit2 = exit.Clone();
        ws = mapper.ValidateToWalkingStep(wsprev, "key1", exit2, 10);
        Assert.NotNull(ws);
        exit2.To = "key1";
        ws = mapper.ValidateToWalkingStep(wsprev, "key1", exit2, 10);
        Assert.Null(ws);
        ws = mapper.ValidateToWalkingStep(wsprev, "key1", exit, 10);
        Assert.NotNull(ws);
        ctx.WithTags([new ValueTag("etag1", 2)]);
        ws = mapper.ValidateToWalkingStep(wsprev, "key1", exit, 10);
        ctx.ClearTags();
        Assert.Null(ws);
        ws = mapper.ValidateToWalkingStep(wsprev, "key1", exit, 10);
        Assert.NotNull(ws);
        opt.WithMaxTotalCost(15);
        ws = mapper.ValidateToWalkingStep(wsprev, "key1", exit, 10);
        Assert.Null(ws);
    }
    [Fact]
    public void TestAddRoomWalkingSteps()
    {
        var md = new MapDatabase();
        md.NewMap();
        var ctx = new Context();
        var opt = new MapperOptions();
        var mapper = new Mapper(md.Current!, ctx, opt);
        var list = new List<WalkingStep>();
        var room = new Room()
        {
            Key = "key1",
            Tags = [
                new ValueTag("etag1", 1)
            ],
            Exits = [
                new Exit()
                {
                    To = "key2",
                    Command = "cmd1",
                    Conditions = [
                        new ValueCondition("etag1", 1, true)
                    ],
                    Cost = 10,
                },
                new Exit()
                {
                    To = "key3",
                    Command = "cmd2",
                    Cost = 20,
                },
            ],
        };
        md.APIInsertRooms([room, new Room() { Key = "key2" }, new Room() { Key = "key3" }, new Room() { Key = "key4" }]);
        mapper.AddRoomWalkingSteps(null, list, "notfound", 15);
        Assert.Empty(list);
        md.APIInsertShortcuts([new Shortcut()
        {
            Key="shortcut1",
            Command="cmd3",
            To="key4",
            Cost=1,
        },
        ]);
        ctx.WithTags([new ValueTag("etag1", 2)]);
        mapper.AddRoomWalkingSteps(null, list, "key1", 15);
        Assert.Equal(2, list.Count);
        list.Sort((a, b) => a.To.CompareTo(b.To));
        Assert.Null(list[0].Prev);
        Assert.Equal("key3", list[0].To);
        Assert.Equal("cmd2", list[0].Command);
        Assert.Equal(20, list[0].Cost);
        Assert.Equal(35, list[0].TotalCost);
        Assert.Equal(19, list[0].Remain);
        Assert.Null(list[1].Prev);
        Assert.Equal("key4", list[1].To);
        Assert.Equal("cmd3", list[1].Command);
        Assert.Equal(1, list[1].Cost);
        Assert.Equal(16, list[1].TotalCost);
        Assert.Equal(0, list[1].Remain);
    }
    private static string SortAndJoin(List<string> list)
    {
        list.Sort();
        return string.Join(";", list);
    }
    [Fact]
    public void TestDilate()
    {
        var md = new MapDatabase();
        md.NewMap();
        var ctx = new Context();
        var opt = new MapperOptions();
        var mapper = new Mapper(md.Current!, ctx, opt);
        md.APIInsertRooms([
            new Room() { Key = "key1" },
            new Room() { Key = "key2" },
            new Room() { Key = "key3" },
            new Room() { Key = "key4" },
            new Room() { Key = "key5" },

        ]);
        ctx.WithPaths([
            new HellMapManager.Models.Path(){From = "key1",To = "key2",Command = "1>2",},
            new HellMapManager.Models.Path(){From = "key2",To = "key1",Command = "2>1",},
            new HellMapManager.Models.Path(){From = "key2",To = "key3",Command = "2>3",},
            new HellMapManager.Models.Path(){From = "key3",To = "key2",Command = "3>2",},
            new HellMapManager.Models.Path(){From = "key3",To = "key4",Command = "3>4",},
            new HellMapManager.Models.Path(){From = "key4",To = "key3",Command = "4>3",},
            new HellMapManager.Models.Path(){From = "key4",To = "key5",Command = "4>5",},
            new HellMapManager.Models.Path(){From = "key5",To = "key4",Command = "5>4",},
        ]);
        Assert.Equal("key3", SortAndJoin(new Walking(mapper).Dilate(["key3"], -1)));
        Assert.Equal("key3", SortAndJoin(new Walking(mapper).Dilate(["key3"], 0)));
        Assert.Equal("key2;key3;key4", SortAndJoin(new Walking(mapper).Dilate(["key3"], 1)));
        Assert.Equal("key1;key2;key3;key4;key5", SortAndJoin(new Walking(mapper).Dilate(["key3"], 2)));
        Assert.Equal("key1;key2;key3;key4;key5", SortAndJoin(new Walking(mapper).Dilate(["key3"], 99)));
    }
    [Fact]
    public void TestQueryPathAny()
    {
        var md = new MapDatabase();
        md.NewMap();
        var ctx = new Context();
        var opt = new MapperOptions();
        var mapper = new Mapper(md.Current!, ctx, opt);
        md.APIInsertRooms([
            new Room() { Key = "key1" },
            new Room() { Key = "key2" },
            new Room() { Key = "key3" },
            new Room() { Key = "key4" },
            new Room() { Key = "key5" },

        ]);
        ctx.WithPaths([
            new HellMapManager.Models.Path(){From = "key1",To = "key2",Command = "1>2",Cost=2},
            new HellMapManager.Models.Path(){From = "key2",To = "key1",Command = "2>1",Cost=5},
            new HellMapManager.Models.Path(){From = "key2",To = "key3",Command = "2>3",Cost=5},
            new HellMapManager.Models.Path(){From = "key3",To = "key2",Command = "3>2",Cost=0},
            new HellMapManager.Models.Path(){From = "key3",To = "key4",Command = "3>4",Cost=1},
            new HellMapManager.Models.Path(){From = "key4",To = "key3",Command = "4>3",Cost=2},
        ]);
        var result = new Walking(mapper).QueryPathAny([], [], 0);
        Assert.False(result.IsSuccess());
        result = new Walking(mapper).QueryPathAny([""], ["key1"], 0);
        Assert.False(result.IsSuccess());
        result = new Walking(mapper).QueryPathAny(["key1"], [""], 0);
        Assert.False(result.IsSuccess());
        result = new Walking(mapper).QueryPathAny(["key1"], ["key4"], 0);
        Assert.True(result.IsSuccess());
        Assert.Equal(3, result.Steps.Count);
        Assert.Equal("1>2;2>3;3>4", Step.JoinCommands(";", result.Steps));
        Assert.Equal(8, result.Cost);
        Assert.Equal("key1", result.From);
        Assert.Equal("key4", result.To);
        Assert.Empty(result.Unvisited);
        result = new Walking(mapper).QueryPathAny(["key1", "key2", "key3", "key4"], ["key5"], 0);
        Assert.False(result.IsSuccess());
        result = new Walking(mapper).QueryPathAny(["key1", "key2", "key3"], ["key3", "key4", "key5"], 10);
        Assert.True(result.IsSuccess());
        Assert.Empty(result.Steps);
        Assert.Equal(10, result.Cost);
        Assert.Equal("key3", result.From);
        Assert.Equal("key3", result.To);
        Assert.Equal("key4;key5", SortAndJoin(result.Unvisited));
        result = new Walking(mapper).QueryPathAny(["key1"], ["key3", "key4", "key5"], 10);
        Assert.True(result.IsSuccess());
        Assert.Equal("1>2;2>3", Step.JoinCommands(";", result.Steps));
        Assert.Equal(17, result.Cost);
        Assert.Equal("key1", result.From);
        Assert.Equal("key3", result.To);
        Assert.Equal("key4;key5", SortAndJoin(result.Unvisited));

    }
}

