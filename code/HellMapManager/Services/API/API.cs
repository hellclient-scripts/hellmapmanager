
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
    public async Task APIListRoutes(HttpContext ctx)
    {
        var option = InputListOption.FromJSON(await LoadBody(ctx));
        if (option is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var routes = Database.APIListRoutes(option!.To());
        await WriteJSON(ctx, RouteModel.FromList(routes));
    }
    public async Task APIRemoveRoutes(HttpContext ctx)
    {
        var list = KeyList.FromJSON(await LoadBody(ctx));
        if (list is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APIRemoveRoutes(list.Keys);
        await Success(ctx);
    }
    public async Task APIInsertRoutes(HttpContext ctx)
    {
        var routes = InputRoutes.FromJSON(await LoadBody(ctx));
        if (routes is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var routelist = RouteModel.ToRouteList(routes.Routes);
        Database.APIInsertRoutes(routelist);
        await Success(ctx);
    }
    public async Task APIListTraces(HttpContext ctx)
    {
        var option = InputListOption.FromJSON(await LoadBody(ctx));
        if (option is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var traces = Database.APIListTraces(option!.To());
        await WriteJSON(ctx, TraceModel.FromList(traces));
    }
    public async Task APIRemoveTraces(HttpContext ctx)
    {
        var list = KeyList.FromJSON(await LoadBody(ctx));
        if (list is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APIRemoveTraces(list.Keys);
        await Success(ctx);
    }
    public async Task APIInsertTraces(HttpContext ctx)
    {
        var traces = InputTraces.FromJSON(await LoadBody(ctx));
        if (traces is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var tracelist = TraceModel.ToTraceList(traces.Traces);
        Database.APIInsertTraces(tracelist);
        await Success(ctx);
    }
    public async Task APIListRegions(HttpContext ctx)
    {
        var option = InputListOption.FromJSON(await LoadBody(ctx));
        if (option is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var regions = Database.APIListRegions(option!.To());
        await WriteJSON(ctx, RegionModel.FromList(regions));
    }
    public async Task APIRemoveRegions(HttpContext ctx)
    {
        var list = KeyList.FromJSON(await LoadBody(ctx));
        if (list is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APIRemoveRegions(list.Keys);
        await Success(ctx);
    }
    public async Task APIInsertRegions(HttpContext ctx)
    {
        var regions = InputRegions.FromJSON(await LoadBody(ctx));
        if (regions is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var regionlist = RegionModel.ToRegionList(regions.Regions);
        Database.APIInsertRegions(regionlist);
        await Success(ctx);
    }
    public async Task APIListShortcuts(HttpContext ctx)
    {
        var option = InputListOption.FromJSON(await LoadBody(ctx));
        if (option is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var shortcuts = Database.APIListShortcuts(option!.To());
        await WriteJSON(ctx, ShortcutModel.FromList(shortcuts));
    }
    public async Task APIRemoveShortcuts(HttpContext ctx)
    {
        var list = KeyList.FromJSON(await LoadBody(ctx));
        if (list is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APIRemoveShortcuts(list.Keys);
        await Success(ctx);
    }
    public async Task APIInsertShortcuts(HttpContext ctx)
    {
        var shortcuts = InputShortcuts.FromJSON(await LoadBody(ctx));
        if (shortcuts is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var shortcutlist = ShortcutModel.ToShortcutList(shortcuts.Shortcuts);
        Database.APIInsertShortcuts(shortcutlist);
        await Success(ctx);
    }
    public async Task APIListVariables(HttpContext ctx)
    {
        var option = InputListOption.FromJSON(await LoadBody(ctx));
        if (option is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var variables = Database.APIListVariables(option!.To());
        await WriteJSON(ctx, VariableModel.FromList(variables));
    }
    public async Task APIRemoveVariables(HttpContext ctx)
    {
        var list = KeyList.FromJSON(await LoadBody(ctx));
        if (list is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APIRemoveVariables(list.Keys);
        await Success(ctx);
    }
    public async Task APIInsertVariables(HttpContext ctx)
    {
        var variables = InputVariables.FromJSON(await LoadBody(ctx));
        if (variables is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var variablelist = VariableModel.ToVariableList(variables.Variables);
        Database.APIInsertVariables(variablelist);
        await Success(ctx);
    }
    public async Task APIListLandmarks(HttpContext ctx)
    {
        var option = InputListOption.FromJSON(await LoadBody(ctx));
        if (option is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var landmarks = Database.APIListLandmarks(option!.To());
        await WriteJSON(ctx, LandmarkModel.FromList(landmarks));
    }
    public async Task APIRemoveLandmarks(HttpContext ctx)
    {
        var list = LandmarkKeyList.FromJSON(await LoadBody(ctx));
        if (list is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APIRemoveLandmarks(KeyType.ToLandmarkKeyList(list.LandmarkKeys));
        await Success(ctx);
    }
    public async Task APIInsertLandmarks(HttpContext ctx)
    {
        var landmarks = InputLandmarks.FromJSON(await LoadBody(ctx));
        if (landmarks is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var landmarklist = LandmarkModel.ToLandmarkList(landmarks.Landmarks);
        Database.APIInsertLandmarks(landmarklist);
        await Success(ctx);
    }
}