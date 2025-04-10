using System.Collections.Generic;
namespace HellMapManager.Models;
public partial class Snapshot
{
    public string Key { get; set; } = "";
    public int Timestamp = 0;
    public string Group { get; set; } = "";

    public List<RoomData> Data { get; set; } = [];
}