
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HellMapManager.Services.API;

public partial class APIServer
{
    public async Task APIVersion(HttpContext ctx)
    {
        await WriteJSON(ctx, Database.APIVersion());
    }
    public async Task APIInfo(HttpContext ctx)
    {
        var info = APIResultInfo.From(Database.APIInfo());
        await WriteJSON(ctx, info);
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
    public async Task APIRemoveRooms(HttpContext ctx)
    {
        var list = KeyList.FromJSON(await LoadBody(ctx));
        if (list is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APIRemoveRooms(list.Keys);
        await Success(ctx);
    }
    public async Task APIInsertRooms(HttpContext ctx)
    {
        var rooms = InputRooms.FromJSON(await LoadBody(ctx));
        if (rooms is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var roomlist = RoomModel.ToRoomList(rooms.Rooms);
        Database.APIInsertRooms(roomlist);
        await Success(ctx);
    }
    public async Task APIListMarkers(HttpContext ctx)
    {
        var option = InputListOption.FromJSON(await LoadBody(ctx));
        if (option is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var markers = Database.APIListMarkers(option!.To());
        await WriteJSON(ctx, MarkerModel.FromList(markers));
    }
    public async Task APIInsertMarkers(HttpContext ctx)
    {
        var markers = InputMarkers.FromJSON(await LoadBody(ctx));
        if (markers is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var markerlist = MarkerModel.ToMarkerList(markers.Markers);
        Database.APIInsertMarkers(markerlist);
        await Success(ctx);
    }
    public async Task APIRemoveMarkers(HttpContext ctx)
    {
        var list = KeyList.FromJSON(await LoadBody(ctx));
        if (list is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APIRemoveMarkers(list.Keys);
        await Success(ctx);
    }
}