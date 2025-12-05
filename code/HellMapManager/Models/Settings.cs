using System.Collections.Generic;

namespace HellMapManager.Models;


public class Settings
{
    public static string CurrentVersion { get; set; } = "1.0";
    public List<RecentFile> Recents { get; set; } = [];
    public string BindPort { get; set; } = "8466";
    public string APIUserName { get; set; } = "";
    public string APIPassWord { get; set; } = "";
    public bool APIEnabled { get; set; } = false;
    public string BuildURL()=> $"http://localhost:{BindPort}/";
}