using System.Text;
using System.Text.Json;
using HellMapManager.Models;
using HellMapManager.Cores;
using HellMapManager.Services.API;

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
    [Fact]
    public async Task TestRouteAPI()
    {
        var mapDatabase = new MapDatabase();
        var route1 = new Route()
        {
            Key = "key1",
            Group = "group1",
            Desc = "desc1",
            Message = "message1",
            Rooms = ["key1", "key2"],
        };
        var route2 = new Route()
        {
            Key = "key2",
            Group = "",
            Desc = "desc2",
            Message = "message2",
            Rooms = ["key3"],
        };
        var newroute2 = new Route()
        {
            Key = "key2",
            Group = "group2",
            Desc = "desc2",
            Message = "message2",
            Rooms = ["key3"],
        };
        var route3 = new Route()
        {
            Key = "key3",
            Group = "group1",
            Desc = "desc3",
            Message = "message3",
            Rooms = ["key4"],
        };
        var route4 = new Route()
        {
            Key = "key4",
            Group = "group2",
            Desc = "desc4",
            Message = "message4",
            Rooms = ["key5"],
        };
        var badroute1 = new Route()
        {
            Key = "",
            Group = "",
            Desc = "",
            Message = "",
            Rooms = [],
        };
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
        var opt = new APIListOption();

        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        var result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertroutes", new InputRoutes() { Routes = RouteModel.FromList([route1, route2, route3]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeroutes", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Empty(result!);
        mapDatabase.NewMap();

        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertroutes", new InputRoutes() { Routes = RouteModel.FromList([route1, route2, route3]) });
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(route2.Equal(result[0].ToRoute()));
        Assert.True(route1.Equal(result[1].ToRoute()));
        Assert.True(route3.Equal(result[2].ToRoute()));
        opt.Clear().WithGroups([""]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Single(result!);
        Assert.True(route2.Equal(result![0].ToRoute()));
        opt.Clear().WithGroups(["group1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Equal(2, result!.Count);
        Assert.True(route1.Equal(result![0].ToRoute()));
        Assert.True(route3.Equal(result![1].ToRoute()));
        opt.Clear().WithGroups(["notfound"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Empty(result!);
        opt.Clear().WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Single(result!);
        Assert.True(route2.Equal(result![0].ToRoute()));
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Empty(result!);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Single(result!);
        Assert.True(route1.Equal(result![0].ToRoute()));
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertroutes", new InputRoutes() { Routes = RouteModel.FromList([]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertroutes", new InputRoutes() { Routes = RouteModel.FromList([newroute2, route4]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Equal(4, result!.Count);
        Assert.True(route1.Equal(result![0].ToRoute()));
        Assert.True(route3.Equal(result![1].ToRoute()));
        Assert.True(newroute2.Equal(result![2].ToRoute()));
        Assert.True(route4.Equal(result![3].ToRoute()));
        Assert.False(badroute1.Validated());
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertroutes", new InputRoutes() { Routes = RouteModel.FromList([badroute1]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Equal(4, result!.Count);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeroutes", new KeyList() { Keys = [] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeroutes", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(route3.Equal(result![0].ToRoute()));
        Assert.True(newroute2.Equal(result![1].ToRoute()));
        Assert.True(route4.Equal(result![2].ToRoute()));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeroutes", new KeyList() { Keys = ["key1", "key2", "key4"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listroutes", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RouteModel>), APIJsonSerializerContext.Default) as List<RouteModel>;
        Assert.Single(result!);
        Assert.True(route3.Equal(result![0].ToRoute()));

        server.Stop();
        return;
    }
    [Fact]
    public async Task TestTraceAPI()
    {
        var mapDatabase = new MapDatabase();
        var trace1 = new Trace()
        {
            Key = "key1",
            Group = "group1",
            Desc = "desc1",
            Message = "message1",
        };
        var trace2 = new Trace()
        {
            Key = "key2",
            Group = "",
            Desc = "desc2",
            Message = "message2",
        };
        var newtrace2 = new Trace()
        {
            Key = "key2",
            Group = "group2",
            Desc = "desc2",
            Message = "message2",
        };
        var trace3 = new Trace()
        {
            Key = "key3",
            Group = "group1",
            Desc = "desc3",
            Message = "message3",
        };
        var trace4 = new Trace()
        {
            Key = "key4",
            Group = "group2",
            Desc = "desc4",
            Message = "message4",
        };
        var badtrace1 = new Trace()
        {
            Key = "",
            Group = "",
            Desc = "",
            Message = "",
        };
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
        var opt = new APIListOption();
        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        var result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/inserttraces", new InputTraces() { Traces = TraceModel.FromList([trace1, trace2, trace3]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removetraces", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Empty(result!);
        mapDatabase.NewMap();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/inserttraces", new InputTraces() { Traces = TraceModel.FromList([trace1, trace2, trace3]) });
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(trace2.Equal(result[0].ToTrace()));
        Assert.True(trace1.Equal(result[1].ToTrace()));
        Assert.True(trace3.Equal(result[2].ToTrace()));
        opt.Clear().WithGroups([""]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Single(result!);
        Assert.True(trace2.Equal(result![0].ToTrace()));
        opt.Clear().WithGroups(["group1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Equal(2, result!.Count);
        Assert.True(trace1.Equal(result![0].ToTrace()));
        Assert.True(trace3.Equal(result![1].ToTrace()));
        opt.Clear().WithGroups(["notfound"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Empty(result!);
        opt.Clear().WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Single(result!);
        Assert.True(trace2.Equal(result![0].ToTrace()));
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Empty(result!);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Single(result!);
        Assert.True(trace1.Equal(result![0].ToTrace()));
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/inserttraces", new InputTraces() { Traces = TraceModel.FromList([]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/inserttraces", new InputTraces() { Traces = TraceModel.FromList([newtrace2, trace4]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Equal(4, result!.Count);
        Assert.True(trace1.Equal(result![0].ToTrace()));
        Assert.True(trace3.Equal(result![1].ToTrace()));
        Assert.True(newtrace2.Equal(result![2].ToTrace()));
        Assert.True(trace4.Equal(result![3].ToTrace()));
        Assert.False(badtrace1.Validated());
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/inserttraces", new InputTraces() { Traces = TraceModel.FromList([badtrace1]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Equal(4, result!.Count);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removetraces", new KeyList() { Keys = [] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removetraces", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(trace3.Equal(result![0].ToTrace()));
        Assert.True(newtrace2.Equal(result![1].ToTrace()));
        Assert.True(trace4.Equal(result![2].ToTrace()));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removetraces", new KeyList() { Keys = ["key1", "key2", "key4"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listtraces", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<TraceModel>), APIJsonSerializerContext.Default) as List<TraceModel>;
        Assert.Single(result!);
        Assert.True(trace3.Equal(result![0].ToTrace()));
        server.Stop();
        return;
    }
    [Fact]
    public async Task TestRegionAPI()
    {
        var mapDatabase = new MapDatabase();
        var region1 = new Region()
        {
            Key = "key1",
            Group = "group1",
            Desc = "desc1",
            Message = "message1",
        };
        var region2 = new Region()
        {
            Key = "key2",
            Group = "",
            Desc = "desc2",
            Message = "message2",
        };
        var newregion2 = new Region()
        {
            Key = "key2",
            Group = "group2",
            Desc = "desc2",
            Message = "message2",
        };
        var region3 = new Region()
        {
            Key = "key3",
            Group = "group1",
            Desc = "desc3",
            Message = "message3",
        };
        var region4 = new Region()
        {
            Key = "key4",
            Group = "group2",
            Desc = "desc4",
            Message = "message4",
        };
        var badregion1 = new Region()
        {
            Key = "",
            Group = "",
            Desc = "",
            Message = "",
        };
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
        var opt = new APIListOption();
        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        var result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertregions", new InputRegions() { Regions = RegionModel.FromList([region1, region2, region3]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeregions", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Empty(result!);
        mapDatabase.NewMap();
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertregions", new InputRegions() { Regions = RegionModel.FromList([region1, region2, region3]) });
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(region2.Equal(result[0].ToRegion()));
        Assert.True(region1.Equal(result[1].ToRegion()));
        Assert.True(region3.Equal(result[2].ToRegion()));
        opt.Clear().WithGroups([""]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Single(result!);
        Assert.True(region2.Equal(result![0].ToRegion()));
        opt.Clear().WithGroups(["group1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Equal(2, result!.Count);
        Assert.True(region1.Equal(result![0].ToRegion()));
        Assert.True(region3.Equal(result![1].ToRegion()));
        opt.Clear().WithGroups(["notfound"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Empty(result!);
        opt.Clear().WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Single(result!);
        Assert.True(region2.Equal(result![0].ToRegion()));
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Empty(result!);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Single(result!);
        Assert.True(region1.Equal(result![0].ToRegion()));
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertregions", new InputRegions() { Regions = RegionModel.FromList([]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertregions", new InputRegions() { Regions = RegionModel.FromList([newregion2, region4]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Equal(4, result!.Count);
        Assert.True(region1.Equal(result![0].ToRegion()));
        Assert.True(region3.Equal(result![1].ToRegion()));
        Assert.True(newregion2.Equal(result![2].ToRegion()));
        Assert.True(region4.Equal(result![3].ToRegion()));
        Assert.False(badregion1.Validated());
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertregions", new InputRegions() { Regions = RegionModel.FromList([badregion1]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Equal(4, result!.Count);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeregions", new KeyList() { Keys = [] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeregions", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(region3.Equal(result![0].ToRegion()));
        Assert.True(newregion2.Equal(result![1].ToRegion()));
        Assert.True(region4.Equal(result![2].ToRegion()));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeregions", new KeyList() { Keys = ["key1", "key2", "key4"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listregions", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<RegionModel>), APIJsonSerializerContext.Default) as List<RegionModel>;
        Assert.Single(result!);
        Assert.True(region3.Equal(result![0].ToRegion()));
        server.Stop();
        return;

    }
    [Fact]
    public async Task TestShortcutAPI()
    {
        var mapDatabase = new MapDatabase();
        var shortcut1 = new Shortcut()
        {
            Key = "key1",
            Command = "cmd1",
            Group = "group1",
            Desc = "desc1",
        };
        var shortcut2 = new Shortcut()
        {
            Key = "key2",
            Command = "cmd2",
            Group = "",
            Desc = "desc2",
        };
        var newshortcut2 = new Shortcut()
        {
            Key = "key2",
            Command = "cmd2",
            Group = "group2",
            Desc = "desc2",
        };
        var shortcut3 = new Shortcut()
        {
            Key = "key3",
            Command = "cmd3",
            Group = "group1",
            Desc = "desc3",
        };
        var shortcut4 = new Shortcut()
        {
            Key = "key4",
            Command = "cmd4",
            Group = "group2",
            Desc = "desc4",
        };
        var badshortcut1 = new Shortcut()
        {
            Key = "",
            Group = "",
            Desc = "",
        };
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
        var opt = new APIListOption();
        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        var result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertshortcuts", new InputShortcuts() { Shortcuts = ShortcutModel.FromList([shortcut1, shortcut2, shortcut3]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeshortcuts", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Empty(result!);
        mapDatabase.NewMap();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertshortcuts", new InputShortcuts() { Shortcuts = ShortcutModel.FromList([shortcut1, shortcut2, shortcut3]) });
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(shortcut2.Equal(result[0].ToShortcut()));
        Assert.True(shortcut1.Equal(result[1].ToShortcut()));
        Assert.True(shortcut3.Equal(result[2].ToShortcut()));
        opt.Clear().WithGroups([""]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Single(result!);
        Assert.True(shortcut2.Equal(result![0].ToShortcut()));
        opt.Clear().WithGroups(["group1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Equal(2, result!.Count);
        Assert.True(shortcut1.Equal(result![0].ToShortcut()));
        Assert.True(shortcut3.Equal(result![1].ToShortcut()));
        opt.Clear().WithGroups(["notfound"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Empty(result!);
        opt.Clear().WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Single(result!);
        Assert.True(shortcut2.Equal(result![0].ToShortcut()));
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Empty(result!);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Single(result!);
        Assert.True(shortcut1.Equal(result![0].ToShortcut()));
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertshortcuts", new InputShortcuts() { Shortcuts = ShortcutModel.FromList([]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertshortcuts", new InputShortcuts() { Shortcuts = ShortcutModel.FromList([newshortcut2, shortcut4]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Equal(4, result!.Count);
        Assert.True(shortcut1.Equal(result![0].ToShortcut()));
        Assert.True(shortcut3.Equal(result![1].ToShortcut()));
        Assert.True(newshortcut2.Equal(result![2].ToShortcut()));
        Assert.True(shortcut4.Equal(result![3].ToShortcut()));
        Assert.False(badshortcut1.Validated());
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertshortcuts", new InputShortcuts() { Shortcuts = ShortcutModel.FromList([badshortcut1]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Equal(4, result!.Count);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeshortcuts", new KeyList() { Keys = [] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeshortcuts", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(shortcut3.Equal(result![0].ToShortcut()));
        Assert.True(newshortcut2.Equal(result![1].ToShortcut()));
        Assert.True(shortcut4.Equal(result![2].ToShortcut()));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removeshortcuts", new KeyList() { Keys = ["key1", "key2", "key4"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listshortcuts", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<ShortcutModel>), APIJsonSerializerContext.Default) as List<ShortcutModel>;
        Assert.Single(result!);
        Assert.True(shortcut3.Equal(result![0].ToShortcut()));
        server.Stop();
        return;
    }
    [Fact]
    public async Task TestVariableAPI()
    {
        var mapDatabase = new MapDatabase();
        var variable1 = new Variable()
        {
            Key = "key1",
            Value = "value1",
            Group = "group1",
            Desc = "desc1",
        };
        var variable2 = new Variable()
        {
            Key = "key2",
            Value = "value2",
            Group = "",
            Desc = "desc2",
        };
        var newvariable2 = new Variable()
        {
            Key = "key2",
            Value = "value2",
            Group = "group2",
            Desc = "desc2",
        };
        var variable3 = new Variable()
        {
            Key = "key3",
            Value = "value3",
            Group = "group1",
            Desc = "desc3",
        };
        var variable4 = new Variable()
        {
            Key = "key4",
            Value = "value4",
            Group = "group2",
            Desc = "desc4",
        };
        var badvariable1 = new Variable()
        {
            Key = "",
            Group = "",
            Desc = "",
            Value = "",
        };
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
        var opt = new APIListOption();
        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        var result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertvariables", new InputVariables() { Variables = VariableModel.FromList([variable1, variable2, variable3]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removevariables", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Empty(result!);
        mapDatabase.NewMap();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertvariables", new InputVariables() { Variables = VariableModel.FromList([variable1, variable2, variable3]) });
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(variable2.Equal(result[0].ToVariable()));
        Assert.True(variable1.Equal(result[1].ToVariable()));
        Assert.True(variable3.Equal(result[2].ToVariable()));
        opt.Clear().WithGroups([""]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Single(result!);
        Assert.True(variable2.Equal(result![0].ToVariable()));
        opt.Clear().WithGroups(["group1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Equal(2, result!.Count);
        Assert.True(variable1.Equal(result![0].ToVariable()));
        Assert.True(variable3.Equal(result![1].ToVariable()));
        opt.Clear().WithGroups(["notfound"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Empty(result!);
        opt.Clear().WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Single(result!);
        Assert.True(variable2.Equal(result![0].ToVariable()));
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Empty(result!);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Single(result!);
        Assert.True(variable1.Equal(result![0].ToVariable()));
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertvariables", new InputVariables() { Variables = VariableModel.FromList([]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertvariables", new InputVariables() { Variables = VariableModel.FromList([newvariable2, variable4]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Equal(4, result!.Count);
        Assert.True(variable1.Equal(result![0].ToVariable()));
        Assert.True(variable3.Equal(result![1].ToVariable()));
        Assert.True(newvariable2.Equal(result![2].ToVariable()));
        Assert.True(variable4.Equal(result![3].ToVariable()));
        Assert.False(badvariable1.Validated());
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertvariables", new InputVariables() { Variables = VariableModel.FromList([badvariable1]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Equal(4, result!.Count);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removevariables", new KeyList() { Keys = [] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removevariables", new KeyList() { Keys = ["key1"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(variable3.Equal(result![0].ToVariable()));
        Assert.True(newvariable2.Equal(result![1].ToVariable()));
        Assert.True(variable4.Equal(result![2].ToVariable()));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removevariables", new KeyList() { Keys = ["key1", "key2", "key4"] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listvariables", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<VariableModel>), APIJsonSerializerContext.Default) as List<VariableModel>;
        Assert.Single(result!);
        Assert.True(variable3.Equal(result![0].ToVariable()));
        server.Stop();
        return;
    }
    [Fact]
    public async Task TestLandmarkAPI()
    {
        var mapDatabase = new MapDatabase();
        var landmark1 = new Landmark()
        {
            Key = "key1",
            Type = "type1",
            Group = "group1",
            Desc = "desc1",
        };
        var landmark1t2 = new Landmark()
        {
            Key = "key1",
            Type = "type2",
            Group = "group1",
            Desc = "desc1",
        };
        var landmark2 = new Landmark()
        {
            Key = "key2",
            Type = "type2",
            Group = "",
            Desc = "desc2",
        };
        var newlandmark2 = new Landmark()
        {
            Key = "key2",
            Type = "type2",
            Group = "group2",
            Desc = "desc2",
        };
        var landmark3 = new Landmark()
        {
            Key = "key3",
            Type = "type1",
            Group = "group1",
            Desc = "desc3",
        };
        var landmark4 = new Landmark()
        {
            Key = "key4",
            Type = "type1",
            Group = "group2",
            Desc = "desc4",
        };
        var badlandmark1 = new Landmark()
        {
            Key = "",
            Group = "",
            Desc = "",
        };
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
        var opt = new APIListOption();
        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        var result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertlandmarks", new InputLandmarks() { Landmarks = LandmarkModel.FromList([landmark1, landmark2, landmark3]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removelandmarks", new LandmarkKeyList() { LandmarkKeys = [KeyType.FromLandmarkKey(landmark1.UniqueKey())] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Empty(result!);
        mapDatabase.NewMap();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertlandmarks", new InputLandmarks() { Landmarks = LandmarkModel.FromList([landmark1, landmark1t2, landmark2, landmark3]) });
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Equal(4, result!.Count);
        Assert.True(landmark2.Equal(result![0].ToLandmark()));
        Assert.True(landmark1.Equal(result![1].ToLandmark()));
        Assert.True(landmark1t2.Equal(result![2].ToLandmark()));
        Assert.True(landmark3.Equal(result![3].ToLandmark()));
        opt.Clear().WithGroups([""]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Single(result!);
        Assert.True(landmark2.Equal(result![0].ToLandmark()));
        opt.Clear().WithGroups(["group1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(landmark1.Equal(result![0].ToLandmark()));
        Assert.True(landmark1t2.Equal(result![1].ToLandmark()));
        Assert.True(landmark3.Equal(result![2].ToLandmark()));
        opt.Clear().WithGroups(["notfound"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Empty(result!);
        opt.Clear().WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Single(result!);
        Assert.True(landmark2.Equal(result![0].ToLandmark()));
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Empty(result!);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Equal(2, result!.Count);
        Assert.True(landmark1.Equal(result![0].ToLandmark()));
        Assert.True(landmark1t2.Equal(result![1].ToLandmark()));
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertlandmarks", new InputLandmarks() { Landmarks = LandmarkModel.FromList([]) }); ;
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertlandmarks", new InputLandmarks() { Landmarks = LandmarkModel.FromList([newlandmark2, landmark4]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Equal(5, result!.Count);
        Assert.True(landmark1.Equal(result![0].ToLandmark()));
        Assert.True(landmark1t2.Equal(result![1].ToLandmark()));
        Assert.True(landmark3.Equal(result![2].ToLandmark()));
        Assert.True(newlandmark2.Equal(result![3].ToLandmark()));
        Assert.True(landmark4.Equal(result![4].ToLandmark()));
        Assert.False(badlandmark1.Validated());
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertlandmarks", new InputLandmarks() { Landmarks = LandmarkModel.FromList([badlandmark1]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Equal(5, result!.Count);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removelandmarks", new LandmarkKeyList() { LandmarkKeys = [] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removelandmarks", new LandmarkKeyList() { LandmarkKeys = [KeyType.FromLandmarkKey(landmark1.UniqueKey())] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Equal(4, result!.Count);
        Assert.True(landmark1t2.Equal(result![0].ToLandmark()));
        Assert.True(landmark3.Equal(result![1].ToLandmark()));
        Assert.True(newlandmark2.Equal(result![2].ToLandmark()));
        Assert.True(landmark4.Equal(result![3].ToLandmark()));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removelandmarks", new LandmarkKeyList() { LandmarkKeys = [KeyType.FromLandmarkKey(landmark1.UniqueKey()), KeyType.FromLandmarkKey(landmark1t2.UniqueKey()), KeyType.FromLandmarkKey(landmark2.UniqueKey()), KeyType.FromLandmarkKey(landmark4.UniqueKey())] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listlandmarks", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<LandmarkModel>), APIJsonSerializerContext.Default) as List<LandmarkModel>;
        Assert.Single(result!);
        Assert.True(landmark3.Equal(result![0].ToLandmark()));
        server.Stop();
        return;
    }
    [Fact]
    public async Task TestSnapshotAPI()
    {
        var mapDatabase = new MapDatabase();
        var snapshot1 = new Snapshot()
        {
            Key = "key1",
            Value = "Value1",
            Type = "type1",
            Group = "group1",
            Timestamp = 1234567890,
        };
        var snapshot1t2 = new Snapshot()
        {
            Key = "key1",
            Value = "Value1",
            Type = "type2",
            Group = "group1",
            Timestamp = 1234567890,
        };
        var snapshot2 = new Snapshot()
        {
            Key = "key2",
            Value = "Value2",
            Type = "type2",
            Group = "",
            Timestamp = 1234567890,
        };
        var newsnapshot2 = new Snapshot()
        {
            Key = "key2",
            Value = "Value2",
            Type = "type2",
            Group = "group2",
            Timestamp = 1234567890,
        };
        var snapshot3 = new Snapshot()
        {
            Key = "key3",
            Value = "Value3",
            Type = "type1",
            Group = "group1",
            Timestamp = 1234567890,
        };
        var snapshot4 = new Snapshot()
        {
            Key = "key4",
            Value = "Value4",
            Type = "type1",
            Group = "group2",
            Timestamp = 1234567890,
        };
        var badsnapshot1 = new Snapshot()
        {
            Key = "",
            Group = "",
        };
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
        var opt = new APIListOption();
        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        var result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertsnapshots", new InputSnapshots() { Snapshots = SnapshotModel.FromList([snapshot1, snapshot2, snapshot3]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Empty(result!);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removesnapshots", new SnapshotKeyList() { Keys = KeyTypeValue.FromSnapshotKeyList([snapshot1.UniqueKey()]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Empty(result!);
        mapDatabase.NewMap();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertsnapshots", new InputSnapshots() { Snapshots = SnapshotModel.FromList([snapshot1, snapshot1t2, snapshot2, snapshot3]) });
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Equal(4, result!.Count);
        Assert.True(snapshot2.Equal(result![0].ToSnapshot()));
        Assert.True(snapshot1.Equal(result[1].ToSnapshot()));
        Assert.True(snapshot1t2.Equal(result[2].ToSnapshot()));
        Assert.True(snapshot3.Equal(result[3].ToSnapshot()));
        opt.Clear().WithGroups([""]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Single(result!);
        Assert.True(snapshot2.Equal(result![0].ToSnapshot()));
        opt.Clear().WithGroups(["group1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Equal(3, result!.Count);
        Assert.True(snapshot1.Equal(result![0].ToSnapshot()));
        Assert.True(snapshot1t2.Equal(result![1].ToSnapshot()));
        Assert.True(snapshot3.Equal(result![2].ToSnapshot()));
        opt.Clear().WithGroups(["notfound"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Empty(result!);
        opt.Clear().WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Single(result!);
        Assert.True(snapshot2.Equal(result![0].ToSnapshot()));
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Empty(result!);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Equal(2, result!.Count);
        Assert.True(snapshot1.Equal(result![0].ToSnapshot()));
        Assert.True(snapshot1t2.Equal(result![1].ToSnapshot()));
        opt = new APIListOption();
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertsnapshots", new InputSnapshots() { Snapshots = SnapshotModel.FromList([]) }); ;
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertsnapshots", new InputSnapshots() { Snapshots = SnapshotModel.FromList([newsnapshot2, snapshot4]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Equal(5, result!.Count);
        Assert.True(snapshot1.Equal(result![0].ToSnapshot()));
        Assert.True(snapshot1t2.Equal(result![1].ToSnapshot()));
        Assert.True(snapshot3.Equal(result![2].ToSnapshot()));
        Assert.True(newsnapshot2.Equal(result![3].ToSnapshot()));
        Assert.True(snapshot4.Equal(result![4].ToSnapshot()));
        Assert.False(badsnapshot1.Validated());
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/insertsnapshots", new InputSnapshots() { Snapshots = SnapshotModel.FromList([badsnapshot1]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Equal(5, result!.Count);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removesnapshots", new SnapshotKeyList() { Keys = [] });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removesnapshots", new SnapshotKeyList() { Keys = KeyTypeValue.FromSnapshotKeyList([snapshot1.UniqueKey()]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Equal(4, result!.Count);
        Assert.True(snapshot1t2.Equal(result![0].ToSnapshot()));
        Assert.True(snapshot3.Equal(result![1].ToSnapshot()));
        Assert.True(newsnapshot2.Equal(result![2].ToSnapshot()));
        Assert.True(snapshot4.Equal(result![3].ToSnapshot()));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/removesnapshots", new SnapshotKeyList() { Keys = KeyTypeValue.FromSnapshotKeyList([snapshot1.UniqueKey(), snapshot1t2.UniqueKey(), snapshot2.UniqueKey(), snapshot4.UniqueKey()]) });
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/listsnapshots", InputListOption.From(opt));
        result = JsonSerializer.Deserialize(resp, typeof(List<SnapshotModel>), APIJsonSerializerContext.Default) as List<SnapshotModel>;
        Assert.Single(result!);
        Assert.True(snapshot3.Equal(result![0].ToSnapshot()));
        server.Stop();
        return;
    }
    private static void InitMapDatabase(MapDatabase md)
    {
        md.NewMap();
        md.APIInsertRooms([
            new Room(){Key="key1",
                Tags=[],
                Exits=[
                    new Exit(){
                        To ="key2",
                        Command ="1>2",
                        Cost=1,
                    },
                    new Exit(){
                        To ="key3",
                        Command ="1>3",
                        Cost=1,
                    },
                ],
            },
            new Room(){Key="key2",
                Tags=[],
                Exits=[
                    new Exit(){
                        To ="key1",
                        Command ="2>1",
                        Cost=1,
                    },
                    new Exit(){
                        To ="key3",
                        Command ="2>3",
                        Cost=1,
                    },
                ],
            },
            new Room(){Key="key3",
                Tags=[],
                Exits=[
                    new Exit(){
                        To ="key1",
                        Command ="3>1",
                        Cost=1,
                    },
                    new Exit(){
                        To ="key3",
                        Command ="3>3",
                        Cost=1,
                    },
                    new Exit(){
                        To ="key4",
                        Command ="3>4",
                        Cost=1,
                    },

                ],
            },
            new Room(){Key="key4",
                Tags=[],
                Exits=[
                    new Exit(){
                        To ="key3",
                        Command ="4>3",
                        Cost=1,
                    },
                    new Exit(){
                        To ="key5",
                        Command ="4>5",
                        Cost=1,
                    },
                ],
            },
            new Room(){Key="key5",
                Tags=[],
                Exits=[
                    new Exit(){
                        To ="key3",
                        Command ="5>3",
                        Cost=1,
                    },
                ],
            },
            new Room(){Key="key7",
                Tags=[],
                Exits=[
                ],
            }
        ]);
        md.APIInsertShortcuts([
            new Shortcut(){
                Key="shortcut1",
                To="key1",
                Command="A>1",
                Cost=2,
            },
        ]);
    }
    private static void InitContext(Context ctx)
    {
        ctx.ClearTags().WithTags([]);
        ctx.ClearRoomConditions().WithRoomConditions([]);
        ctx.ClearRooms().WithRooms([
                new Room(){
                Key ="key6",
                Tags=[new ValueTag("ctxroom", 1)],
                Exits=[
                    new Exit(){
                        To ="key3",
                        Command ="6>3",
                        Cost=1,
                    },
                ],
            }

        ]);
        ctx.ClearShortcuts().WithShortcuts([
            new Shortcut(){
                To="key6",
                Command="A>6C",
                Conditions=[new ValueCondition("noctxpath", 1,true)],
                Cost=2,
            },
        ]);
        ctx.ClearPaths().WithPaths([
            new HellMapManager.Models.Path(){
                From="key5",
                To="key6",
                Command="5>6C",
                Conditions=[new ValueCondition("noctxpath", 1,true)],
                Cost=1,
            },
            new HellMapManager.Models.Path(){
                From="key1",
                To="key2",
                Conditions=[new ValueCondition("noctxpath", 1,true)],
                Command="1>2C",
                Cost=1,
            },
        ]);
        ctx.ClearWhitelist().WithWhitelist([]);
        ctx.ClearBlacklist().WithBlacklist([]);
        ctx.ClearBlockedLinks().WithBlockedLinks([]);
        ctx.ClearCommandCosts().WithCommandCosts([]);
    }
    [Fact]
    public async Task TestMap()
    {
        var mapDatabase = new MapDatabase();
        var ctx = new Context();
        var opt = new MapperOptions();
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
        string resp;
        string exit;
        QueryResultModel? queryresult;
        List<string> rooms;
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.Null(queryresult);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key1"],
            Target = ["key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.Null(queryresult);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.Null(queryresult);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key1", "key6"],
            Iterations = 2,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        Assert.Empty(rooms);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key1",
            Command = "1>2",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", exit);
        InitMapDatabase(mapDatabase);
        InitContext(ctx);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>2", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key1"],
            Target = ["key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>2", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>2", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key1", "key6"],
            Iterations = 2,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key2;key3;key4;key6", string.Join(";", rooms));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key1",
            Command = "1>2",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key2", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "notfound",
            Command = "1>2",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key1",
            Command = "notfound",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", exit);
        //shortcut
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key6",
            Command = "A>1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key1", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key1",
            Command = "A>6C",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key6", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1;1>2", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key6"],
            Target = ["key1", "key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1;1>2", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key6"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));
        opt.WithDisableShortcuts(true);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key6",
            Command = "A>1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key1",
            Command = "A>6C",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1;1>2", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key6"],
            Target = ["key1", "key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key2"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1;1>2", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key6"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key3;key6", string.Join(";", rooms));

        //tag
        opt = new MapperOptions();
        InitContext(ctx);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key1",
            Command = "A>6C",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key6", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key3", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3;A>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key1"],
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3;3>4;4>5;5>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key1"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key2;key3;key6", string.Join(";", rooms));
        ctx.WithTags([new ValueTag("noctxpath", 1)]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key1",
            Command = "A>6C",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key3", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key1"],
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key1"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key2;key3", string.Join(";", rooms));

        //RoomConditions
        opt = new MapperOptions();
        InitContext(ctx);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key1",
            Command = "A>6C",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key6", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key3", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3;A>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key1"],
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3;3>4;4>5;5>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key1"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key2;key3;key6", string.Join(";", rooms));
        ctx.WithRoomConditions([new ValueCondition("ctxroom", 1, true)]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key1",
            Command = "A>6C",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key3", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key1"],
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key1"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key2;key3", string.Join(";", rooms));

        //Whitelist
        opt = new MapperOptions();
        InitContext(ctx);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key3",
            Command = "3>4",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key4", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>6C;6>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key3"],
            Target = ["key6", "key4"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("3>4", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3;3>4;4>5;5>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key3"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key3;key4;key6", string.Join(";", rooms));
        ctx.WithWhitelist(["key1", "key2", "key3", "key5", "key6"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key3",
            Command = "3>4",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key3"],
            Target = ["key6", "key4"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key3"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));

        //blacklist
        opt = new MapperOptions();
        InitContext(ctx);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key1",
            Command = "1>3",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key3", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key4", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>6C;6>3;3>4", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key4"],
            Target = ["key6", "key3"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("4>3", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("1>3;3>4;4>5;5>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key4"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key3;key4;key5;key6", string.Join(";", rooms));
        ctx.WithBlacklist(["key3"]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key1",
            Command = "1>3",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key4", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key4"],
            Target = ["key6", "key3"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key1",
            Target = ["key5", "key6"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>6C", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key4"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key4;key5;key6", string.Join(";", rooms));
        //BlockedLinks
        opt = new MapperOptions();
        InitContext(ctx);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key6",
            Command = "A>1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key1", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key6"],
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key6"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));
        ctx.WithBlockedLinks([new Link("key6", "key1")]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key6",
            Command = "A>1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key6"],
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key6"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key3;key6", string.Join(";", rooms));

        //CommandCosts
        opt = new MapperOptions();
        InitContext(ctx);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key6",
            Command = "A>1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key1", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key6"],
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key6"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));
        ctx.WithCommandCosts([new CommandCost("A>1", "key1", 99)]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key6",
            Command = "A>1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key1", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key6"],
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key6"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));
        //MaxExitCost
        opt = new MapperOptions();
        InitContext(ctx);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key6",
            Command = "A>1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key1", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key6"],
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key6"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));
        opt.MaxExitCost = 1;
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key6",
            Command = "A>1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key6"],
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("6>3;3>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key6"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key3;key6", string.Join(";", rooms));

        //MaxTotalCost
        opt = new MapperOptions();
        InitContext(ctx);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key6",
            Command = "A>1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key1", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key6"],
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key6"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));
        opt.MaxTotalCost = 3;
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/trackexit", new InputTrackExit()
        {
            Start = "key6",
            Command = "A>1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        exit = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("key1", exit);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathall", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathany", new InputQueryPathAny()
        {
            From = ["key6"],
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/querypathordered", new InputQueryPath()
        {
            Start = "key6",
            Target = ["key1", "key5"],
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        queryresult = JsonSerializer.Deserialize(resp, typeof(QueryResultModel), APIJsonSerializerContext.Default) as QueryResultModel;
        Assert.NotNull(queryresult);
        Assert.Equal("A>1", Step.JoinCommands(";", StepModel.ToStepList(queryresult!.Steps)));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/dilate", new InputDilate()
        {
            Src = ["key6"],
            Iterations = 1,
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        rooms = JsonSerializer.Deserialize(resp, typeof(List<string>), APIJsonSerializerContext.Default) as List<string> ?? [];
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));
        server.Stop();
        return;
    }
    [Fact]
    public async Task TestGetVariable()
    {
        var mapDatabase = new MapDatabase();
        mapDatabase.NewMap();
        var variable = mapDatabase.APIGetVariable("key1");
        Assert.Equal("", variable);
        mapDatabase.APIInsertVariables([new (){
            Key = "key1",
            Value = "value1",
            Group = "group1",
            Desc = "desc1",
        }]);
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();

        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/getvariable", new InputKey() { Key = "key1" });
        var result = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("value1", result);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/getvariable", new InputKey() { Key = "keynotfound" });
        result = JsonSerializer.Deserialize(resp, typeof(string), APIJsonSerializerContext.Default) as string ?? "";
        Assert.Equal("", result);
        server.Stop();
    }
    [Fact]
    public async Task TestAPIGetRoom()
    {
        var mapDatabase = new MapDatabase();
        var ctx = new Context().WithRooms([
            new Room()
            {
                Key = "ctx1",
                Group="ctxroom",
            },
            new Room()
            {
                Key = "ctx2",
                Group="ctxroom",
            },
        ]);
        var opt = new MapperOptions();
        var server = new HellMapManager.Services.API.APIServer();
        server.BindMapDatabase(mapDatabase);
        server.Start();
        var resp = await Post($"http://localhost:{server.Port}" + "/api/db/getroom", new InputGetRoom()
        {
            Key = "key1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        var result = JsonSerializer.Deserialize(resp, typeof(RoomModel), APIJsonSerializerContext.Default) as RoomModel;
        Assert.Null(result);
        mapDatabase.NewMap();
        mapDatabase.APIInsertRooms([
            new Room()
            {
                Key = "key1"
            },
            new Room()
            {
                Key = "ctx2"
            },
        ]);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/getroom", new InputGetRoom()
        {
            Key = "ctx1",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        result = JsonSerializer.Deserialize(resp, typeof(RoomModel), APIJsonSerializerContext.Default) as RoomModel;
        Assert.NotNull(result);
        Assert.Equal("ctx1", result!.Key);
        Assert.Equal("ctxroom", result.Group);
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/getroom", new InputGetRoom()
        {
            Key = "ctx2",
            Environment = EnvironmentModel.From(ctx.ToEnvironment()),
            Options = MapperOptionsModel.From(opt),
        });
        result = JsonSerializer.Deserialize(resp, typeof(RoomModel), APIJsonSerializerContext.Default) as RoomModel;
        Assert.NotNull(result);
        Assert.Equal("ctx2", result!.Key);
        Assert.Equal("ctxroom", result.Group);
        server.Stop();
        return;
    }
}