namespace HellMapManager.Adapters;

public class SystemAdapter
{
    public static readonly SystemAdapter Instance = new();
    public void Reset()
    {
        File = new FileAdapter();
    }
    public IFileAdapter File = new FileAdapter();
}

