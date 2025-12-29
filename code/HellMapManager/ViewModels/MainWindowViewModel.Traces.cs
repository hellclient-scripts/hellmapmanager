using System;
using HellMapManager.Cores;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitTraces()
    {
        AppKernel.MapDatabase.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(FilteredTraces));
        };
    }
    public string TracesFilter { get; set; } = "";
    public void FilterTraces()
    {
        OnPropertyChanged(nameof(FilteredTraces));
    }
    public ObservableCollection<Trace> FilteredTraces
    {
        get
        {
            if (AppKernel.MapDatabase.Current != null)
            {
                var models = AppKernel.MapDatabase.Current.Map.Traces;
                if (!string.IsNullOrEmpty(TracesFilter))
                {
                    HellMapManager.Utils.FilterUtil.SplitFilter(TracesFilter).ForEach(filter =>
                    {
                        models = models.FindAll(r => r.Filter(filter));
                    });
                }
                return new ObservableCollection<Trace>(models);
            }
            return [];
        }
    }

}
