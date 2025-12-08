using System.Text;
using System.Text.Json;
using HellMapManager.Cores;
using HellMapManager.Services.API;
using HellMapManager.Helpers;
using HellMapManager.Models;
using Tmds.DBus.Protocol;
using Microsoft.Msagl.Layout.Layered;

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
        mapDatabase.SetCurrent(HellMapManager.Models.MapFile.Create("name","desc"));
        resp = await Post($"http://localhost:{server.Port}" + "/api/db/info", "");
        var result2 = (JsonSerializer.Deserialize(resp, typeof(APIResultInfo), APIJsonSerializerContext.Default) as APIResultInfo);
        Assert.NotNull(result2);
        Assert.Equal("name", result2.Name);
        Assert.Equal("desc", result2.Desc);
        server.Stop();
    }

}