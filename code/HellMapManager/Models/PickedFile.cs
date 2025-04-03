using System;

namespace HellMapManager.Models;

public class PickedFile
{
    PickedFile(string path, string type)
    {
        Path = path;
        Type = type;
    }
    string Path = "";
    string Type = "";
}