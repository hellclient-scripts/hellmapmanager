namespace HellMapManager.Misc;

public class AppVersion(int major, int minor, int patch)
{
    public static AppVersion Current { get; } = new(0, 20251224, 0);
    public int Major { get; } = major;
    public int Minor { get; } = minor;
    public int Patch { get; } = patch;

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }
}