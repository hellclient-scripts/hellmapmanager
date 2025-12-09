using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using HellMapManager.Cores;
using HellMapManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
namespace HellMapManager.Services.API;

public partial class APIServer
{

    public static APIServer Instance { get; } = new APIServer();
    private MapDatabase Database = new();
    private WebApplication? App;
    public bool Running { get => App is not null; }
    public void BindMapDatabase(MapDatabase db)
    {
        Database = db;
    }
    public APIServer()
    {
    }
    private WebApplication buildApp()
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.Logging.ClearProviders();
        var app = builder.Build();
        app.Use(MiddlewareSetHeaderServer);
        app.Use(MiddlewareBasicauth);
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            App = app;
            RaiseUpdatedEvent(this);
        });
        app.Lifetime.ApplicationStopped.Register(() =>
        {
            App = null;
            RaiseUpdatedEvent(this);
        });
        ConfigureRoutes(app);
        return app;

    }
    public int Port { get; set; } = Settings.DefaultAPIPort;
    public string UserName { get; set; } = "";
    public string PassWord { get; set; } = "";
    public void Start()
    {
        if (App is not null)
        {
            return;
        }
        var app = buildApp();
        app.Urls.Clear();
        Port = Database.Settings.GetPort();
        UserName = Database.Settings.APIUserName;
        PassWord = Database.Settings.APIPassWord;
        app.Urls.Add(Database.Settings.BuildURL());
        app.RunAsync();
    }
    public void Stop()
    {
        if (App is null)
        {
            return;
        }
        App.StopAsync();
    }

    private void ConfigureRoutes(WebApplication app)
    {

        app.Map("/api/version", APIVersion);
        app.Map("{*url}", NotFound);
        var DBAPI = app.MapGroup("/api/db");
        DBAPI.Map("/info", APIInfo);
        DBAPI.MapPost("/listrooms", APIListRooms);
        DBAPI.MapPost("/removerooms", APIRemoveRooms);
        DBAPI.MapPost("/insertrooms", APIInsertRooms);
        DBAPI.MapPost("/listmarkers", APIListMarkers);
        DBAPI.MapPost("/removemarkers", APIRemoveMarkers);
        DBAPI.MapPost("/insertmarkers", APIInsertMarkers);
        DBAPI.MapPost("/listroutes", APIListRoutes);
        DBAPI.MapPost("/removeroutes", APIRemoveRoutes);
        DBAPI.MapPost("/insertroutes", APIInsertRoutes);
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
    private async Task MiddlewareBasicauth(HttpContext ctx, RequestDelegate next)
    {
        if (UserName != "" && PassWord != "")
        {
            var header = ctx.Request.Headers.Authorization.FirstOrDefault();
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(header ?? "");
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? "");
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var requsername = credentials[0];
                var reqpassword = credentials[1];
                if (requsername != UserName || reqpassword != PassWord)
                {
                    await Unauthorized(ctx);
                    return;
                }
            }
            catch
            {
                await Unauthorized(ctx);
                return;

            }
        }
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
    public async Task Success(HttpContext ctx)
    {
        await WriteJSON(ctx, "success", 200);
    }
    public async Task Unauthorized(HttpContext ctx)
    {
        ctx.Response.StatusCode = 401;
        await ctx.Response.WriteAsync("Unauthorized");
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
    public event EventHandler? UpdatedEvent;
    public void RaiseUpdatedEvent(object? sender)
    {
        UpdatedEvent?.Invoke(sender, EventArgs.Empty);
    }

}


