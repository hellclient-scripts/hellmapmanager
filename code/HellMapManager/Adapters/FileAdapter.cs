using System.IO;

namespace HellMapManager.Adapters;

public interface IFileAdapter
{
    Stream ReadStream(string file);
    Stream WriteStream(string file);
}

public class FileAdapter : IFileAdapter
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