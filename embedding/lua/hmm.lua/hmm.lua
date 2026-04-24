local hmm = require('hmmlua')
local m={}

HMMDll={}
HMMDll.__index = HMMDll

function m.new()
    local self = setmetatable({}, HMMDll)
    self.dll = hmm
    self.DllEncoding = 0 --0 for utf-8, 1 for gbk
    return self
end

function HMMDll:call(funcname, arg)
    return self.dll[funcname](arg or "",self.DllEncoding)
end
m.HMMDll=HMMDll

Environment={}
Environment.__index = Environment
function Environment.new()
    local self = setmetatable({}, Environment)
        self.Tags={} --ValueTag
        self.RoomConditions={} --ValueCondition
        self.Rooms ={} --Room
        self.Paths ={} --path
        self.Shortcuts ={} --RoomConditionExitModel
        self.Whitelist = {} --string
        self.Blacklist = {} --string
        self.BlockedLinks ={} --Link
        self.CommandCosts ={} --CommandCost
        self.RoomTags ={} --RoomTag
    return self
end
m.Environment=Environment

MapperOptions={}
MapperOptions.__index = MapperOptions
function MapperOptions.new()
    local self = setmetatable({}, MapperOptions)
        self.MaxExitCost = 0
        self.MaxTotalCost = 0
        self.DisableShortcuts = False
        self.CommandWhitelist = {} --string
        self.CommandNotContains = {} --string
    return self
end
m.MapperOptions=MapperOptions

QueryPathAny={}
QueryPathAny.__index = QueryPathAny
function QueryPathAny.new()
    local self = setmetatable({}, QueryPathAny)
        self.From={} --string
        self.Target={} --string
        self.Environment=nil
        self.Options=nil
    return self
end
m.QueryPathAny=QueryPathAny

QueryPath={}
QueryPath.__index = QueryPath
function QueryPath.new()
    local self = setmetatable({}, QueryPath)
        self.Start=""
        self.Target={} --string
        self.Environment=nil
        self.Options=nil
    return self
end
m.QueryPath=QueryPath



KeyTypeValue = {}
KeyTypeValue.__index = KeyTypeValue
function KeyTypeValue.new()
    local self = setmetatable({}, KeyTypeValue)
    self.Key = ""
    self.Type = ""
    self.Value = ""
    return self
end
m.KeyTypeValue = KeyTypeValue

SnapshotKeyList = {}
SnapshotKeyList.__index = SnapshotKeyList
function SnapshotKeyList.new()
    local self = setmetatable({}, SnapshotKeyList)
    self.Keys = {} -- KeyTypeValue
    return self
end
m.SnapshotKeyList = SnapshotKeyList

KeyType = {}
KeyType.__index = KeyType
function KeyType.new()
    local self = setmetatable({}, KeyType)
    self.Key = ""
    self.Type = ""
    return self
end
m.KeyType = KeyType

LandmarkKeyList = {}
LandmarkKeyList.__index = LandmarkKeyList
function LandmarkKeyList.new()
    local self = setmetatable({}, LandmarkKeyList)
    self.LandmarkKeys = {} -- KeyType
    return self
end
m.LandmarkKeyList = LandmarkKeyList

KeyList = {}
KeyList.__index = KeyList
function KeyList.new()
    local self = setmetatable({}, KeyList)
    self.Keys = {} -- string
    return self
end
m.KeyList = KeyList

ListOption = {}
ListOption.__index = ListOption
function ListOption.new()
    local self = setmetatable({}, ListOption)
    self.Keys = {} -- string
    self.Groups = {} -- string
    return self
end
m.ListOption = ListOption

ValueTag = {}
ValueTag.__index = ValueTag
function ValueTag.new()
    local self = setmetatable({}, ValueTag)
    self.Key = ""
    self.Value = 0
    return self
end
m.ValueTag = ValueTag

Data = {}
Data.__index = Data
function Data.new()
    local self = setmetatable({}, Data)
    self.Key = ""
    self.Value = ""
    return self
end
m.Data = Data

ValueCondition = {}
ValueCondition.__index = ValueCondition
function ValueCondition.new()
    local self = setmetatable({}, ValueCondition)
    self.Key = ""
    self.Not = false
    self.Value = 0
    return self
end
m.ValueCondition = ValueCondition

Exit = {}
Exit.__index = Exit
function Exit.new()
    local self = setmetatable({}, Exit)
    self.Command = ""
    self.To = ""
    self.Cost = 1
    self.Conditions = {} -- ValueCondition
    return self
end
m.Exit = Exit

Room = {}
Room.__index = Room
function Room.new()
    local self = setmetatable({}, Room)
    self.Key = ""
    self.Name = ""
    self.Desc = ""
    self.Group = ""
    self.Tags = {} -- ValueTag
    self.Exits = {} -- Exit
    self.Data = {} -- Data
    return self
end
m.Room = Room

Rooms = {}
Rooms.__index = Rooms
function Rooms.new()
    local self = setmetatable({}, Rooms)
    self.Rooms = {} -- Room
    return self
end
m.Rooms = Rooms

Marker = {}
Marker.__index = Marker
function Marker.new()
    local self = setmetatable({}, Marker)
    self.Key = ""
    self.Value = ""
    self.Group = ""
    self.Desc = ""
    self.Message = ""
    return self
end
m.Marker = Marker

Markers = {}
Markers.__index = Markers
function Markers.new()
    local self = setmetatable({}, Markers)
    self.Markers = {} -- Marker
    return self
end
m.Markers = Markers

Route = {}
Route.__index = Route
function Route.new()
    local self = setmetatable({}, Route)
    self.Key = ""
    self.Desc = ""
    self.Group = ""
    self.Message = ""
    self.Rooms = {} -- string
    return self
end
m.Route = Route

Routes = {}
Routes.__index = Routes
function Routes.new()
    local self = setmetatable({}, Routes)
    self.Routes = {} -- Route
    return self
end
m.Routes = Routes

Trace = {}
Trace.__index = Trace
function Trace.new()
    local self = setmetatable({}, Trace)
    self.Key = ""
    self.Group = ""
    self.Desc = ""
    self.Message = ""
    self.Locations = {} -- string
    return self
end
m.Trace = Trace

Traces = {}
Traces.__index = Traces
function Traces.new()
    local self = setmetatable({}, Traces)
    self.Traces = {} -- Trace
    return self
end
m.Traces = Traces

RegionItem = {}
RegionItem.__index = RegionItem
function RegionItem.new()
    local self = setmetatable({}, RegionItem)
    self.Not = false
    self.Type = ""
    self.Value = ""
    return self
end
m.RegionItem = RegionItem

Region = {}
Region.__index = Region
function Region.new()
    local self = setmetatable({}, Region)
    self.Key = ""
    self.Group = ""
    self.Desc = ""
    self.Message = ""
    self.Items = {} -- RegionItem
    return self
end
m.Region = Region

Regions = {}
Regions.__index = Regions
function Regions.new()
    local self = setmetatable({}, Regions)
    self.Regions = {} -- Region
    return self
end
m.Regions = Regions

Shortcut = {}
Shortcut.__index = Shortcut
function Shortcut.new()
    local self = setmetatable({}, Shortcut)
    self.Key = ""
    self.Command = ""
    self.To = ""
    self.RoomConditions = {} -- ValueCondition
    self.Conditions = {} -- ValueCondition
    self.Cost = 1
    self.Group = ""
    self.Desc=""
    return self
end
m.Shortcut = Shortcut

Shortcuts = {}
Shortcuts.__index = Shortcuts
function Shortcuts.new()
    local self = setmetatable({}, Shortcuts)
    self.Shortcuts = {} -- Shortcut
    return self
end
m.Shortcuts = Shortcuts

Variable = {}
Variable.__index = Variable
function Variable.new()
    local self = setmetatable({}, Variable)
    self.Key = ""
    self.Value = ""
    self.Group = ""
    self.Desc = ""
    return self
end
m.Variable = Variable

Variables = {}
Variables.__index = Variables
function Variables.new()
    local self = setmetatable({}, Variables)
    self.Variables = {} -- Variable
    return self
end
m.Variables = Variables

Landmark = {}
Landmark.__index = Landmark
function Landmark.new()
    local self = setmetatable({}, Landmark)
    self.Key = ""
    self.Type = ""
    self.Value = ""
    self.Group = ""
    self.Desc = ""
    return self
end
m.Landmark = Landmark

Landmarks = {}
Landmarks.__index = Landmarks
function Landmarks.new()
    local self = setmetatable({}, Landmarks)
    self.Landmarks = {} -- Landmark
    return self
end
m.Landmarks = Landmarks

Snapshot = {}
Snapshot.__index = Snapshot
function Snapshot.new()
    local self = setmetatable({}, Snapshot)
    self.Key = ""
    self.Timestamp = 0
    self.Group = ""
    self.Type = ""
    self.Count = 1
    self.Value = ""
    return self
end
m.Snapshot = Snapshot

Snapshots = {}
Snapshots.__index = Snapshots
function Snapshots.new()
    local self = setmetatable({}, Snapshots)
    self.Snapshots = {} -- Snapshot
    return self
end
m.Snapshots = Snapshots

Path = {}
Path.__index = Path
function Path.new()
    local self = setmetatable({}, Path)
    self.From = ""
    self.Command = ""
    self.To = ""
    self.Conditions = {} -- ValueCondition
    self.Cost = 1
    return self
end
m.Path = Path

RoomConditionExit = {}
RoomConditionExit.__index = RoomConditionExit
function RoomConditionExit.new()
    local self = setmetatable({}, RoomConditionExit)
    self.RoomConditions = {} -- ValueCondition
    self.Command = ""
    self.To = ""
    self.Conditions = {} -- ValueCondition
    self.Cost = 1
    return self
end
m.RoomConditionExit = RoomConditionExit

Link = {}
Link.__index = Link
function Link.new()
    local self = setmetatable({}, Link)
    self.From = ""
    self.To = ""
    return self
end
m.Link = Link


CommandCost = {}
CommandCost.__index = CommandCost
function CommandCost.new()
    local self = setmetatable({}, CommandCost)
    self.Command = ""
    self.To = ""
    self.Cost = 1
    return self
end
m.CommandCost = CommandCost

RoomTag = {}
RoomTag.__index = RoomTag
function RoomTag.new()
    local self = setmetatable({}, RoomTag)
    self.Room = ""
    self.Key = ""
    self.Value = 1
    return self
end
m.RoomTag = RoomTag

Dilate = {}
Dilate.__index = Dilate
function Dilate.new()
    local self = setmetatable({}, Dilate)
    self.Source = {} -- string
    self.Iterations = 1
    self.Environment = nil
    self.Options = nil
    return self
end
m.Dilate = Dilate

TrackExit = {}
TrackExit.__index = TrackExit
function TrackExit.new()
    local self = setmetatable({}, TrackExit)
    self.Start = ""
    self.Command = ""
    self.Environment = nil
    self.Options = nil
    return self
end
m.TrackExit = TrackExit

Key = {}
Key.__index = Key
function Key.new()
    local self = setmetatable({}, Key)
    self.Key = ""
    return self
end
m.Key = Key

GetRoom = {}
GetRoom.__index = GetRoom
function GetRoom.new()
    local self = setmetatable({}, GetRoom)
    self.Key = ""
    self.Environment = nil
    self.Options = nil
    return self
end
m.GetRoom = GetRoom

SnapshotFilter = {}
SnapshotFilter.__index = SnapshotFilter
function SnapshotFilter.new()
    local self = setmetatable({}, SnapshotFilter)
    self.Key = nil
    self.Type = nil
    self.Group = nil
    self.MaxCount = 0
    return self
end
m.SnapshotFilter = SnapshotFilter
TakeSnapshot={}
TakeSnapshot.__index = TakeSnapshot
function TakeSnapshot.new()
        local self = setmetatable({}, TakeSnapshot)
        self.Key = ""
        self.Type = ""
        self.Group = ""
        self.Value = ""
end
m.TakeSnapshot = TakeSnapshot
SnapshotSearch = {}
SnapshotSearch.__index = SnapshotSearch
function SnapshotSearch.new()
    local self = setmetatable({}, SnapshotSearch)
    self.Type = nil
    self.Group = nil
    self.Keywords = {} -- string
    self.PartialMatch = true
    self.Any = false
    self.MaxNoise = 0
    return self
end
m.SnapshotSearch = SnapshotSearch

RoomFilter = {}
RoomFilter.__index = RoomFilter
function RoomFilter.new()
    local self = setmetatable({}, RoomFilter)
    self.RoomConditions = {} -- ValueCondition
    self.HasAnyExitTo = {} -- string
    self.HasAnyData = {} -- Data
    self.HasAnyName = {} -- string
    self.HasAnyGroup = {} -- string
    self.ContainsAnyData = {} -- Data
    self.ContainsAnyName = {} -- string
    self.ContainsAnyKey = {} -- string
    return self
end
m.RoomFilter = RoomFilter

SearchRooms = {}
SearchRooms.__index = SearchRooms
function SearchRooms.new()
    local self = setmetatable({}, SearchRooms)
    self.Filter = RoomFilter.new()
    return self
end
m.SearchRooms = SearchRooms

FilterRooms = {}
FilterRooms.__index = FilterRooms
function FilterRooms.new()
    local self = setmetatable({}, FilterRooms)
    self.Filter = RoomFilter.new()
    self.Source = {} -- string
    return self
end
m.FilterRooms = FilterRooms

GroupRoom = {}
GroupRoom.__index = GroupRoom
function GroupRoom.new()
    local self = setmetatable({}, GroupRoom)
    self.Room = ""
    self.Group = ""
    return self
end
m.GroupRoom = GroupRoom

TagRoom = {}
TagRoom.__index = TagRoom
function TagRoom.new()
    local self = setmetatable({}, TagRoom)
    self.Room = ""
    self.Tag = ""
    self.Value = 0
    return self
end
m.TagRoom = TagRoom

TraceLocation = {}
TraceLocation.__index = TraceLocation
function TraceLocation.new()
    local self = setmetatable({}, TraceLocation)
        self.Key = ""
        self.Location = ""
        return self
end
m.TraceLocation = TraceLocation

return m