
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
}