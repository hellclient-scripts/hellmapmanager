using HellMapManager.Models;
using HellMapManager.Windows.NewConditionWindow;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.EditExitWindow;

public class EditExitWindowViewModel : ObservableObject
{
    public EditExitWindowViewModel(Exit? raw, ExternalValidator checker)
    {
        Raw = raw;
        Item = (raw is not null) ? new ExitForm(raw.Clone(), checker) : new ExitForm(checker);
    }
    public Exit? Raw { get; set; }
    public ExitForm Item { get; set; }
    public string Title
    {
        get => Raw is null ? "新建出口" : $"编辑出口";
    }
    public string ConditionValidator(ConditionForm form)
    {
        return "";
    }
}
