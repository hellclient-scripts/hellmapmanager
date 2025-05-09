namespace HellMapManager.Cores;

public class AppKernel
{
    public static readonly AppKernel Instance = new AppKernel();
    public readonly MapDatabase MapDatabase = new MapDatabase();

}