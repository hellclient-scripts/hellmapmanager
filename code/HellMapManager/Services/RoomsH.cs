using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using HellMapManager.Models;
using System;

namespace HellMapManager.Services;

public class RoomsH
{
    public static List<Room> Open(string file)
    {
        using var fileStream = new FileStream(file, FileMode.Open);
        var body = new byte[fileStream.Length];
        fileStream.ReadAsync(body, 0, (int)fileStream.Length);
        var rooms = Load(Encoding.UTF8.GetString(body));
        return rooms;
    }
    public static List<Room> Load(string data)
    {
        List<Room> result = [];
        var lines = data.Split("\n");
        foreach (string line in lines)
        {
            var linedata = line.Trim();
            if (linedata == "" || linedata.StartsWith("//"))
            {
                continue;
            }
            var room = RoomFormatter.DecodeRoom(linedata);
            if (room is not null)
            {
                result.Add(room);
            }
        }
        return result;
    }
}