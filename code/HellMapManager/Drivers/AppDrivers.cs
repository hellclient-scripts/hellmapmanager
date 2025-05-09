using System.IO;

namespace HellMapManager.Drivers;

public class AppDrivers
{
    public static readonly IFileIODriver FileIO = new FileIODriver();
}