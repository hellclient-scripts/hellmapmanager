namespace HellMapManager.Adapters;

public class SystemAdapter
{
    public static readonly SystemAdapter Instance = new();
    public IFileAdapter File = new FileAdapter();
}

