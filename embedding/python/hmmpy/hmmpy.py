from ctypes import *
import platform
import json
    
class HMMDll():
    Encoding = 'utf-8'
    DllEncoding = 0
    def __init__(self, dllpath):
        if platform.system()=="Windows":
            self.dll=WinDLL(dllpath)
        else:
            self.dll=CDLL(dllpath)
    def call(self, funcname, arg=""):
        func = getattr(self.dll, funcname)
        func.restype = c_char_p 
        return func(arg.encode(self.Encoding), self.DllEncoding).decode(self.Encoding)
    def encode(self, obj):
        return json.dumps(obj,default = lambda x: x.__dict__)
class Environment:
    def __init__(self):
        self.Tags=[] #ValueTag
        self.RoomConditions=[] #ValueCondition
        self.Rooms =[] #Room
        self.Paths =[] #path
        self.Shortcuts =[] #RoomConditionExitModel
        self.Whitelist = [] #string
        self.Blacklist = [] #string
        self.BlockedLinks =[] #Link
        self.CommandCosts =[] #CommandCost
        self.RoomTags =[] #RoomTag

class MapperOptions:
    def __init__(self):
        self.MaxExitCost = 0
        self.MaxTotalCost = 0
        self.DisableShortcuts = False
        self.CommandWhitelist = [] #string
        self.CommandNotContains = [] #string
class QueryPathAny:
    def __init__(self):
        self.From=[] #string
        self.Target=[] #string
        self.Environment=None
        self.MapperOptions=None

class QueryPath:

    def __init__(self):
        self.Start=""
        self.Target=[] #string
        self.Environment=None
        self.MapperOptions=None

class KeyTypeValue:
    def __init__(self):
        self.Key=""
        self.Type=""
        self.Value=""
class SnapshotKeyList:
    def __init__(self):
        self.Keys=[] #KeyTypeValue
class KeyType:
    def __init__(self):
        self.Key=""
        self.Type=""
class LandmarkKeyList:
    def __init__(self):
        self.LandmarkKeys=[] #KeyType

class KeyList:
    def __init__(self):
        self.Keys=[] #string

class ListOption:
    def __init__(self):
        self.Keys=[] #string
        self.Groups=[] #string

class ValueTag:
    def __init__(self):
        self.Key=""
        self.Value=0
class Data:
    def __init__(self):
        self.Key=""
        self.Value=""
class ValueCondition:
    def __init__(self):
        self.Key=""
        self.Not=False
        self.Value=0

class Exit:
    def __init__(self):
        self.Command=""
        self.To=""
        self.Cost=0
        self.Conditions=[] #ValueCondition

class Room:
    def __init__(self):
        self.Key=""
        self.Name=""
        self.Desc=""
        self.Group=""
        self.Tags=[] #ValueTag
        self.Exits=[] #Exit
        self.Data=[] #Data
        
class Rooms:
    def __init__(self):
        self.Rooms=[] #Room

class Marker:
    def __init__(self):
        self.Key=""
        self.Value=""
        self.Group=""
        self.Desc=""
        self.Message=""
class Markers:
    def __init__(self):
        self.Markers=[] #Marker
class Route:
    def __init__(self):
        self.Key=""
        self.Desc=""
        self.Group=""
        self.Message=""
        self.Rooms=[] #string

class Routes:
    def __init__(self):
        self.Routes=[] #Route

class Trace:
    def __init__(self):
        self.Key=""
        self.Group=""
        self.Desc=""
        self.Message=""
        self.Locations=[] #string

class Traces:
    def __init__(self):
        self.Traces=[] #Trace

class RegionItem:
    def __init__(self):
        self.Not = False
        self.Type = ""
        self.Value = ""

class Region:
    def __init__(self):
        self.Key=""
        self.Group=""
        self.Desc=""
        self.Message=""
        self.Items=[] #RegionItem

class Regions:
    def __init__(self):
        self.Regions=[] #Region
class Shortcut:
    def __init__(self):
        self.Key=""
        self.Command=""
        self.To=""
        self.RoomConditions=[] #ValueCondition
        self.Conditions=[] #ValueCondition
        self.Cost=0
        self.Group=""
        self.Desc=""

class Shortcuts:
    def __init__(self):
        self.Shortcuts=[] #Shortcut
        
class Variable:
    def __init__(self):
        self.Key=""
        self.Value=""
        self.Group=""
        self.Desc=""

class Variables:
    def __init__(self):
        self.Variables=[] #Variable

class Landmark:
    def __init__(self):
        self.Key=""
        self.Type=""
        self.Value=""
        self.Group=""
        self.Desc=""

class Landmarks:
    def __init__(self):
        self.Landmarks=[] #Landmark

class Snapshot:
    def __init__(self):
        self.Key=""
        self.Timestamp=0
        self.Group=""
        self.Type=""
        self.Count=1
        self.Value=""

class Snapshots:
    def __init__(self):
        self.Snapshots=[] #Snapshot

class Path:
    def __init__(self):
        self.From=""
        self.Command=""
        self.To=""
        self.Conditions=[] #ValueCondition
        self.Cost=1
class RoomConditionExit:
    def __init__(self):
        self.RoomConditions=[] #ValueCondition
        self.Command=""
        self.To=""
        self.Conditions=[] #ValueCondition
        self.Cost=1
class Link:
    def __init__(self):
        self.From=""
        self.To=""

class CommandCost:
    def __init__(self):
        self.Command=""
        self.To=""
        self.Cost=0
class RoomTag:
    def __init__(self):
        self.Room=""
        self.Key=""
        self.Value=1

class Dilate:
    def __init__(self):
        self.Source=[] #string
        self.Iterations=1
        self.Environment=None
        self.Options=None
class TrackExit:
    def __init__(self):
        self.Start=""
        self.Command=""
        self.Environment=None
        self.Options=None
class Key:
    def __init__(self):
        self.Key=""
class GetRoom:
    def __init__(self):
        self.Key=""
        self.Environment=None
        self.Options=None
class SnapshotFilter:
    def __init__(self):
        self.Key = None
        self.Type = None
        self.Group = None
        self.MaxCount = 0
class TakeSnapshot:
    def __init__(self):
        self.Key = ""
        self.Type = ""
        self.Group = ""
        self.Value = ""
class SnapshotSearch:
    def __init__(self):
        self.Type = None
        self.Group = None
        self.Keywords = [] #string
        self.PartialMatch = True
        self.Any = False
        self.MaxNoise = 0
class RoomFilter:
    def __init__(self):
        self.RoomConditions = [] #ValueCondition
        self.HasAnyExitTo = [] #string
        self.HasAnyData = [] #Data
        self.HasAnyName = [] #string
        self.HasAnyGroup = [] #string
        self.ContainsAnyData = [] #Data
        self.ContainsAnyName = [] #string
        self.ContainsAnyKey = [] #string
class SearchRooms:
    def __init__(self):
        self.Filter = RoomFilter()
class FilterRooms:
    def __init__(self):
        self.Filter = RoomFilter()
        self.Source = [] #string
class GroupRoom:
    def __init__(self):
        self.Room=""
        self.Group=""
class TagRoom:
    def __init__(self):
        self.Room=""
        self.Tag=""
        self.Value=0
class TraceLocation:
    def __init__(self):
        self.Key = ""
        self.Location = ""