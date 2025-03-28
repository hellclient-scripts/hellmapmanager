using System.Collections.Generic;

namespace HellMapManager.Models;

public class Settings
{
    public static string CurrentVersion = "1.0";
    public List<string> Recents = [];
    public string BindPort = "8466";
    public string APIUserName = "";
    public string APIPassWord = "";
    public bool APIEnabled = false;
}