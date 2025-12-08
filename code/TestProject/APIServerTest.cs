using System.Text;
using System.Text.Json;
using HellMapManager.Cores;
using HellMapManager.Services.API;
using HellMapManager.Models;

namespace TestProject;

public class APIServerTest
{
    public APIServerTest()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    }
    private async Task<string> Post(string url, object data)
    {

        var client = new HttpClient();
        using var content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
        using var response = await client.PostAsync(url, content);
        var responseString = await response.Content.ReadAsStringAsync();
        return responseString ?? "";
    }
    [Fact]
    public async Task TestVersion()
    {
        var mapDatabase = new MapDatabase();
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
        Assert.Equal(MapDatabase.Version, mapDatabase.APIVersion());
        var resp = await Post($"http://localhost:{server.Port}" + "/api/version", "");
        var result = JsonSerializer.Deserialize(resp, typeof(int), APIJsonSerializerContext.Default) as int?;
        Assert.Equal(1000, result);
        server.Stop();
    }
    [Fact]
    public async Task TestInfo()
    {
        var mapDatabase = new MapDatabase();
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
        Assert.Equal(MapDatabase.Version, mapDatabase.APIVersion());
        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/info", "");
        var result = JsonSerializer.Deserialize(resp, typeof(APIResultInfo), APIJsonSerializerContext.Default);
        Assert.Null(result);
        mapDatabase.SetCurrent(HellMapManager.Models.MapFile.Create("name", "desc"));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/info", "");
        var result2 = (JsonSerializer.Deserialize(resp, typeof(APIResultInfo), APIJsonSerializerContext.Default) as APIResultInfo);
        Assert.NotNull(result2);
        Assert.Equal("name", result2.Name);
        Assert.Equal("desc", result2.Desc);
        server.Stop();
    }
    [Fact]
    public async Task TestRoomAPI()
    {
        var mapDatabase = new MapDatabase();
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();

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
        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        var result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;

        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertrooms", new InputRooms() { Rooms = RoomModel.FromList([room1, room2, room3]) });
        Assert.Equal("\"success\"", resp);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removerooms", new KeyList() { Keys = ["key1"] });
        Assert.Equal("\"success\"", resp);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Empty(result!);
        mapDatabase.NewMap();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertrooms", new InputRooms() { Rooms = RoomModel.FromList([room1, room2, room3]) });
        Assert.Equal("\"success\"", resp);
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(room2.Equal(result[0].ToRoom()));
        Assert.True(room1.Equal(result[1].ToRoom()));
        Assert.True(room3.Equal(result[2].ToRoom()));
        opt.Clear().WithGroups([""]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Single(result!);
        Assert.True(room2.Equal(result![0].ToRoom()));
        opt.Clear().WithGroups(["group1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Equal(2, result!.Count);
        Assert.True(room1.Equal(result![0].ToRoom()));
        Assert.True(room3.Equal(result![1].ToRoom()));
        opt.Clear().WithGroups(["notfound"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Empty(result!);
        opt.Clear().WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Single(result!);
        Assert.True(room2.Equal(result![0].ToRoom()));
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Empty(result!);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Single(result!);
        Assert.True(room1.Equal(result![0].ToRoom()));
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertrooms", new InputRooms() { Rooms = RoomModel.FromList([]) });
        Assert.Equal("\"success\"", resp);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertrooms", new InputRooms() { Rooms = RoomModel.FromList([newroom2, room4]) });
        Assert.Equal("\"success\"", resp);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Equal(4, result!.Count);
        Assert.True(room1.Equal(result![0].ToRoom()));
        Assert.True(room3.Equal(result![1].ToRoom()));
        Assert.True(newroom2.Equal(result![2].ToRoom()));
        Assert.True(room4.Equal(result![3].ToRoom()));
        Assert.False(badroom.Validated());
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertrooms", new InputRooms() { Rooms = RoomModel.FromList([badroom]) });
        Assert.Equal("\"success\"", resp);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Equal(4, result!.Count);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removerooms", new KeyList() { Keys = [] });
        Assert.Equal("\"success\"", resp);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removerooms", new KeyList() { Keys = ["key1"] });
        Assert.Equal("\"success\"", resp);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(room3.Equal(result![0].ToRoom()));
        Assert.True(newroom2.Equal(result![1].ToRoom()));
        Assert.True(room4.Equal(result![2].ToRoom()));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removerooms", new KeyList() { Keys = ["key1", "key2", "key4"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listrooms", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RoomModel>), APIJsonSerializerContext.Default) as List<RoomModel>;
        Assert.Single(result!);
        Assert.True(room3.Equal(result![0].ToRoom()));

        server.Stop();
    }
    [Fact]
    public async Task TestMarkerAPI()
    {
        var mapDatabase = new MapDatabase();
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
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
        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        var result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertmarkers", new InputMarkers() { Markers = MarkerModel.FromList([marker1, marker2, marker3]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removemarkers", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Empty(result!);
        mapDatabase.NewMap();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertmarkers", new InputMarkers() { Markers = MarkerModel.FromList([marker1, marker2, marker3]) });
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(marker2.Equal(result[0].ToMarker()));
        Assert.True(marker1.Equal(result[1].ToMarker()));
        Assert.True(marker3.Equal(result[2].ToMarker()));
        opt.Clear().WithGroups([""]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Single(result!);
        Assert.True(marker2.Equal(result![0].ToMarker()));
        opt.Clear().WithGroups(["group1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Equal(2, result!.Count);
        Assert.True(marker1.Equal(result![0].ToMarker()));
        Assert.True(marker3.Equal(result![1].ToMarker()));
        opt.Clear().WithGroups(["notfound"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Empty(result!);
        opt.Clear().WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Single(result!);
        Assert.True(marker2.Equal(result![0].ToMarker()));
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Empty(result!);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Single(result!);
        Assert.True(marker1.Equal(result![0].ToMarker()));
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertmarkers", new InputMarkers() { Markers = MarkerModel.FromList([]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertmarkers", new InputMarkers() { Markers = MarkerModel.FromList([newmarker2, marker4]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Equal(4, result!.Count);
        Assert.True(marker1.Equal(result![0].ToMarker()));
        Assert.True(marker3.Equal(result![1].ToMarker()));
        Assert.True(newmarker2.Equal(result![2].ToMarker()));
        Assert.True(marker4.Equal(result![3].ToMarker()));
        Assert.False(badmarker.Validated());
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertmarkers", new InputMarkers() { Markers = MarkerModel.FromList([badmarker]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Equal(4, result!.Count);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removemarkers", new KeyList() { Keys = [] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removemarkers", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(marker3.Equal(result![0].ToMarker()));
        Assert.True(newmarker2.Equal(result![1].ToMarker()));
        Assert.True(marker4.Equal(result![2].ToMarker()));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removemarkers", new KeyList() { Keys = ["key1", "key2", "key4"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listmarkers", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<MarkerModel>), APIJsonSerializerContext.Default) as List<MarkerModel>;
        Assert.Single(result!);
        Assert.True(marker3.Equal(result![0].ToMarker()));
        server.Stop();
        return;
    }

}