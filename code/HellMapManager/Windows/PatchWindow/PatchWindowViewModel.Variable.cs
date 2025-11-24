using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.ViewModels;

namespace HellMapManager.Windows.PatchWindow;

public partial class PatchWindowViewModel : ViewModelBase
{
    public partial void InitVariables()
    {
        UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(FilteredVariables));
        };
    }
    public string VariablesFilter { get; set; } = "";
    public void FilterVariables()
    {
        OnPropertyChanged(nameof(FilteredVariables));
    }
    public ObservableCollection<PatchItem> FilteredVariables
    {
        get
        {
            var rooms = Patch.Variables.Items;
            if (string.IsNullOrEmpty(VariablesFilter))
            {
                return new ObservableCollection<PatchItem>(rooms);
            }
            return new ObservableCollection<PatchItem>(rooms.FindAll(r =>
            {
                if (r.Display is VariableDiff rd)
                {
                    var model = rd.Model;
                    if (model != null)
                    {
                        return model.Filter(VariablesFilter);
                    }
                }
                return false;
            }));
        }
    }
    
}
