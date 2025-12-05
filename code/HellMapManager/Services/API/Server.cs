using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;
using HellMapManager.Cores;
using HellMapManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
namespace HellMapManager.Services.API;

public partial class APIServer
{

    public static APIServer Instance { get; } = new APIServer();
    private MapDatabase Database = new();
    private WebApplication App;
    public bool Running { get; set; } = false;
    public void BindMapDatabase(MapDatabase db)
    {
        Database = db;
    }
    public APIServer()
    {
        var builder = WebApplication.CreateSlimBuilder();
        var app = builder.Build();
        App = app;
        app.Use(MiddlewareSetHeaderServer);
        app.Lifetime.ApplicationStarted.Register(() => { Running = true; });
        app.Lifetime.ApplicationStopped.Register(() => { Running = false; });
        ConfigureRoutes();
    }
    public int Port { get; set; } = Settings.DefaultAPIPort;
    public void Start()
    {
        App.Urls.Clear();
        Port = Database.Settings.GetPort();
        App.Urls.Add(Database.Settings.BuildURL());
        App.RunAsync();
    }
    public void Stop()
    {
        App.StopAsync();
    }

    private void ConfigureRoutes()
    {
        App.MapGet("/api/version", APIVersion);
        App.Map("{*url}", NotFound);
        var DBAPI = App.MapGroup("/api/db");
        DBAPI.Map("/info", APIInfo);
        DBAPI.MapPost("/listrooms", APIListRooms);
    }
    private readonly APIJsonSerializerContext jsonctx = new(new JsonSerializerOptions()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    });
    private readonly Encoding gb18030Encoding = System.Text.Encoding.GetEncoding("GB18030");
    private async Task MiddlewareSetHeaderServer(HttpContext ctx, RequestDelegate next)
    {
        ctx.Response.Headers["Server"] = "HellMapManager";
        await next(ctx);
    }
    private async Task<string> LoadBody(HttpContext ctx)
    {
        var enc = Encoding.UTF8;
        if (ctx.Request.Headers.TryGetValue("charset", out var charset) && charset.ToString().ToLower() == "gb18030")
        {
            enc = gb18030Encoding;
        }
        using var reader = new StreamReader(ctx.Request.Body, enc);
        return await reader.ReadToEndAsync();
    }
    private async Task WriteJSON(HttpContext ctx, object? obj, int statusCode = 200)
    {
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = statusCode;
        var enc = Encoding.UTF8;
        var json = JsonSerializer.Serialize(obj, typeof(object), jsonctx);
        if (ctx.Request.Headers.TryGetValue("charset", out var charset) && charset.ToString().ToLower() == "gb18030")
        {
            enc = gb18030Encoding;
        }

        await ctx.Response.WriteAsync(json, enc);
    }
    public async Task NotFound(HttpContext ctx)
    {
        ctx.Response.StatusCode = 404;
        await ctx.Response.WriteAsync("Not Found");
    }
    public async Task InvalidJSONRequest(HttpContext ctx)
    {
        await BadRequest(ctx, "Invalid JSON");
    }

    public async Task BadRequest(HttpContext ctx, string msg)
    {
        ctx.Response.StatusCode = 400;
        await ctx.Response.WriteAsync(msg == "" ? "Bad Request" : msg);
    }
}


