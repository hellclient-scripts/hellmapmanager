using System.IO;

namespace HellMapManager.Drivers;

public interface IFileIODriver
{
    Stream ReadStream(string file);
    Stream WriteStream(string file);
}

public class FileIODriver : IFileIODriver
{
    public Stream ReadStream(string name)
    {
        return new FileStream(name, FileMode.Open);
    }

    public Stream WriteStream(string name)
    {
        return new FileStream(name, FileMode.Create);
    }
}