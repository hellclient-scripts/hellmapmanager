using HellMapManager.States;
using HellMapManager.Models;

namespace TestProject;
public class APITest
{
    [Fact]
    public void TestRoomAPI()
    {
        bool updated = false;
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        List<Room> rooms = new List<Room>();
        var room1 = new Room()
        {
            Key = "key1",
            Group = "group1",
        };
        var room2 = new Room()
        {
            Key = "key2",
            Group = "",
        };
        var newroom2 = new Room()
        {
            Key = "key2",
            Group = "group2",
        };
        var room3 = new Room()
        {
            Key = "key3",
            Group = "group1",
        };
        var room4 = new Room()
        {
            Key = "key4",
            Group = "group2",
        };
        var opt = new APIListOption();
        Assert.Null(opt.Key);
        Assert.Null(opt.Group);
        rooms = appState.APIListRooms(opt);
        Assert.Empty(rooms);
        appState.APIInsertRooms([room1, room2, room3]);
        rooms = appState.APIListRooms(opt);
        Assert.Empty(rooms);
        Assert.False(updated);
        appState.APIRemoveRooms(["key1"]);
        rooms = appState.APIListRooms(opt);
        Assert.Empty(rooms);
        Assert.False(updated);
        appState.NewMap();
        appState.APIInsertRooms([room1, room2, room3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        rooms = appState.APIListRooms(opt);
        Assert.Equal(3, rooms.Count);
        Assert.Equal(room2, rooms[0]);
        Assert.Equal(room1, rooms[1]);
        Assert.Equal(room3, rooms[2]);
        opt.Group = "";
        rooms = appState.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room2, rooms[0]);
        opt.Group = "group1";
        rooms = appState.APIListRooms(opt);
        Assert.Equal(2, rooms.Count);
        Assert.Equal(room1, rooms[0]);
        Assert.Equal(room3, rooms[1]);
        opt.Group = "notfound";
        rooms = appState.APIListRooms(opt);
        Assert.Empty(rooms);
        opt.Group = null;
        opt.Key = "key2";
        rooms = appState.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room2, rooms[0]);
        opt.Group = "group1";
        opt.Key = "key2";
        rooms = appState.APIListRooms(opt);
        Assert.Empty(rooms);
        opt.Key = "key1";
        rooms = appState.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room1, rooms[0]);
        updated = false;
        opt = new APIListOption();
        appState.APIInsertRooms([]);
        Assert.False(updated);
        appState.APIInsertRooms([newroom2, room4]);
        Assert.True(updated);
        updated = false;
        rooms = appState.APIListRooms(opt);
        Assert.Equal(4, rooms.Count);
        Assert.Equal(room1, rooms[0]);
        Assert.Equal(room3, rooms[1]);
        Assert.Equal(newroom2, rooms[2]);
        Assert.Equal(room4, rooms[3]);

    }
}