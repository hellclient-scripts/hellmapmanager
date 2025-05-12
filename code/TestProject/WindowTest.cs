using HellMapManager;
using Avalonia;
using HellMapManager.Views;
using HellMapManager.Views.Welcome;
using HellMapManager.Views.Mapfile.Overview;
using HellMapManager.Views.Mapfile.Rooms;
using HellMapManager.Views.Mapfile.Markers;
using HellMapManager.Views.Mapfile.Routes;
using HellMapManager.Views.Mapfile.Traces;
using HellMapManager.Views.Mapfile.Regions;
using HellMapManager.Views.Mapfile.Landmarks;
using HellMapManager.Views.Mapfile.Shortcuts;
using HellMapManager.Views.Mapfile.Variables;
using HellMapManager.Views.Mapfile.Snapshots;
using HellMapManager.Windows.EditDataWindow;
using HellMapManager.Windows.EditExitWindow;
using HellMapManager.Windows.EditRoomWindow;
using HellMapManager.Windows.EditRegionWindow;
using HellMapManager.Windows.EditRegionItemWindow;
using HellMapManager.Windows.EditSnapshotWindow;
using HellMapManager.Windows.EditVariableWindow;
using HellMapManager.Windows.EditMarkerWindow;
using HellMapManager.Windows.EditRouteWindow;
using HellMapManager.Windows.EditLandmarkWindow;
using HellMapManager.Windows.EditShortcutWindow;
using HellMapManager.Windows.EditTraceWindow;
using HellMapManager.Windows.NewConditionWindow;
using HellMapManager.Windows.NewTagWindow;
using HellMapManager.Windows.PickRoomWindow;
using HellMapManager.Windows.RelationMapWindow;
using HellMapManager.Windows.UpdateMapWindow;
using HellMapManager.Windows.RoomsHExportWindow;
using HellMapManager.ViewModels;
using HellMapManager.Helpers;
using HellMapManager.Models;
namespace TestProject;

[Collection("MainState")]
public class WindowTest()
{
    [Fact]
    public void TestInit()
    {
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace().SetupWithoutStarting();
        var app = new App();
        app.Initialize();
        var mainWindow = new MainWindow() { DataContext = new MainWindowViewModel() };
        mainWindow.InitializeComponent();
        new Welcome() { DataContext = new MainWindowViewModel() };
        new Overview() { DataContext = new MainWindowViewModel() };
        new Rooms() { DataContext = new MainWindowViewModel() };
        new Markers() { DataContext = new MainWindowViewModel() };
        new Routes() { DataContext = new MainWindowViewModel() };
        new Traces() { DataContext = new MainWindowViewModel() };
        new Regions() { DataContext = new MainWindowViewModel() };
        new Landmarks() { DataContext = new MainWindowViewModel() };
        new Shortcuts() { DataContext = new MainWindowViewModel() };
        new Variables() { DataContext = new MainWindowViewModel() };
        new Snapshots() { DataContext = new MainWindowViewModel() };
        new HellMapManager.Views.Components.Condition.Condition();
        new HellMapManager.Views.Components.Exit.Exit();
        new HellMapManager.Views.Components.RoomData.RoomData();
        new HellMapManager.Views.Components.RoomPicker.RoomPicker();
        new HellMapManager.Views.Components.UpDownList.UpDownList();
        new HellMapManager.Views.Components.RegionItem.RegionItem();
        new EditDataWindow() { DataContext = new EditDataWindowViewModel(null, new EditRoomWindowViewModel(null, false).DataValidator) };
        new EditExitWindow() { DataContext = new EditExitWindowViewModel(null, new EditRoomWindowViewModel(null, false).ExitValidator) };
        new EditRoomWindow() { DataContext = new EditRoomWindowViewModel(null, false) };
        new EditRegionWindow() { DataContext = new EditRegionWindowViewModel(null, false) };
        new EditRegionItemWindow() { DataContext = new EditRegionItemWindowViewModel(null, new EditRegionWindowViewModel(null, false).RegionItemValidator) };
        new EditSnapshotWindow() { DataContext = new EditSnapshotWindowViewModel(null, false) };
        new EditVariableWindow() { DataContext = new EditVariableWindowViewModel(null, false) };
        new EditMarkerWindow() { DataContext = new EditMarkerWindowViewModel(null, false) };
        new EditRouteWindow() { DataContext = new EditRouteWindowViewModel(null, false) };
        new EditLandmarkWindow() { DataContext = new EditLandmarkWindowViewModel(null, false) };
        new EditShortcutWindow() { DataContext = new EditShortcutWindowViewModel(null, false) };
        new EditTraceWindow() { DataContext = new EditTraceWindowViewModel(null, false) };
        new NewConditionWindow() { DataContext = new NewConditionWindowViewModel(null, new EditShortcutWindowViewModel(null, false).ConditionValidator) };
        new NewTagWindow() { DataContext = new NewTagWindowViewModel(null, new EditRoomWindowViewModel(null, false).TagValidator) };
        new PickRoomWindow() { DataContext = new PickRoomWindowViewModel() };
        new RelationMapWindow(new RelationMapWindowViewModel(new RelationMapItem(new Room(), 0)));
        new UpdateMapWindow() { DataContext = new UpdateMapWindowViewModel(new MapSettings()) };
        new RoomsHExportWindow() { DataContext = new RoomsHExportOption() };
    }
}