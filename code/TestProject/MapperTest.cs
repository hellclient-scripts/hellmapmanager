using HellMapManager.Models;
using HellMapManager.Helpers;
using HellMapManager.Cores;

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
            Cost = 10,
        };
        var mapper = new Mapper(md.Current!, ctx, new MapperOptions());
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
    }
}