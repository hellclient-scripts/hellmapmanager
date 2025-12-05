
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Text;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Msagl.Layout.Incremental;
using System.IO;

namespace HellMapManager.Services.API;

public partial class APIServer
{
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
        using var reader = new StreamReader(ctx.Request.Body, Encoding.UTF8);
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
    public async Task APIVersion(HttpContext ctx)
    {
        await WriteJSON(ctx, Database.APIVersion());
    }
    public async Task APIInfo(HttpContext ctx)
    {
        var info = APIResultInfo.From(Database.APIInfo());
        await WriteJSON(ctx, info);
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
    public async Task APIListRooms(HttpContext ctx)
    {
        var option = InputListOption.FromJSON(await LoadBody(ctx));
        if (option is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var rooms = Database.APIListRooms(option!.To());
        await WriteJSON(ctx, RoomModel.FromList(rooms));
    }
}