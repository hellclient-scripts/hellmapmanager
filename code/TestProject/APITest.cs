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
        var badroom = new Room()
        {
            Key = "",
            Group = "group1",
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
        Assert.False(badroom.Validated());
        appState.APIInsertRooms([badroom]);
        Assert.True(updated);
        updated = false;
        rooms = appState.APIListRooms(opt);
        Assert.Equal(4, rooms.Count);
        appState.APIRemoveRooms([]);
        Assert.False(updated);
        appState.APIRemoveRooms(["key1"]);
        Assert.True(updated);
        rooms = appState.APIListRooms(opt);
        Assert.Equal(3, rooms.Count);
        Assert.Equal(room3, rooms[0]);
        Assert.Equal(newroom2, rooms[1]);
        Assert.Equal(room4, rooms[2]);
        appState.APIRemoveRooms(["key1", "key2", "key4"]);
        Assert.True(updated);
        rooms = appState.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room3, rooms[0]);
    }
    [Fact]
    public void TestMarkerAPI()
    {
        bool updated = false;
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        List<Marker> markers = new List<Marker>();
        var marker1 = new Marker()
        {
            Key = "key1",
            Value = "value1",
            Group = "group1",
        };
        var marker2 = new Marker()
        {
            Key = "key2",
            Value = "value2",
            Group = "",
        };
        var newmarker2 = new Marker()
        {
            Key = "key2",
            Value = "value2",
            Group = "group2",
        };
        var marker3 = new Marker()
        {
            Key = "key3",
            Value = "value3",
            Group = "group1",
        };
        var marker4 = new Marker()
        {
            Key = "key4",
            Value = "value4",
            Group = "group2",
        };
        var badmarker = new Marker()
        {
            Key = "badkey",
            Value = "",
            Group = "group1",
        };
        var opt = new APIListOption();
        Assert.Null(opt.Key);
        Assert.Null(opt.Group);
        markers = appState.APIListMarkers(opt);
        Assert.Empty(markers);
        appState.APIInsertMarkers([marker1, marker2, marker3]);
        markers = appState.APIListMarkers(opt);
        Assert.Empty(markers);
        Assert.False(updated);
        appState.APIRemoveMarkers(["key1"]);
        markers = appState.APIListMarkers(opt);
        Assert.Empty(markers);
        Assert.False(updated);
        appState.NewMap();
        appState.APIInsertMarkers([marker1, marker2, marker3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        markers = appState.APIListMarkers(opt);
        Assert.Equal(3, markers.Count);
        Assert.Equal(marker2, markers[0]);
        Assert.Equal(marker1, markers[1]);
        Assert.Equal(marker3, markers[2]);
        opt.Group = "";
        markers = appState.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker2, markers[0]);
        opt.Group = "group1";
        markers = appState.APIListMarkers(opt);
        Assert.Equal(2, markers.Count);
        Assert.Equal(marker1, markers[0]);
        Assert.Equal(marker3, markers[1]);
        opt.Group = "notfound";
        markers = appState.APIListMarkers(opt);
        Assert.Empty(markers);
        opt.Group = null;
        opt.Key = "key2";
        markers = appState.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker2, markers[0]);
        opt.Group = "group1";
        opt.Key = "key2";
        markers = appState.APIListMarkers(opt);
        Assert.Empty(markers);
        opt.Key = "key1";
        markers = appState.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker1, markers[0]);
        updated = false;
        opt = new APIListOption();
        appState.APIInsertMarkers([]);
        Assert.False(updated);
        appState.APIInsertMarkers([newmarker2, marker4]);
        Assert.True(updated);
        updated = false;
        markers = appState.APIListMarkers(opt);
        Assert.Equal(4, markers.Count);
        Assert.Equal(marker1, markers[0]);
        Assert.Equal(marker3, markers[1]);
        Assert.Equal(newmarker2, markers[2]);
        Assert.Equal(marker4, markers[3]);
        Assert.False(badmarker.Validated());
        appState.APIInsertMarkers([badmarker]);
        Assert.True(updated);
        updated = false;
        markers = appState.APIListMarkers(opt);
        Assert.Equal(4, markers.Count);
        appState.APIRemoveMarkers([]);
        Assert.False(updated);
        appState.APIRemoveMarkers(["key1"]);
        Assert.True(updated);
        markers = appState.APIListMarkers(opt);
        Assert.Equal(3, markers.Count);
        Assert.Equal(marker3, markers[0]);
        Assert.Equal(newmarker2, markers[1]);
        Assert.Equal(marker4, markers[2]);
        appState.APIRemoveMarkers(["key1", "key2", "key4"]);
        Assert.True(updated);
        markers = appState.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker3, markers[0]);
    }
}