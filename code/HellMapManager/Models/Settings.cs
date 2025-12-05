using System.Collections.Generic;

namespace HellMapManager.Models;


public class Settings
{
    public static string CurrentVersion { get; set; } = "1.0";
    public List<RecentFile> Recents { get; set; } = [];
    public const int DefaultAPIPort = 8466;
    public int GetPort()
    {
        return APIPort > 0 ? APIPort : DefaultAPIPort;
    }
    public int APIPort { get; set; } = DefaultAPIPort;
    public string APIUserName { get; set; } = "";
    public string APIPassWord { get; set; } = "";
    public bool APIEnabled { get; set; } = false;
    public string BuildURL() => $"http://localhost:{GetPort()}/";
}