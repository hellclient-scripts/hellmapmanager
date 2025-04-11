using System.Collections.Generic;
namespace HellMapManager.Models;

public partial class Shortcut : Exit
{
    public string Key { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

    public List<string> RoomTags { get; set; } = [];
    public List<string> RoomExTags { get; set; } = [];
    public new bool Validated()
    {
        return Key != "" && base.Validated();
    }

}