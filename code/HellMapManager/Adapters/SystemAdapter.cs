namespace HellMapManager.Adapters;

public class SystemAdapter
{
    public static void Reset()
    {
        File = new FileAdapter();
    }
    public static IFileAdapter File { get; set; } = new FileAdapter();
}

