using System;
using System.Runtime.InteropServices;
using System.Text;
using HellMapManager.Cores;


namespace HellMapManager.Services.API;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using HellMapManager.Models;
#if DLL
public static class Dll
{
    static Dll()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        gb18030Encoding = Encoding.GetEncoding("GB18030");
    }
    private readonly static APIJsonSerializerContext jsonctx = new(new JsonSerializerOptions()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    });
    private readonly static Encoding gb18030Encoding;

    public static IntPtr Output(object? obj, int encoding)
    {

        var json = JsonSerializer.Serialize(obj, typeof(object), jsonctx);
        json ??= "";
        byte[] data;
        try
        {
            data = encoding switch
            {
                1 => gb18030Encoding.GetBytes(json),
                _ => Encoding.UTF8.GetBytes(json),
            };
        }
        catch (Exception)
        {
            data = Encoding.UTF8.GetBytes("");
        }
        IntPtr ptr = Marshal.AllocHGlobal(data.Length + 1);
        Marshal.Copy(data, 0, ptr, data.Length);
        Marshal.WriteByte(ptr + data.Length, 0);
        return ptr;
    }
    public const int MaxInputSize = 1024 * 1024 * 1024;
    public static unsafe string Input(IntPtr ptr, int encoding)
    {
        if (ptr == IntPtr.Zero) return "";
        try
        {
            byte* pByte = (byte*)ptr;
            int length = 0;
            while (pByte[length] != 0 && length < MaxInputSize)
            {
                length++;
            }
            var data = new byte[length];
            Marshal.Copy(ptr, data, 0, length);
            var str = encoding switch
            {
                1 => gb18030Encoding.GetString(data) ?? "",
                _ => Encoding.UTF8.GetString(data) ?? "",
            };
            return str;
        }
        catch (Exception)
        {
            return "";
        }
        finally
        {
            // Marshal.FreeHGlobal(ptr);
        }
    }
    public static byte[] Raw(IntPtr ptr, int encoding)
    {
        if (ptr == IntPtr.Zero) return [];
        try
        {
            int length = 0;
            while (Marshal.ReadByte(ptr, length) != 0)
            {
                length++;
            }

            byte[] destination = new byte[length];
            Marshal.Copy(ptr, destination, 0, length);
            return destination;
        }
        catch (Exception)
        {
            return [];
        }
        finally
        {
            // Marshal.FreeHGlobal(ptr);
        }
    }
    public static IntPtr RawOut(string utf8str, int encoding)
    {
        byte[] data;
        try
        {
            data = encoding switch
            {
                1 => gb18030Encoding.GetBytes(utf8str),
                _ => Encoding.UTF8.GetBytes(utf8str),
            };
        }
        catch (Exception)
        {
            data = Encoding.UTF8.GetBytes("");
        }
        IntPtr ptr = Marshal.AllocHGlobal(data.Length + 1);
        Marshal.Copy(data, 0, ptr, data.Length);
        Marshal.WriteByte(ptr + data.Length, 0);
        return ptr;
    }

    [UnmanagedCallersOnly(EntryPoint = "version")]
    public static IntPtr DllVersion(IntPtr input, int encoding)
    {
        Input(input, encoding);
        return Output(AppKernel.MapDatabase.APIVersion(), encoding);
    }
    [UnmanagedCallersOnly(EntryPoint = "import")]
    public static IntPtr DllImport(IntPtr input, int encoding)
    {
        try
        {
            var data = Raw(input, encoding);

            var result = AppKernel.MapDatabase.Import(data);
            return Output(result, encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "export")]
    public static IntPtr DllExport(IntPtr input, int encoding)
    {

        try
        {
            Raw(input, encoding);
            var result = AppKernel.MapDatabase.Export();
            return RawOut(result, encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "close")]
    public static IntPtr DllClose(IntPtr input, int encoding)
    {
        Raw(input, encoding);
        var result = AppKernel.MapDatabase.CloseCurrent();
        return Output(result, encoding);
    }
    [UnmanagedCallersOnly(EntryPoint = "create")]
    public static IntPtr DllCreate(IntPtr input, int encoding)
    {
        Raw(input, encoding);
        var result = AppKernel.MapDatabase.Create();
        return Output(result, encoding);
    }
    [UnmanagedCallersOnly(EntryPoint = "info")]
    public static IntPtr DllInfo(IntPtr input, int encoding)
    {
        try
        {
            var info = APIResultInfo.From(AppKernel.MapDatabase.APIInfo());
            return Output(info, encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "listrooms")]
    public static IntPtr DllListRooms(IntPtr input, int encoding)
    {
        try
        {
            var option = InputListOption.FromJSON(Input(input, encoding));
            if (option is null)
            {
                return RawOut("", encoding);
            }
            var rooms = AppKernel.MapDatabase.APIListRooms(option!.To());
            return Output(rooms, encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }

    }
    [UnmanagedCallersOnly(EntryPoint = "removerooms")]
    public static IntPtr DllRemoveRooms(IntPtr input, int encoding)
    {
        try
        {
            var list = KeyList.FromJSON(Input(input, encoding));
            if (list is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APIRemoveRooms(list.Keys);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }

    }
    [UnmanagedCallersOnly(EntryPoint = "insertrooms")]
    public static IntPtr DllInsertRooms(IntPtr input, int encoding)
    {
        try
        {

            var rooms = InputRooms.FromJSON(Input(input, encoding));
            if (rooms is null)
            {
                return RawOut("", encoding);
            }
            var roomlist = RoomModel.ToRoomList(rooms.Rooms);
            AppKernel.MapDatabase.APIInsertRooms(roomlist);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "listmarkers")]
    public static IntPtr DllListMarkers(IntPtr input, int encoding)
    {
        try
        {
            var option = InputListOption.FromJSON(Input(input, encoding));
            if (option is null)
            {
                return RawOut("", encoding);
            }
            var markers = AppKernel.MapDatabase.APIListMarkers(option!.To());
            return Output(MarkerModel.FromList(markers), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "insertmarkers")]
    public static IntPtr DllInsertMarkers(IntPtr input, int encoding)
    {
        try
        {
            var markers = InputMarkers.FromJSON(Input(input, encoding));
            if (markers is null)
            {
                return RawOut("", encoding);
            }
            var markerlist = MarkerModel.ToMarkerList(markers.Markers);
            AppKernel.MapDatabase.APIInsertMarkers(markerlist);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "removemarkers")]
    public static IntPtr DllRemoveMarkers(IntPtr input, int encoding)
    {
        try
        {
            var list = KeyList.FromJSON(Input(input, encoding));
            if (list is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APIRemoveMarkers(list.Keys);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "listroutes")]
    public static IntPtr DllListRoutes(IntPtr input, int encoding)
    {
        try
        {
            var option = InputListOption.FromJSON(Input(input, encoding));
            if (option is null)
            {
                return RawOut("", encoding);
            }
            var routes = AppKernel.MapDatabase.APIListRoutes(option!.To());
            return Output(RouteModel.FromList(routes), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "removeroutes")]
    public static IntPtr DllRemoveRoutes(IntPtr input, int encoding)
    {
        try
        {
            var list = KeyList.FromJSON(Input(input, encoding));
            if (list is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APIRemoveRoutes(list.Keys);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "insertroutes")]
    public static IntPtr DllInsertRoutes(IntPtr input, int encoding)
    {
        try
        {
            var routes = InputRoutes.FromJSON(Input(input, encoding));
            if (routes is null)
            {
                return RawOut("", encoding);
            }
            var routelist = RouteModel.ToRouteList(routes.Routes);
            AppKernel.MapDatabase.APIInsertRoutes(routelist);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "listtraces")]
    public static IntPtr DllListTraces(IntPtr input, int encoding)
    {
        try
        {
            var option = InputListOption.FromJSON(Input(input, encoding));
            if (option is null)
            {
                return RawOut("", encoding);
            }
            var traces = AppKernel.MapDatabase.APIListTraces(option!.To());
            return Output(TraceModel.FromList(traces), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "removetraces")]
    public static IntPtr DllRemoveTraces(IntPtr input, int encoding)
    {
        try
        {
            var list = KeyList.FromJSON(Input(input, encoding));
            if (list is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APIRemoveTraces(list.Keys);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "inserttraces")]
    public static IntPtr DllInsertTraces(IntPtr input, int encoding)
    {
        try
        {
            var traces = InputTraces.FromJSON(Input(input, encoding));
            if (traces is null)
            {
                return RawOut("", encoding);
            }
            var tracelist = TraceModel.ToTraceList(traces.Traces);
            AppKernel.MapDatabase.APIInsertTraces(tracelist);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "listregions")]
    public static IntPtr DllListRegions(IntPtr input, int encoding)
    {
        try
        {
            var option = InputListOption.FromJSON(Input(input, encoding));
            if (option is null)
            {
                return RawOut("", encoding);
            }
            var regions = AppKernel.MapDatabase.APIListRegions(option!.To());
            return Output(RegionModel.FromList(regions), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "removeregions")]
    public static IntPtr DllRemoveRegions(IntPtr input, int encoding)
    {
        try
        {
            var list = KeyList.FromJSON(Input(input, encoding));
            if (list is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APIRemoveRegions(list.Keys);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "insertregions")]
    public static IntPtr DllInsertRegions(IntPtr input, int encoding)
    {
        try
        {
            var regions = InputRegions.FromJSON(Input(input, encoding));
            if (regions is null)
            {
                return RawOut("", encoding);
            }
            var regionlist = RegionModel.ToRegionList(regions.Regions);
            AppKernel.MapDatabase.APIInsertRegions(regionlist);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "listshortcuts")]
    public static IntPtr DllListShortcuts(IntPtr input, int encoding)
    {
        try
        {
            var option = InputListOption.FromJSON(Input(input, encoding));
            if (option is null)
            {
                return RawOut("", encoding);
            }
            var shortcuts = AppKernel.MapDatabase.APIListShortcuts(option!.To());
            return Output(ShortcutModel.FromList(shortcuts), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "removeshortcuts")]
    public static IntPtr DllRemoveShortcuts(IntPtr input, int encoding)
    {
        try
        {
            var list = KeyList.FromJSON(Input(input, encoding));
            if (list is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APIRemoveShortcuts(list.Keys);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "insertshortcuts")]
    public static IntPtr DllInsertShortcuts(IntPtr input, int encoding)
    {
        try
        {
            var shortcuts = InputShortcuts.FromJSON(Input(input, encoding));
            if (shortcuts is null)
            {
                return RawOut("", encoding);
            }
            var shortcutlist = ShortcutModel.ToShortcutList(shortcuts.Shortcuts);
            AppKernel.MapDatabase.APIInsertShortcuts(shortcutlist);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "listvariables")]
    public static IntPtr DllListVariables(IntPtr input, int encoding)
    {
        try
        {
            var option = InputListOption.FromJSON(Input(input, encoding));
            if (option is null)
            {
                return RawOut("", encoding);
            }
            var variables = AppKernel.MapDatabase.APIListVariables(option!.To());
            return Output(VariableModel.FromList(variables), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "removevariables")]
    public static IntPtr DllRemoveVariables(IntPtr input, int encoding)
    {
        try
        {
            var list = KeyList.FromJSON(Input(input, encoding));
            if (list is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APIRemoveVariables(list.Keys);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "insertvariables")]
    public static IntPtr DllInsertVariables(IntPtr input, int encoding)
    {
        try
        {
            var variables = InputVariables.FromJSON(Input(input, encoding));
            if (variables is null)
            {
                return RawOut("", encoding);
            }
            var variablelist = VariableModel.ToVariableList(variables.Variables);
            AppKernel.MapDatabase.APIInsertVariables(variablelist);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "listlandmarks")]
    public static IntPtr DllListLandmarks(IntPtr input, int encoding)
    {
        try
        {
            var option = InputListOption.FromJSON(Input(input, encoding));
            if (option is null)
            {
                return RawOut("", encoding);
            }
            var landmarks = AppKernel.MapDatabase.APIListLandmarks(option!.To());
            return Output(LandmarkModel.FromList(landmarks), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "removelandmarks")]
    public static IntPtr DllRemoveLandmarks(IntPtr input, int encoding)
    {
        try
        {
            var list = LandmarkKeyList.FromJSON(Input(input, encoding));
            if (list is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APIRemoveLandmarks(KeyType.ToLandmarkKeyList(list.LandmarkKeys));
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "insertlandmarks")]
    public static IntPtr DllInsertLandmarks(IntPtr input, int encoding)
    {
        try
        {
            var landmarks = InputLandmarks.FromJSON(Input(input, encoding));
            if (landmarks is null)
            {
                return RawOut("", encoding);
            }
            var landmarklist = LandmarkModel.ToLandmarkList(landmarks.Landmarks);
            AppKernel.MapDatabase.APIInsertLandmarks(landmarklist);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "listsnapshots")]
    public static IntPtr DllListSnapshots(IntPtr input, int encoding)
    {
        try
        {

            var option = InputListOption.FromJSON(Input(input, encoding));
            if (option is null)
            {
                return RawOut("", encoding);
            }
            var snapshots = AppKernel.MapDatabase.APIListSnapshots(option!.To());
            return Output(SnapshotModel.FromList(snapshots), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }

    }
    [UnmanagedCallersOnly(EntryPoint = "removesnapshots")]
    public static IntPtr DllRemoveSnapshots(IntPtr input, int encoding)
    {
        try
        {
            var list = SnapshotKeyList.FromJSON(Input(input, encoding));
            if (list is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APIRemoveSnapshots(KeyTypeValue.ToSnapshotKeyList(list.Keys));
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }

    }
    [UnmanagedCallersOnly(EntryPoint = "insertsnapshots")]
    public static IntPtr DllInsertSnapshots(IntPtr input, int encoding)
    {
        try
        {
            var snapshots = InputSnapshots.FromJSON(Input(input, encoding));
            if (snapshots is null)
            {
                return RawOut("", encoding);
            }
            var snapshotlist = SnapshotModel.ToSnapshotList(snapshots.Snapshots);
            AppKernel.MapDatabase.APIInsertSnapshots(snapshotlist);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "querypathany")]
    public static IntPtr DllQueryPathAny(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var query = InputQueryPathAny.FromJSON(data);
            if (query is null)
            {
                return RawOut("", encoding);
            }
            var result = AppKernel.MapDatabase.APIQueryPathAny(query.From, query.Target, Context.FromEnvironment(query?.Environment?.ToEnvironment()), query?.Options?.ToMapperOptions());
            return Output(QueryResultModel.FromQueryResult(result), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "querypathall")]
    public static IntPtr DllQueryPathAll(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var query = InputQueryPath.FromJSON(data);
            if (query is null)
            {
                return RawOut("", encoding);
            }
            var result = AppKernel.MapDatabase.APIQueryPathAll(query.Start, query.Target, Context.FromEnvironment(query?.Environment?.ToEnvironment()), query?.Options?.ToMapperOptions());
            return Output(QueryResultModel.FromQueryResult(result), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "querypathordered")]
    public static IntPtr DllQueryPathOrdered(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var query = InputQueryPath.FromJSON(data);
            if (query is null)
            {
                return RawOut("", encoding);
            }
            var result = AppKernel.MapDatabase.APIQueryPathOrdered(query.Start, query.Target, Context.FromEnvironment(query?.Environment?.ToEnvironment()), query?.Options?.ToMapperOptions());
            return Output(QueryResultModel.FromQueryResult(result), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "dilate")]
    public static IntPtr DllDilate(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var dilateInput = InputDilate.FromJSON(data);
            if (dilateInput is null)
            {
                return RawOut("", encoding);
            }
            var output = AppKernel.MapDatabase.APIDilate(dilateInput.Source, dilateInput.Iterations, Context.FromEnvironment(dilateInput?.Environment?.ToEnvironment()), dilateInput?.Options?.ToMapperOptions());
            return Output(output, encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "trackexit")]
    public static IntPtr DllTrackExit(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var trackExitInput = InputTrackExit.FromJSON(data);
            if (trackExitInput is null)
            {
                return RawOut("", encoding);
            }
            var output = AppKernel.MapDatabase.APITrackExit(trackExitInput.Start, trackExitInput.Command, Context.FromEnvironment(trackExitInput?.Environment?.ToEnvironment()), trackExitInput?.Options?.ToMapperOptions());
            return Output(output, encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "getvariable")]
    public static IntPtr DllGetVariable(IntPtr input, int encoding)
    {
        try
        {

            var data = Input(input, encoding);
            var inputKey = InputKey.FromJSON(data);
            if (inputKey is null)
            {
                return RawOut("", encoding);
            }
            var variable = AppKernel.MapDatabase.APIGetVariable(inputKey.Key);
            return Output(variable, encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "queryregionrooms")]
    public static IntPtr DllQueryRegionRooms(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputKey = InputKey.FromJSON(data);
            if (inputKey is null)
            {
                return RawOut("", encoding);
            }
            var rooms = AppKernel.MapDatabase.APIQueryRegionRooms(inputKey.Key);
            return Output(rooms, encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "getroom")]
    public static IntPtr DllGetRoom(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputKey = InputGetRoom.FromJSON(data);
            if (inputKey is null)
            {
                return RawOut("", encoding);
            }
            var room = AppKernel.MapDatabase.APIGetRoom(inputKey.Key, Context.FromEnvironment(inputKey?.Environment?.ToEnvironment()), inputKey?.Options?.ToMapperOptions());
            return Output(RoomModel.From(room), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "clearsnapshots")]
    public static IntPtr DllClearSnapshots(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputFilter = InputSnapshotFilter.FromJSON(data);
            if (inputFilter is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APIClearSnapshots(inputFilter.ToSnapshotFilter());
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "takesnapshot")]
    public static IntPtr DllTakeSnapshot(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputTakeSnapshot = InputTakeSnapshot.FromJSON(data);
            if (inputTakeSnapshot is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APITakeSnapshot(inputTakeSnapshot.Key, inputTakeSnapshot.Type, inputTakeSnapshot.Value, inputTakeSnapshot.Group);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "searchsnapshots")]
    public static IntPtr DllSearchSnapshots(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputSearchSnapshots = SnapshotSearchModel.FromJSON(data);
            if (inputSearchSnapshots is null)
            {
                return RawOut("", encoding);
            }
            var snapshots = AppKernel.MapDatabase.APISearchSnapshots(inputSearchSnapshots.ToSnapshotSearch());
            return Output(SnapshotSearchResultModel.FromList(snapshots), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "searchrooms")]
    public static IntPtr DllSearchRooms(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputSearchRooms = InputSearchRooms.FromJSON(data);
            if (inputSearchRooms is null)
            {
                return RawOut("", encoding);
            }
            var rooms = AppKernel.MapDatabase.APISearchRooms(inputSearchRooms.Filter.ToRoomFilter());
            return Output(RoomModel.FromList(rooms), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "filterrooms")]
    public static IntPtr DllFilterRooms(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputFilterRooms = InputFilterRooms.FromJSON(data);
            if (inputFilterRooms is null)
            {
                return RawOut("", encoding);
            }
            var rooms = AppKernel.MapDatabase.APIFilterRooms(inputFilterRooms.Source, inputFilterRooms.Filter.ToRoomFilter());
            return Output(RoomModel.FromList(rooms), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "grouproom")]
    public static IntPtr DllGroupRoom(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputGroupRoom = InputGroupRoom.FromJSON(data);
            if (inputGroupRoom is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APIGroupRoom(inputGroupRoom.Room, inputGroupRoom.Group);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "tagroom")]
    public static IntPtr DllTagRoom(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputTagRoom = InputTagRoom.FromJSON(data);
            if (inputTagRoom is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APITagRoom(inputTagRoom.Room, inputTagRoom.Tag, inputTagRoom.Value);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "setroomdata")]
    public static IntPtr DllSetRoomData(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputSetRoomData = InputSetRoomData.FromJSON(data);
            if (inputSetRoomData is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APISetRoomData(inputSetRoomData.Room, inputSetRoomData.Key, inputSetRoomData.Value);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "tracelocation")]
    public static IntPtr DllTraceLocation(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputTraceLocation = InputTraceLocation.FromJSON(data);
            if (inputTraceLocation is null)
            {
                return RawOut("", encoding);
            }
            AppKernel.MapDatabase.APITraceLocation(inputTraceLocation.Key, inputTraceLocation.Location);
            return RawOut("ok", encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
    [UnmanagedCallersOnly(EntryPoint = "getroomexits")]
    public static IntPtr DllGetRoomExits(IntPtr input, int encoding)
    {
        try
        {
            var data = Input(input, encoding);
            var inputGetRoom = InputGetRoom.FromJSON(data);
            if (inputGetRoom is null)
            {
                return RawOut("", encoding);
            }
            var exits = AppKernel.MapDatabase.APIGetRoomExits(inputGetRoom.Key, Context.FromEnvironment(inputGetRoom?.Environment?.ToEnvironment()), inputGetRoom?.Options?.ToMapperOptions());
            return Output(ExitModel.FromList(exits), encoding);
        }
        catch (Exception)
        {
            return RawOut("", encoding);
        }
    }
}

#endif