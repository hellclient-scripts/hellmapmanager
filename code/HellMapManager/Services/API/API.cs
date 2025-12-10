
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using HellMapManager.Models;
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
    public async Task APIListSnapshots(HttpContext ctx)
    {
        var option = InputListOption.FromJSON(await LoadBody(ctx));
        if (option is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var snapshots = Database.APIListSnapshots(option!.To());
        await WriteJSON(ctx, SnapshotModel.FromList(snapshots));
    }
    public async Task APIRemoveSnapshots(HttpContext ctx)
    {
        var list = SnapshotKeyList.FromJSON(await LoadBody(ctx));
        if (list is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APIRemoveSnapshots(KeyTypeValue.ToSnapshotKeyList(list.Keys));
        await Success(ctx);
    }
    public async Task APIInsertSnapshots(HttpContext ctx)
    {
        var snapshots = InputSnapshots.FromJSON(await LoadBody(ctx));
        if (snapshots is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var snapshotlist = SnapshotModel.ToSnapshotList(snapshots.Snapshots);
        Database.APIInsertSnapshots(snapshotlist);
        await Success(ctx);
    }
    public async Task APIQueryPathAny(HttpContext ctx)
    {
        var query = InputQueryPathAny.FromJSON(await LoadBody(ctx));
        if (query is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var result = Database.APIQueryPathAny(query.From, query.Target, Context.FromEnvironment(query.Environment.ToEnvironment()), query.Options.ToMapperOptions());
        await WriteJSON(ctx, QueryResultModel.FromQueryResult(result));
    }
    public async Task APIQueryPathAll(HttpContext ctx)
    {
        var query = InputQueryPath.FromJSON(await LoadBody(ctx));
        if (query is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var result = Database.APIQueryPathAll(query.Start, query.Target, Context.FromEnvironment(query.Environment.ToEnvironment()), query.Options.ToMapperOptions());
        await WriteJSON(ctx, QueryResultModel.FromQueryResult(result));
    }
    public async Task APIQueryPathOrdered(HttpContext ctx)
    {
        var query = InputQueryPath.FromJSON(await LoadBody(ctx));
        if (query is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var result = Database.APIQueryPathOrdered(query.Start, query.Target, Context.FromEnvironment(query.Environment.ToEnvironment()), query.Options.ToMapperOptions());
        await WriteJSON(ctx, QueryResultModel.FromQueryResult(result));
    }
    public async Task APIDilate(HttpContext ctx)
    {
        var input = InputDilate.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var output = Database.APIDilate(input.Src, input.Iterations, Context.FromEnvironment(input.Environment.ToEnvironment()), input.Options.ToMapperOptions());
        await WriteJSON(ctx, output);
    }
    public async Task APITrackExit(HttpContext ctx)
    {
        var input = InputTrackExit.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var output = Database.APITrackExit(input.Start, input.Command, Context.FromEnvironment(input.Environment.ToEnvironment()), input.Options.ToMapperOptions());
        await WriteJSON(ctx, output);
    }
    public async Task APIGetVariable(HttpContext ctx)
    {
        var input = InputKey.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var variable = Database.APIGetVariable(input.Key);
        await WriteJSON(ctx, variable);
    }
    public async Task APIQueryRegionRooms(HttpContext ctx)
    {
        var input = InputKey.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var rooms = Database.APIQueryRegionRooms(input.Key);
        await WriteJSON(ctx, rooms);
    }
    public async Task APIGetRoom(HttpContext ctx)
    {
        var input = InputGetRoom.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var room = Database.APIGetRoom(input.Key, Context.FromEnvironment(input.Environment.ToEnvironment()), input.Options.ToMapperOptions());
        await WriteJSON(ctx, RoomModel.From(room));
    }
    public async Task APIClearSnapshot(HttpContext ctx)
    {

        var input = InputSnapshotFilter.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APIClearSnapshot(input.ToSnapshotFilter());
        await Success(ctx);
    }
    public async Task APITakeSnapshot(HttpContext ctx)
    {

        var input = InputTakeSnapshot.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APITakeSnapshot(input.Key, input.Type, input.Value, input.Group);
        await Success(ctx);
    }
    public async Task APISearchSnapshots(HttpContext ctx)
    {
        var input = SnapshotSearchModel.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var snapshots = Database.APISearchSnapshots(input.ToSnapshotSearch());
        await WriteJSON(ctx, SnapshotSearchResultModel.FromList(snapshots));
    }
    public async Task APISearchRooms(HttpContext ctx)
    {
        var input = InputSearchRooms.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var rooms = Database.APISearchRooms(input.Filter.ToRoomFilter());
        await WriteJSON(ctx, RoomModel.FromList(rooms));
    }
    public async Task APIFilterRooms(HttpContext ctx)
    {
        var input = InputFilterRooms.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var rooms = Database.APIFilterRooms(input.Source, input.Filter.ToRoomFilter());
        await WriteJSON(ctx, RoomModel.FromList(rooms));
    }
    public async Task APIGroupRoom(HttpContext ctx)
    {

        var input = InputGroupRoom.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APIGroupRoom(input.Room, input.Group);
        await Success(ctx);
    }
    public async Task APITagRoom(HttpContext ctx)
    {

        var input = InputTagRoom.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APITagRoom(input.Room, input.Tag, input.Value);
        await Success(ctx);
    }
    public async Task APISetRoomData(HttpContext ctx)
    {

        var input = InputSetRoomData.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APISetRoomData(input.Room, input.Key, input.Value);
        await Success(ctx);
    }
    public async Task APITraceLocation(HttpContext ctx)
    {

        var input = InputTraceLocation.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        Database.APITraceLocation(input.Key, input.Location);
        await Success(ctx);
    }
    public async Task APIGetRoomExits(HttpContext ctx)
    {
        var input = InputGetRoom.FromJSON(await LoadBody(ctx));
        if (input is null)
        {
            await InvalidJSONRequest(ctx);
            return;
        }
        var exits = Database.APIGetRoomExits(input.Key, Context.FromEnvironment(input.Environment.ToEnvironment()), input.Options.ToMapperOptions());
        await WriteJSON(ctx, ExitModel.FromList(exits));
    }
}