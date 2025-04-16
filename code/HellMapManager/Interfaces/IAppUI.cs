namespace HellMapManager.Interfaces;
using System.Threading.Tasks;

public interface IAppUI
{
    Task<string> AskLoadFile();
    Task<string> AskImportRoomsH();
    Task<string> AskSaveAs();
    Task<bool> ConfirmModified();
    Task<bool> ConfirmImport();
}

public class DummyAppUI : IAppUI
{
    public Task<string> AskLoadFile()
    {
        return Task.FromResult("");
    }

    public Task<string> AskImportRoomsH()
    {
        return Task.FromResult("");
    }

    public Task<string> AskSaveAs()
    {
        return Task.FromResult("");
    }

    public Task<bool> ConfirmModified()
    {
        return Task.FromResult(true);
    }

    public Task<bool> ConfirmImport()
    {
        return Task.FromResult(true);
    }
}