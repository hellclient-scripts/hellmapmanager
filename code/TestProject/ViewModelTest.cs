using HellMapManager.Models;
using HellMapManager.Cores;
using HellMapManager.Windows.EditDataWindow;
using HellMapManager.Windows.EditExitWindow;
using HellMapManager.Windows.EditLandmarkWindow;
using HellMapManager.Windows.EditMarkerWindow;
using HellMapManager.Windows.EditRegionWindow;
using HellMapManager.Windows.EditRouteWindow;
using HellMapManager.Windows.EditTraceWindow;
using HellMapManager.Windows.EditVariableWindow;
using HellMapManager.Windows.EditSnapshotWindow;
using HellMapManager.Windows.EditShortcutWindow;
using HellMapManager.Windows.EditRoomWindow;
using HellMapManager.Windows.NewConditionWindow;
using HellMapManager.Windows.NewTagWindow;
using HellMapManager.Windows.PickRoomWindow;
using HellMapManager.Windows.UpdateMapWindow;
using HellMapManager.Windows.EditRegionItemWindow;
using HellMapManager.ViewModels;
namespace TestProject;

[Collection("Core")]
public class ViewModelTest
{
    [Fact]
    public void TestMainWindowViewModel()
    {
        AppKernel.MapDatabase.CloseCurrent();
        var vm = new MainWindowViewModel();
        string? lastchanged;
        vm.PropertyChanged += (sender, args) =>
        {
            lastchanged = args.PropertyName;
        };
        Assert.Equal("HellMapManager", vm.TitleInfo);
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

        AppKernel.MapDatabase.NewMap();
        Assert.Equal("<未保存>", vm.GetMapPathLabel);
        Assert.Equal("<未命名>", vm.GetMapNameLabel);
        Assert.Equal("UTF8", vm.GetMapEncodingLabel);
        Assert.NotEqual("", vm.LastModifiedLabel);
        Assert.Equal("<无描述>", vm.GetMapDescLabel);
        Assert.True(vm.IsMapNameEmpty);
        Assert.True(vm.IsMapPathEmpty);
        Assert.True(vm.IsMapDescEmpty);

        Assert.Equal("HellMapManager * <未保存>", vm.TitleInfo);
        AppKernel.MapDatabase.Current!.Path = "/path";
        Assert.Equal("HellMapManager * /path", vm.TitleInfo);
        AppKernel.MapDatabase.Current.Modified = false;
        Assert.Equal("HellMapManager /path", vm.TitleInfo);
        Assert.False(vm.CanShowWelcome);
        Assert.True(vm.IsFileOpend);
        Assert.Empty(vm.Recents);
        AppKernel.MapDatabase.AddRecent(new RecentFile("name", "path"));
        Assert.Single(vm.Recents);
        Assert.Equal("name", vm.Recents[0].Name);
        Assert.Equal("path", vm.Recents[0].Path);

        Assert.Equal("/path", vm.GetMapPathLabel);
        AppKernel.MapDatabase.Current.Map.Info.Name = "name";
        Assert.Equal("name", vm.GetMapNameLabel);
        AppKernel.MapDatabase.Current.Map.Encoding = MapEncoding.GB18030;
        Assert.Equal("GB18030", vm.GetMapEncodingLabel);
        AppKernel.MapDatabase.Current.Map.Info.Desc = "desc";
        Assert.Equal("desc", vm.GetMapDescLabel);
        Assert.False(vm.IsMapNameEmpty);
        Assert.False(vm.IsMapPathEmpty);
        Assert.False(vm.IsMapDescEmpty);

        AppKernel.MapDatabase.APIInsertRooms([
            new Room(){Key="room1"},
            new Room(){Key="room2"},
        ]);
        Assert.Equal(2, vm.FilteredRooms.Count);
        vm.RoomsFilter = "room1";
        Assert.Single(vm.FilteredRooms);
        AppKernel.MapDatabase.APIInsertMarkers([
            new Marker(){Key="marker1",Value="room1"},
            new Marker(){Key="marker2",Value="room2"},
        ]);
        Assert.Equal(2, vm.FilteredMarkers.Count);
        vm.MarkersFilter = "marker1";
        Assert.Single(vm.FilteredMarkers);
        AppKernel.MapDatabase.APIInsertRoutes([
            new Route(){Key="route1"},
            new Route(){Key="route2"},
        ]);
        Assert.Equal(2, vm.FilteredRoutes.Count);
        vm.RoutesFilter = "route1";
        Assert.Single(vm.FilteredRoutes);
        AppKernel.MapDatabase.APIInsertTraces([
            new Trace(){Key="trace1"},
            new Trace(){Key="trace2"},
        ]);
        Assert.Equal(2, vm.FilteredTraces.Count);
        vm.TracesFilter = "trace1";
        Assert.Single(vm.FilteredTraces);

        AppKernel.MapDatabase.APIInsertRegions([
            new Region(){Key="region1"},
            new Region(){Key="region2"},
        ]);
        Assert.Equal(2, vm.FilteredRegions.Count);
        vm.RegionsFilter = "region1";
        Assert.Single(vm.FilteredRegions);

        AppKernel.MapDatabase.APIInsertLandmarks([
            new Landmark(){Key="landmark1"},
            new Landmark(){Key="landmark2"},
        ]);
        Assert.Equal(2, vm.FilteredLandmarks.Count);
        vm.LandmarksFilter = "landmark1";
        Assert.Single(vm.FilteredLandmarks);

        AppKernel.MapDatabase.APIInsertShortcuts([
            new Shortcut(){Key="shortcut1",Command="cmd1"},
            new Shortcut(){Key="shortcut2",Command="cmd2"},
        ]);
        Assert.Equal(2, vm.FilteredShortcuts.Count);
        vm.ShortcutsFilter = "shortcut1";
        Assert.Single(vm.FilteredShortcuts);

        AppKernel.MapDatabase.APIInsertVariables([
            new Variable(){Key="variable1"},
            new Variable(){Key="variable2"},
        ]);
        Assert.Equal(2, vm.FilteredVariables.Count);
        vm.VariablesFilter = "variable1";
        Assert.Single(vm.FilteredVariables);

        AppKernel.MapDatabase.APIInsertSnapshots([
            new Snapshot(){Key="snapshot1",Timestamp=1234567890},
            new Snapshot(){Key="snapshot2",Timestamp=1234567890},
        ]);
        Assert.Equal(2, vm.FilteredSnapshots.Count);
        vm.SnapshotsFilter = "snapshot1";
        Assert.Single(vm.FilteredSnapshots);

    }
    [Fact]
    public void TestEditDataWindowViewModel()
    {
        var vm = new EditDataWindowViewModel(null, (DataForm form) => "");
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Null(vm.Item.Raw);
        Assert.Equal("", vm.Item.Key);
        Assert.Equal("", vm.Item.Value);
        Assert.Equal("新建数据", vm.Title);

        Assert.Equal("主键不能为空", vm.Item.Validate());
        vm.Item.Key = "key";
        Assert.Equal("值不能为空", vm.Item.Validate());
        vm.Item.Value = "value";
        Assert.Equal("", vm.Item.Validate());
        var vm2 = new EditDataWindowViewModel(null, (DataForm form) => "test error");
        Assert.Equal("test error", vm2.Item.Validate());
        var model = vm.Item.ToData();
        Assert.Equal("key", model.Key);
        Assert.Equal("value", model.Value);
        vm = new EditDataWindowViewModel(model, (DataForm form) => "");
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("value", vm.Item.Value);
        Assert.Equal("编辑数据 key", vm.Title);
    }
    [Fact]
    public void TestEditExitWindowViewModel()
    {
        var vm = new EditExitWindowViewModel(null, (ExitForm form) => "");
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Null(vm.Item.Raw);
        Assert.Equal("", vm.Item.Command);
        Assert.Equal("", vm.Item.To);
        Assert.Equal(1, vm.Item.Cost);
        Assert.Equal("新建出口", vm.Title);

        Assert.Equal("指令不能为空", vm.Item.Validate());
        vm.Item.Command = "cmd1";
        vm.Item.To = "to1";
        vm.Item.Cost = 2;
        vm.Item.Conditions.Add(new ValueCondition("key1", 0, true));
        Assert.Equal("", vm.Item.Validate());
        var vm2 = new EditExitWindowViewModel(null, (ExitForm form) => "test error");
        Assert.Equal("test error", vm2.Item.Validate());
        var model = vm.Item.ToExit();
        Assert.Equal(vm.Item.Command, model.Command);
        Assert.Equal(vm.Item.To, model.To);
        Assert.Single(model.Conditions);
        Assert.True(model.Conditions[0].Equal(vm.Item.Conditions[0]));
        Assert.Equal(vm.Item.Cost, model.Cost);
        vm = new EditExitWindowViewModel(model, (ExitForm form) => "");
        Assert.Equal(vm.Item.Command, model.Command);
        Assert.Equal(vm.Item.To, model.To);
        Assert.Single(model.Conditions);
        Assert.True(model.Conditions[0].Equal(vm.Item.Conditions[0]));
        Assert.Equal(vm.Item.Cost, model.Cost);
        Assert.Equal("编辑出口", vm.Title);

        vm.Item.Conditions.Add(new ValueCondition("key2", 0, false));
        vm.Item.Arrange();
        Assert.Equal(2, vm.Item.Conditions.Count);
        Assert.Equal("key2", vm.Item.Conditions[0].Key);
        Assert.Equal("key1", vm.Item.Conditions[1].Key);

        Assert.Equal("", vm.ConditionValidator(new ConditionForm(vm.ConditionValidator)));
        vm = new EditExitWindowViewModel(null, (ExitForm form) => "");
        model = new Exit()
        {
            Command = "cmd1",
            To = "to1",
            Cost = 2,
            Conditions = new List<ValueCondition>()
            {
                new ValueCondition("key1",0, true),
                new ValueCondition("key2",0, false),
                new ValueCondition("key3",0, true),
            }
        };
        vm = new EditExitWindowViewModel(model, (ExitForm form) => "");
        vm.Item.Arrange();
        Assert.Equal(3, vm.Item.Conditions.Count);
        Assert.Equal("key2", vm.Item.Conditions[0].Key);
        Assert.Equal("key1", vm.Item.Conditions[1].Key);
        Assert.Equal("key3", vm.Item.Conditions[2].Key);
    }
    [Fact]
    public void TestEditLandmarkWindowViewModel()
    {
        var vm = new EditLandmarkWindowViewModel(null, false);
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Key);
        Assert.Equal("", vm.Item.Value);
        Assert.Equal("", vm.Item.Type);
        Assert.Equal("", vm.Item.Group);
        Assert.Equal("", vm.Item.Desc);
        Assert.Equal("新建定位", vm.Title);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);
        vm.EnterEdit();
        Assert.False(vm.Editing);
        vm.Item.Key = "key2";

        var model = new Landmark()
        {
            Key = "key",
            Value = "value",
            Type = "type",
            Group = "group",
            Desc = "desc",
        };
        var model2 = new Landmark()
        {
            Key = "key2",
            Value = "value",
            Type = "type",
            Group = "group",
            Desc = "desc",
        };
        vm = new EditLandmarkWindowViewModel(model, true);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("value", vm.Item.Value);
        Assert.Equal("type", vm.Item.Type);
        Assert.Equal("group", vm.Item.Group);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.True(vm.ViewMode);
        Assert.True(vm.Editable);
        Assert.False(vm.Editing);
        Assert.Equal("查看定位 key", vm.Title);
        vm.EnterEdit();
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.True(vm.Editing);
        Assert.Equal("编辑定位 key", vm.Title);
        vm.Item.Key = "key2";
        vm.CancelEdit();
        Assert.Equal("key", vm.Item.Key);
        Assert.True(vm.Item.ToLandmark().Equal(model));
        vm = new EditLandmarkWindowViewModel(model, false);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("value", vm.Item.Value);
        Assert.Equal("type", vm.Item.Type);
        Assert.Equal("group", vm.Item.Group);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);

        AppKernel.MapDatabase.NewMap();
        AppKernel.MapDatabase.APIInsertLandmarks([model, model2]);
        Assert.Equal("", vm.Item.Validate());
        vm.Item.Key = "key2";
        Assert.Equal("定位主键已存在", vm.Item.Validate());
        vm.Item.Key = "";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "a\nb";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Value = "";
        vm.Item.Key = "key3";
        Assert.Equal("值不能为空", vm.Item.Validate());
        vm.Item.Value = "value3";
        Assert.Equal("", vm.Item.Validate());
        vm = new EditLandmarkWindowViewModel(null, false);
        vm.Item.Key = "key";
        vm.Item.Type = "type";
        vm.Item.Value = "value";
        Assert.Equal("定位主键已存在", vm.Item.Validate());
        vm.Item.Key = "key3";
        Assert.Equal("", vm.Item.Validate());
    }
    [Fact]
    public void TestEditMarkerWindowViewModel()
    {
        var vm = new EditMarkerWindowViewModel(null, false);
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Key);
        Assert.Equal("", vm.Item.Value);
        Assert.Equal("", vm.Item.Desc);
        Assert.Equal("", vm.Item.Group);
        Assert.Equal("", vm.Item.Message);
        Assert.Equal("新建标记", vm.Title);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);

        var model = new Marker()
        {
            Key = "key",
            Value = "value",
            Desc = "desc",
            Group = "group",
            Message = "message",
        };
        var model2 = new Marker()
        {
            Key = "key2",
            Value = "value2",
            Desc = "desc2",
            Group = "group2",
            Message = "message2",
        };
        vm = new EditMarkerWindowViewModel(model, true);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("value", vm.Item.Value);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("group", vm.Item.Group);
        Assert.Equal("group", vm.Item.Group);
        Assert.True(vm.ViewMode);
        Assert.True(vm.Editable);
        Assert.False(vm.Editing);
        Assert.Equal("查看标记 key", vm.Title);
        vm.EnterEdit();
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.True(vm.Editing);
        Assert.Equal("编辑标记 key", vm.Title);
        vm.Item.Key = "key2";
        vm.CancelEdit();
        Assert.Equal("key", vm.Item.Key);
        Assert.True(vm.Item.ToMarker().Equal(model));
        vm = new EditMarkerWindowViewModel(model, false);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("value", vm.Item.Value);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("group", vm.Item.Group);
        Assert.Equal("message", vm.Item.Message);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);


        AppKernel.MapDatabase.NewMap();
        AppKernel.MapDatabase.APIInsertMarkers([model, model2]);
        Assert.Equal("", vm.Item.Validate());
        vm.Item.Key = "key2";
        Assert.Equal("标记主键已存在", vm.Item.Validate());
        vm.Item.Key = "";
        vm.Item.Value = "";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "a\nb";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "key3";
        Assert.Equal("房间值不能为空", vm.Item.Validate());
        vm.Item.Value = "value3";
        Assert.Equal("", vm.Item.Validate());
        vm = new EditMarkerWindowViewModel(null, false);
        vm.Item.Key = "key";
        Assert.Equal("标记主键已存在", vm.Item.Validate());
        vm.Item.Key = "key3";
        vm.Item.Value = "value3";
        Assert.Equal("", vm.Item.Validate());
    }
    [Fact]
    public void TestEditRegionWindowViewModel()
    {
        var vm = new EditRegionWindowViewModel(null, false);
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Key);
        Assert.Equal("", vm.Item.Group);
        Assert.Equal("", vm.Item.Desc);
        Assert.Equal("", vm.Item.Message);
        Assert.Equal("新建地区", vm.Title);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);

        var model = new Region()
        {
            Key = "key",
            Group = "group",
            Desc = "desc",
            Message = "message",
            Items = new List<RegionItem>()
            {
                new RegionItem(RegionItemType.Room,"item1",true),
            }
        };
        var model2 = new Region()
        {
            Key = "key2",
            Group = "group2",
            Desc = "desc2",
            Message = "message2",
            Items = new List<RegionItem>()
            {
                new RegionItem(RegionItemType.Room,"item2",true),
            }
        };
        vm = new EditRegionWindowViewModel(model, true);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("group", vm.Item.Group);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("message", vm.Item.Message);
        Assert.Single(vm.Item.Items);
        Assert.True(vm.Item.Items[0].Equal(model.Items[0]));
        Assert.True(vm.ViewMode);
        Assert.True(vm.Editable);
        Assert.False(vm.Editing);
        Assert.Equal("查看地区 key", vm.Title);
        vm.EnterEdit();
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.True(vm.Editing);
        Assert.Equal("编辑地区 key", vm.Title);
        vm.Item.Key = "key2";
        vm.CancelEdit();
        Assert.Equal("key", vm.Item.Key);
        Assert.True(vm.Item.ToRegion().Equal(model));
        vm = new EditRegionWindowViewModel(model, false);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("group", vm.Item.Group);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("message", vm.Item.Message);
        Assert.Single(vm.Item.Items);
        Assert.True(vm.Item.Items[0].Equal(model.Items[0]));
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);
        AppKernel.MapDatabase.NewMap();
        AppKernel.MapDatabase.APIInsertRegions([model, model2]);
        Assert.Equal("", vm.Item.Validate());
        vm.Item.Key = "key2";
        Assert.Equal("地区主键已存在", vm.Item.Validate());
        vm.Item.Key = "";
        vm.Item.Group = "";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "a\nb";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "key3";
        Assert.Equal("", vm.Item.Validate());
        vm = new EditRegionWindowViewModel(null, false);
        vm.Item.Key = "key";
        Assert.Equal("地区主键已存在", vm.Item.Validate());
        vm.Item.Key = "key3";
        Assert.Equal("", vm.Item.Validate());

        Assert.Equal("", vm.RegionItemValidator(new RegionItemForm(vm.RegionItemValidator)));
    }
    [Fact]
    public void TestEditRouteWindowViewModel()
    {
        var vm = new EditRouteWindowViewModel(null, false);
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Key);
        Assert.Equal("", vm.Item.Group);
        Assert.Equal("", vm.Item.Desc);
        Assert.Equal("", vm.Item.Message);
        Assert.Equal("新建路线", vm.Title);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);

        var model = new Route()
        {
            Key = "key",
            Group = "group",
            Desc = "desc",
            Message = "message",
            Rooms = ["room1"]
        };
        var model2 = new Route()
        {
            Key = "key2",
            Group = "group2",
            Desc = "desc2",
            Message = "message2",
            Rooms = ["room2"]
        };
        vm = new EditRouteWindowViewModel(model, true);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("group", vm.Item.Group);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("message", vm.Item.Message);
        Assert.True(vm.Item.Rooms == string.Join("\n", model.Rooms));
        Assert.True(vm.ViewMode);
        Assert.True(vm.Editable);
        Assert.False(vm.Editing);
        Assert.Equal("查看路线 key", vm.Title);
        vm.EnterEdit();
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.True(vm.Editing);
        Assert.Equal("编辑路线 key", vm.Title);
        vm.Item.Key = "key2";
        vm.CancelEdit();
        Assert.Equal("key", vm.Item.Key);
        Assert.True(vm.Item.ToRoute().Equal(model));
        vm = new EditRouteWindowViewModel(model, false);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("group", vm.Item.Group);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("message", vm.Item.Message);
        Assert.True(vm.Item.Rooms == string.Join("\n", model.Rooms));
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);
        AppKernel.MapDatabase.NewMap();
        AppKernel.MapDatabase.APIInsertRoutes([model, model2]);
        Assert.Equal("", vm.Item.Validate());
        vm.Item.Key = "key2";
        Assert.Equal("路线主键已存在", vm.Item.Validate());
        vm.Item.Key = "";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "a\nb";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "key3";
        Assert.Equal("", vm.Item.Validate());
        vm = new EditRouteWindowViewModel(null, false);
        vm.Item.Key = "key";
        Assert.Equal("路线主键已存在", vm.Item.Validate());
        vm.Item.Key = "key3";
        Assert.Equal("", vm.Item.Validate());

        var form = new RouteForm((e) => "");
        form.Rooms = "a\n\nb\nc";
        Assert.Equal("a\nb\nc", form.Rooms);
    }
    [Fact]
    public void TestEditTraceViewModel()
    {
        var vm = new EditTraceWindowViewModel(null, false);
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Key);
        Assert.Equal("", vm.Item.Group);
        Assert.Equal("", vm.Item.Desc);
        Assert.Equal("", vm.Item.Message);
        Assert.Equal("新建足迹", vm.Title);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);

        var model = new Trace()
        {
            Key = "key",
            Group = "group",
            Desc = "desc",
            Message = "message",
            Locations = ["room1"]
        };
        var model2 = new Trace()
        {
            Key = "key2",
            Group = "group2",
            Desc = "desc2",
            Message = "message2",
            Locations = ["room2"]
        };
        vm = new EditTraceWindowViewModel(model, true);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("group", vm.Item.Group);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("message", vm.Item.Message);
        Assert.True(vm.Item.Rooms == string.Join("\n", model.Locations));
        Assert.True(vm.ViewMode);
        Assert.True(vm.Editable);
        Assert.False(vm.Editing);
        Assert.Equal("查看足迹 key", vm.Title);
        vm.EnterEdit();
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.True(vm.Editing);
        Assert.Equal("编辑足迹 key", vm.Title);
        vm.Item.Key = "key2";
        vm.CancelEdit();
        Assert.Equal("key", vm.Item.Key);
        Assert.True(vm.Item.ToTrace().Equal(model));
        vm = new EditTraceWindowViewModel(model, false);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("group", vm.Item.Group);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("message", vm.Item.Message);
        Assert.True(vm.Item.Rooms == string.Join("\n", model.Locations));
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);
        AppKernel.MapDatabase.NewMap();
        AppKernel.MapDatabase.APIInsertTraces([model, model2]);
        Assert.Equal("", vm.Item.Validate());
        vm.Item.Key = "key2";
        Assert.Equal("足迹主键已存在", vm.Item.Validate());
        vm.Item.Key = "";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "a\nb";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "key3";
        Assert.Equal("", vm.Item.Validate());
        vm = new EditTraceWindowViewModel(null, false);
        vm.Item.Key = "key";
        Assert.Equal("足迹主键已存在", vm.Item.Validate());
        vm.Item.Key = "key3";
        Assert.Equal("", vm.Item.Validate());

        var form = new TraceForm((e) => "");
        form.Rooms = "a\n\nb\nc";
        Assert.Equal("a\nb\nc", form.Rooms);
    }
    [Fact]
    public void TestVariableWindowViewModel()
    {
        var vm = new EditVariableWindowViewModel(null, false);
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Key);
        Assert.Equal("", vm.Item.Value);
        Assert.Equal("", vm.Item.Desc);
        Assert.Equal("", vm.Item.Group);
        Assert.Equal("新建变量", vm.Title);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);

        var model = new Variable()
        {
            Key = "key",
            Value = "value",
            Desc = "desc",
            Group = "group",
        };
        var model2 = new Variable()
        {
            Key = "key2",
            Value = "value2",
            Desc = "desc2",
            Group = "group2",
        };
        vm = new EditVariableWindowViewModel(model, true);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("value", vm.Item.Value);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("group", vm.Item.Group);
        Assert.True(vm.ViewMode);
        Assert.True(vm.Editable);
        Assert.False(vm.Editing);
        Assert.Equal("查看变量 key", vm.Title);
        vm.EnterEdit();
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.True(vm.Editing);
        Assert.Equal("编辑变量 key", vm.Title);
        vm.Item.Key = "key2";
        vm.CancelEdit();
        Assert.Equal("key", vm.Item.Key);
        Assert.True(vm.Item.ToVariable().Equal(model));
        vm = new EditVariableWindowViewModel(model, false);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("value", vm.Item.Value);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("group", vm.Item.Group);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);
        AppKernel.MapDatabase.NewMap();
        AppKernel.MapDatabase.APIInsertVariables([model, model2]);
        Assert.Equal("", vm.Item.Validate());
        vm.Item.Key = "key2";
        Assert.Equal("变量主键已存在", vm.Item.Validate());
        vm.Item.Key = "";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "a\nb";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "key3";
        Assert.Equal("", vm.Item.Validate());
        vm = new EditVariableWindowViewModel(null, false);
        vm.Item.Key = "key";
        Assert.Equal("变量主键已存在", vm.Item.Validate());
        vm.Item.Key = "key3";
        Assert.Equal("", vm.Item.Validate());
    }
    [Fact]
    public void TestEditShortcutWindowViewModel()
    {
        var vm = new EditShortcutWindowViewModel(null, false);
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Key);
        Assert.Equal("", vm.Item.Command);
        Assert.Equal("", vm.Item.Desc);
        Assert.Equal("", vm.Item.Group);
        Assert.Equal("新建捷径", vm.Title);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);

        var model = new Shortcut()
        {
            Key = "key",
            Command = "command",
            Desc = "desc",
            Group = "group",
        };
        var model2 = new Shortcut()
        {
            Key = "key2",
            Command = "command2",
            Desc = "desc2",
            Group = "group2",
        };
        vm = new EditShortcutWindowViewModel(model, true);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("command", vm.Item.Command);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("group", vm.Item.Group);
        Assert.True(vm.ViewMode);
        Assert.True(vm.Editable);
        Assert.False(vm.Editing);
        Assert.Equal("查看捷径 key", vm.Title);
        vm.EnterEdit();
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.True(vm.Editing);
        Assert.Equal("编辑捷径 key", vm.Title);
        vm.Item.Key = "key2";
        vm.CancelEdit();
        Assert.Equal("key", vm.Item.Key);
        Assert.True(vm.Item.ToShortcut().Equal(model));
        vm = new EditShortcutWindowViewModel(model, false);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("command", vm.Item.Command);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("group", vm.Item.Group);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);
        AppKernel.MapDatabase.NewMap();
        AppKernel.MapDatabase.APIInsertShortcuts([model, model2]);
        Assert.Equal("", vm.Item.Validate());
        vm.Item.Key = "key2";
        Assert.Equal("捷径主键已存在", vm.Item.Validate());
        vm.Item.Key = "";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "a\nb";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "key3";
        vm.Item.Command = "";
        Assert.Equal("指令不能为空", vm.Item.Validate());
        vm.Item.Command = "cmd3";
        Assert.Equal("", vm.Item.Validate());
        vm = new EditShortcutWindowViewModel(null, false);
        vm.Item.Key = "key";
        Assert.Equal("捷径主键已存在", vm.Item.Validate());
        vm.Item.Key = "key3";
        vm.Item.Command = "";
        var model3 = new Shortcut()
        {
            Key = "key3",
            Command = "command3",
            Desc = "desc3",
            Group = "group3",
            RoomConditions = new List<ValueCondition>()
            {
                new ValueCondition("rkey1",0, true),
                new ValueCondition("rkey2",0, false),
                new ValueCondition("rkey3",0, true),
            },
            Conditions = new List<ValueCondition>()
            {
                new ValueCondition("key1",0,true),
                new ValueCondition("key2",0,false),
                new ValueCondition("key3",0,true),
            }
        };
        var form = new ShortcutForm(model3, (e) => "");
        form.Arrange();
        Assert.Equal(3, form.Conditions.Count);
        Assert.Equal("key2", form.Conditions[0].Key);
        Assert.Equal("key1", form.Conditions[1].Key);
        Assert.Equal("key3", form.Conditions[2].Key);
        Assert.Equal(3, form.RoomConditions.Count);
        Assert.Equal("rkey2", form.RoomConditions[0].Key);
        Assert.Equal("rkey1", form.RoomConditions[1].Key);
        Assert.Equal("rkey3", form.RoomConditions[2].Key);
        vm = new EditShortcutWindowViewModel(model3, false);
        var cform = new ConditionForm((e) => "");
        cform.Key = "key1";
        Assert.Equal("条件已存在", vm.ConditionValidator(cform));
        cform.Key = "key4";
        Assert.Equal("", vm.ConditionValidator(cform));
        cform.Key = "rkey1";
        Assert.Equal("房间条件已存在", vm.RoomConditionValidator(cform));
        cform.Key = "rkey4";
        Assert.Equal("", vm.RoomConditionValidator(cform));
    }
    [Fact]
    public void TestEditRoomWindowViewModel()
    {
        var vm = new EditRoomWindowViewModel(null, false);
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Key);
        Assert.Equal("", vm.Item.Desc);
        Assert.Equal("", vm.Item.Group);
        Assert.Equal("新建房间", vm.Title);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);

        var model = new Room()
        {
            Key = "key",
            Desc = "desc",
            Group = "group",
            Tags = [new ValueTag("tag1", 0)],
            Data = [new Data("key1", "value1")],
            Exits = [new Exit(){
                Command="command1",
                To="to1",
                Cost=1,
                Conditions=[new ValueCondition("key1",0,true)]
            }],

        };
        var model2 = new Room()
        {
            Key = "key2",
            Desc = "desc2",
            Group = "group2",
            Tags = [new ValueTag("tag1", 0)],
            Data = [new Data("key1", "value1")],
            Exits = [new Exit(){
                Command="command1",
                To="to1",
                Cost=1,
                Conditions=[new ValueCondition("key1",0,true)]
            }],
        };
        vm = new EditRoomWindowViewModel(model, true);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("group", vm.Item.Group);
        Assert.Single(vm.Item.Tags);
        Assert.Equal("tag1", vm.Item.Tags[0].Key);
        Assert.Single(vm.Item.Data);
        Assert.True(model.Data[0].Equal(vm.Item.Data[0]));
        Assert.Single(vm.Item.Exits);
        Assert.True(model.Exits[0].Equal(vm.Item.Exits[0]));
        Assert.True(vm.ViewMode);
        Assert.True(vm.Editable);
        Assert.False(vm.Editing);
        Assert.Equal("查看房间 (key)", vm.Title);
        vm.EnterEdit();
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.True(vm.Editing);
        Assert.Equal("编辑房间 (key)", vm.Title);
        vm.Item.Key = "key2";
        vm.CancelEdit();
        Assert.Equal("key", vm.Item.Key);
        Assert.True(vm.Item.ToRoom().Equal(model));
        vm = new EditRoomWindowViewModel(model, false);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("desc", vm.Item.Desc);
        Assert.Equal("group", vm.Item.Group);
        Assert.Single(vm.Item.Tags);
        Assert.Equal("tag1", vm.Item.Tags[0].Key);
        Assert.Single(vm.Item.Data);
        Assert.True(model.Data[0].Equal(vm.Item.Data[0]));
        Assert.Single(vm.Item.Exits);
        Assert.True(model.Exits[0].Equal(vm.Item.Exits[0]));
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);
        AppKernel.MapDatabase.NewMap();
        AppKernel.MapDatabase.APIInsertRooms([model, model2]);
        Assert.Equal("", vm.Item.Validate());
        vm.Item.Key = "key2";
        Assert.Equal("房间主键已存在", vm.Item.Validate());
        vm.Item.Key = "";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "a\nb";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "key3";
        Assert.Equal("", vm.Item.Validate());
        vm = new EditRoomWindowViewModel(null, false);
        vm.Item.Key = "key";
        Assert.Equal("房间主键已存在", vm.Item.Validate());
        var model3 = new Room()
        {
            Key = "key3",
            Desc = "desc3",
            Group = "group3",
            Tags = [new ValueTag("tag2", 0), new ValueTag("tag1", 0)],
            Data = [new Data("key2", "value2"), new Data("key1", "value1")],
            Exits = [new Exit(){
                Command="command2",
                To="to2",
                Cost=1,
                Conditions=[new ValueCondition("key1",0,true),new ValueCondition("key2",0,false)]
            }],
        };
        vm = new EditRoomWindowViewModel(model3, false);
        vm.Item.Arrange();
        Assert.Equal(2, vm.Item.Data.Count);
        Assert.Equal("key1", vm.Item.Data[0].Key);
        Assert.Equal("key2", vm.Item.Data[1].Key);
        Assert.Equal(2, vm.Item.Tags.Count);
        Assert.Equal("tag1", vm.Item.Tags[0].Key);
        Assert.Equal("tag2", vm.Item.Tags[1].Key);
        Assert.Single(vm.Item.Exits);
        Assert.Equal(2, vm.Item.Exits[0].Conditions.Count);
        Assert.Equal("key2", vm.Item.Exits[0].Conditions[0].Key);
        Assert.Equal("key1", vm.Item.Exits[0].Conditions[1].Key);
        var data = new Data("key1", "value1");
        var dataform = new DataForm(data, (form) => "");
        Assert.Equal("", vm.DataValidator(dataform));
        dataform.Key = "key2";
        Assert.Equal("数据主键已存在", vm.DataValidator(dataform));
        dataform = new DataForm((form) => "");
        dataform.Key = "key2";
        Assert.Equal("数据主键已存在", vm.DataValidator(dataform));
        dataform.Key = "key3";
        Assert.Equal("", vm.DataValidator(dataform));
        var tagform = new TagForm(new ValueTag("tag1", 0), (form) => "");
        Assert.Equal("标签已存在", vm.TagValidator(tagform));
        tagform.Key = "tag3";
        Assert.Equal("", vm.TagValidator(tagform));
        var exitform = new ExitForm(new Exit(), (form) => "");
        Assert.Equal("", vm.ExitValidator(exitform));
    }
    [Fact]
    public void TestEditSnapshotWindowViewModel()
    {
        var vm = new EditSnapshotWindowViewModel(null, false);
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Key);
        Assert.Equal("新建快照", vm.Title);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);

        var model = new Snapshot()
        {
            Key = "key",
            Type = "type",
            Value = "value",
            Group = "group",
            Timestamp = 1234567890,
        };
        var model2 = new Snapshot()
        {
            Key = "key2",
            Type = "type2",
            Value = "value2",
            Group = "group2",
            Timestamp = 1234567890,

        };
        vm = new EditSnapshotWindowViewModel(model, true);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("type", vm.Item.Type);
        Assert.Equal("value", vm.Item.Value);
        Assert.Equal("group", vm.Item.Group);

        Assert.True(vm.ViewMode);
        Assert.True(vm.Editable);
        Assert.False(vm.Editing);
        Assert.Equal("查看快照 key", vm.Title);
        vm.EnterEdit();
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.True(vm.Editing);
        Assert.Equal("编辑快照 key", vm.Title);
        vm.Item.Key = "key2";
        vm.CancelEdit();
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal(model.UniqueKey().ToString(), vm.Item.UniqueKey);
        Assert.Equal(model.UniqueKey().ToString(), vm.Item.ToSnapshot().UniqueKey().ToString());
        vm = new EditSnapshotWindowViewModel(model, false);
        Assert.NotNull(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("type", vm.Item.Type);
        Assert.Equal("value", vm.Item.Value);
        Assert.Equal("group", vm.Item.Group);
        Assert.False(vm.ViewMode);
        Assert.False(vm.Editable);
        Assert.False(vm.Editing);
        AppKernel.MapDatabase.NewMap();
        AppKernel.MapDatabase.APIInsertSnapshots([model, model2]);
        Assert.Equal("", vm.Item.Validate());
        vm.Item.Key = "";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "a\nb";
        Assert.Equal("主键无效", vm.Item.Validate());
        vm.Item.Key = "key3";
        vm.Item.Value = "";
        Assert.Equal("值不能为空", vm.Item.Validate());
        vm.Item.Key = "key3";
        vm.Item.Value = "value3";
        Assert.Equal("", vm.Item.Validate());
        var form = new SnapshotForm(model, (e) => e.Key == "error" ? "error key" : "");
        form.Key = "error";
        Assert.Equal("error key", form.Validate());
        form.Key = "key3";
        Assert.Equal("", form.Validate());
    }
    [Fact]
    public void TestNewConditionWindow()
    {
        var vm = new NewConditionWindowViewModel(null, (ConditionForm form) => "");
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Key);
        Assert.False(vm.Item.Not);
        Assert.Equal("新建条件", vm.Title);
        var model = new ValueCondition("key", 0, true);
        vm = new NewConditionWindowViewModel(model, (ConditionForm form) => form.Key == "error" ? "key error" : "");
        Assert.Equal("key", vm.Item.Key);
        Assert.True(vm.Item.Not);
        Assert.True(vm.Item.ToCondition().Equal(model));
        vm.Item.Key = "";
        Assert.Equal("标签不能为空", vm.Item.Validate());
        vm.Item.Key = "error";
        Assert.Equal("key error", vm.Item.Validate());
        vm.Item.Key = "key2";
        Assert.Equal("", vm.Item.Validate());
    }
    [Fact]
    public void TestNewTagWindow()
    {
        var vm = new NewTagWindowViewModel(null, (TagForm form) => "");
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Key);
        var model = new ValueTag("key", 0);
        vm = new NewTagWindowViewModel(model, (TagForm form) => form.Key == "error" ? "key error" : "");
        Assert.Equal("key", vm.Item.Key);
        Assert.Equal("key", vm.Item.ToTag().Key);
        vm.Item.Key = "";
        Assert.Equal("标签不能为空", vm.Item.Validate());
        vm.Item.Key = "error";
        Assert.Equal("key error", vm.Item.Validate());
        vm.Item.Key = "key2";
        Assert.Equal("", vm.Item.Validate());
    }
    [Fact]
    public void TestPickRoomWindowViewModel()
    {
        var vm = new PickRoomWindowViewModel();
        AppKernel.MapDatabase.CloseCurrent();
        Assert.Empty(vm.FilteredRooms);
        Assert.Equal(0, vm.GetMapRoomsCount);
        AppKernel.MapDatabase.NewMap();
        Assert.Empty(vm.FilteredRooms);
        Assert.Equal(0, vm.GetMapRoomsCount);
        AppKernel.MapDatabase.APIInsertRooms([
            new Room(){Key="room1"},
            new Room(){Key="room2"},
        ]);
        Assert.Equal(2, vm.FilteredRooms.Count);
        Assert.Equal(2, vm.GetMapRoomsCount);
        vm.RoomsFilter = "room1";
        vm.FilterRooms();
        Assert.Single(vm.FilteredRooms);
        Assert.Equal("room1", vm.FilteredRooms[0].Key);
        Assert.Equal(2, vm.GetMapRoomsCount);
    }
    [Fact]
    public void TestUpdateMapWindowViewModel()
    {
        var items = UpdateMapWindowViewModel.Items;
        Assert.Equal(2, items.Count);
        Assert.Equal(MapEncoding.Default, items[0].Key);
        Assert.Equal("UTF-8", items[0].Label);
        Assert.Equal(MapEncoding.GB18030, items[1].Key);
        Assert.Equal("GB18030", items[1].Label);
        var model = new MapSettings()
        {
            Name = "name",
            Desc = "desc",
            Encoding = MapEncoding.GB18030,
        };
        var vm = new UpdateMapWindowViewModel(model);
        Assert.Equal(model, vm.Settings);

    }
    [Fact]
    public void TestEditRegionItemWindow()
    {
        var vm = new EditRegionItemWindowViewModel(null, (RegionItemForm form) => "");
        Assert.Null(vm.Raw);
        Assert.NotNull(vm.Item);
        Assert.Equal("", vm.Item.Value);
        Assert.Equal(RegionItemType.Room, vm.Item.Type);
        Assert.False(vm.Item.Not);
        Assert.Equal("新元素", vm.Title);

        var model = new RegionItem(RegionItemType.Zone, "value", true);
        vm = new EditRegionItemWindowViewModel(model, (RegionItemForm form) => form.Value == "error" ? "value error" : "");
        Assert.Equal("value", vm.Item.Value);
        Assert.Equal(RegionItemType.Zone, vm.Item.Type);
        Assert.True(vm.Item.Not);
        Assert.True(vm.Item.ToRegionItem().Equal(model));
        vm.Item.Value = "";
        Assert.Equal("值不能为空", vm.Item.Validate());
        vm.Item.Value = "error";
        Assert.Equal("value error", vm.Item.Validate());
        vm.Item.Value = "value2";
        Assert.Equal("", vm.Item.Validate());
        Assert.Equal("编辑元素", vm.Title);
        var ris = EditRegionItemWindowViewModel.Items;
        Assert.Equal(2, ris.Count);
        Assert.Equal(RegionItemType.Room, ris[0].Type);
        Assert.Equal("房间", ris[0].Value);
        Assert.Equal(RegionItemType.Zone, ris[1].Type);
        Assert.Equal("房间分组", ris[1].Value);
    }
}

