using HellMapManager.States;
using HellMapManager.Models;

namespace TestProject;
public class APITest
{
    [Fact]
    public void TestRoomAPI()
    {
        var appState = new AppState();
        appState.NewMap();
        List<Room> rooms = new List<Room>();
        var room1 = new Room()
        {
            Key = "key1",
            Group = "group2",
        };
        var room2 = new Room()
        {
            Key = "key2",
            Group = "",
        };
        var room3 = new Room()
        {
            Key = "key3",
            Group = "group2",
        };
        var opt = new APIListOption();
        Assert.Null(opt.Key);
        Assert.Null(opt.Group);
        rooms = appState.APIListRooms(opt);
        Assert.Empty(rooms);

    }
}