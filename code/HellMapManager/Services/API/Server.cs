using System.Text.Json.Serialization;
using HellMapManager.Cores;
using HellMapManager.Models;
using Microsoft.AspNetCore.Builder;
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
    public int Port{get;set;}=Settings.DefaultAPIPort;
    public void Start()
    {
        App.Urls.Clear();
        Port=Database.Settings.GetPort();
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
        var DBAPI=App.MapGroup("/api/db");
        DBAPI.MapGet("/info", APIInfo);
    }

}


