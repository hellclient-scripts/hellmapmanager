using HellMapManager;
using HellMapManager.Models;
using HellMapManager.States;
using HellMapManager.ViewModels;
namespace TestProject;

public class ViewModelTest
{
    [Fact]
    public void TestMainWindowViewModel()
    {
        var vm = new MainWindowViewModel();
        string? lastchanged;
        vm.PropertyChanged += (sender, args) =>
        {
            lastchanged = args.PropertyName;
        };
        Assert.Equal("Hell Map Manager", vm.TitleInfo);
        Assert.Equal(3, MainWindowViewModel.HelpLinks.Count);
        Assert.True(vm.CanShowWelcome);
        Assert.False(vm.IsFileOpend);
        Assert.Empty(vm.FilteredRooms);
        Assert.Equal(0, vm.GetMapRoomsCount);
        Assert.Empty(vm.FilteredMarkers);
        Assert.Equal(0, vm.GetMapMarkersCount);
        Assert.Empty(vm.FilteredRoutes);
        Assert.Equal(0, vm.GetMapRoutesCount);
        Assert.Empty(vm.FilteredTraces);
        Assert.Equal(0, vm.GetMapTracesCount);
        Assert.Empty(vm.FilteredRegions);
        Assert.Equal(0, vm.GetMapRegionsCount);
        Assert.Empty(vm.FilteredLandmarks);
        Assert.Equal(0, vm.GetMapLandmarksCount);
        Assert.Empty(vm.FilteredShortcuts);
        Assert.Equal(0, vm.GetMapShortcutsCount);
        Assert.Empty(vm.FilteredVariables);
        Assert.Equal(0, vm.GetMapVariablesCount);
        Assert.Empty(vm.FilteredSnapshots);
        Assert.Equal(0, vm.GetMapSnapshotsCount);

        Assert.Equal("", vm.GetMapPathLabel);
        Assert.Equal("", vm.GetMapNameLabel);
        Assert.Equal("", vm.GetMapEncodingLabel);
        Assert.Equal("", vm.LastModifiedLabel);
        Assert.Equal("", vm.GetMapDescLabel);
        Assert.True(vm.IsMapNameEmpty);
        Assert.True(vm.IsMapPathEmpty);
        Assert.True(vm.IsMapDescEmpty);

        lastchanged = null;
        vm.FilterRooms();
        Assert.Equal(nameof(vm.FilteredRooms), lastchanged);
        lastchanged = null;
        vm.FilterMarkers();
        Assert.Equal(nameof(vm.FilteredMarkers), lastchanged);
        lastchanged = null;
        vm.FilterRoutes();
        Assert.Equal(nameof(vm.FilteredRoutes), lastchanged);
        lastchanged = null;
        vm.FilterTraces();
        Assert.Equal(nameof(vm.FilteredTraces), lastchanged);
        lastchanged = null;
        vm.FilterRegions();
        Assert.Equal(nameof(vm.FilteredRegions), lastchanged);
        lastchanged = null;
        vm.FilterLandmarks();
        Assert.Equal(nameof(vm.FilteredLandmarks), lastchanged);
        lastchanged = null;
        vm.FilterShortcuts();
        Assert.Equal(nameof(vm.FilteredShortcuts), lastchanged);
        lastchanged = null;
        vm.FilterVariables();
        Assert.Equal(nameof(vm.FilteredVariables), lastchanged);
        lastchanged = null;
        vm.FilterSnapshots();
        Assert.Equal(nameof(vm.FilteredSnapshots), lastchanged);

        AppState.Main.NewMap();
        Assert.Equal("<未保存>", vm.GetMapPathLabel);
        Assert.Equal("<未命名>", vm.GetMapNameLabel);
        Assert.Equal("UTF8", vm.GetMapEncodingLabel);
        Assert.NotEqual("", vm.LastModifiedLabel);
        Assert.Equal("<无描述>", vm.GetMapDescLabel);
        Assert.True(vm.IsMapNameEmpty);
        Assert.True(vm.IsMapPathEmpty);
        Assert.True(vm.IsMapDescEmpty);

        Assert.Equal("* <未保存> Hell Map Manager", vm.TitleInfo);
        AppState.Main.Current!.Path = "/path";
        Assert.Equal("* /path Hell Map Manager", vm.TitleInfo);
        AppState.Main.Current.Modified = false;
        Assert.Equal("/path Hell Map Manager", vm.TitleInfo);
        Assert.False(vm.CanShowWelcome);
        Assert.True(vm.IsFileOpend);
        Assert.Empty(vm.Recents);
        AppState.Main.AddRecent(new RecentFile("name", "path"));
        Assert.Single(vm.Recents);
        Assert.Equal("name", vm.Recents[0].Name);
        Assert.Equal("path", vm.Recents[0].Path);

        Assert.Equal("/path", vm.GetMapPathLabel);
        AppState.Main.Current.Map.Info.Name = "name";
        Assert.Equal("name", vm.GetMapNameLabel);
        AppState.Main.Current.Map.Encoding = MapEncoding.GB18030;
        Assert.Equal("GB18030", vm.GetMapEncodingLabel);
        AppState.Main.Current.Map.Info.Desc = "desc";
        Assert.Equal("desc", vm.GetMapDescLabel);
        Assert.False(vm.IsMapNameEmpty);
        Assert.False(vm.IsMapPathEmpty);
        Assert.False(vm.IsMapDescEmpty);

        AppState.Main.APIInsertRooms([
            new Room(){Key="room1"},
            new Room(){Key="room2"},
        ]);
        Assert.Equal(2, vm.FilteredRooms.Count);
        vm.RoomsFilter = "room1";
        Assert.Single(vm.FilteredRooms);
        AppState.Main.APIInsertMarkers([
            new Marker(){Key="marker1",Value="room1"},
            new Marker(){Key="marker2",Value="room2"},
        ]);
        Assert.Equal(2, vm.FilteredMarkers.Count);
        vm.MarkersFilter = "marker1";
        Assert.Single(vm.FilteredMarkers);
        AppState.Main.APIInsertRoutes([
            new Route(){Key="route1"},
            new Route(){Key="route2"},
        ]);
        Assert.Equal(2, vm.FilteredRoutes.Count);
        vm.RoutesFilter = "route1";
        Assert.Single(vm.FilteredRoutes);
        AppState.Main.APIInsertTraces([
            new Trace(){Key="trace1"},
            new Trace(){Key="trace2"},
        ]);
        Assert.Equal(2, vm.FilteredTraces.Count);
        vm.TracesFilter = "trace1";
        Assert.Single(vm.FilteredTraces);

        AppState.Main.APIInsertRegions([
            new Region(){Key="region1"},
            new Region(){Key="region2"},
        ]);
        Assert.Equal(2, vm.FilteredRegions.Count);
        vm.RegionsFilter = "region1";
        Assert.Single(vm.FilteredRegions);

        AppState.Main.APIInsertLandmarks([
            new Landmark(){Key="landmark1"},
            new Landmark(){Key="landmark2"},
        ]);
        Assert.Equal(2, vm.FilteredLandmarks.Count);
        vm.LandmarksFilter = "landmark1";
        Assert.Single(vm.FilteredLandmarks);

        AppState.Main.APIInsertShortcuts([
            new Shortcut(){Key="shortcut1",Command="cmd1"},
            new Shortcut(){Key="shortcut2",Command="cmd2"},
        ]);
        Assert.Equal(2, vm.FilteredShortcuts.Count);
        vm.ShortcutsFilter = "shortcut1";
        Assert.Single(vm.FilteredShortcuts);

        AppState.Main.APIInsertVariables([
            new Variable(){Key="variable1"},
            new Variable(){Key="variable2"},
        ]);
        Assert.Equal(2, vm.FilteredVariables.Count);
        vm.VariablesFilter = "variable1";
        Assert.Single(vm.FilteredVariables);

        AppState.Main.APIInsertSnapshots([
            new Snapshot(){Key="snapshot1",Timestamp=1234567890},
            new Snapshot(){Key="snapshot2",Timestamp=1234567890},
        ]);
        Assert.Equal(2, vm.FilteredSnapshots.Count);
        vm.SnapshotsFilter = "snapshot1";
        Assert.Single(vm.FilteredSnapshots);

    }
}

