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
