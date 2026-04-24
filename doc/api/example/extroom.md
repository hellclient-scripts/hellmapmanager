# 临时房间和路径

临时房间代表动态的房间，一般是用于玩家的自建房，或者迷宫等需要寻路，但不是地图固有信息的部分

## python 代码
```python
import json
import hmmpy
hmm = hmmpy.HMMDll("./HellMapManager.so")
with open('hongchen.hmm', 'r', encoding='utf-8') as f:
    hmm.call("import",f.read())
extrooms=[]
extpaths=[]
def buildMyRoom(entry,roomid,roomname):
    myroom1=hmmpy.Room()
    myroom1.Key="myroom-entry"
    myroom1.Name=roomname+"大厅"
    myroomexit1=hmmpy.Exit()
    myroomexit1.Command="open gate;n"
    myroomexit1.To="myroom-home"
    myroomexit2=hmmpy.Exit()
    myroomexit2.Command="out"
    myroomexit2.To=entry
    myroom1.Exits=[myroomexit1,myroomexit2]
    myroom2=hmmpy.Room()
    myroom2.Key="myroom-home"
    myroom2.Name=roomname+"卧室"
    myroomexit3=hmmpy.Exit()
    myroomexit3.Command="open gate;s"
    myroomexit3.To="myroom-entry"
    myroom2.Exits=[myroomexit3]
    extrooms.append(myroom1)
    extrooms.append(myroom2)
    entrypath=hmmpy.Path()
    entrypath.From=entry
    entrypath.Command="go "+roomid
    entrypath.To="myroom-entry"
    extpaths.append(entrypath)

buildMyRoom("2440","myhouse","大别野")

mazemaptxt="""
┌─┬─┬─┬─┬─┬─┬─┬─┐
│　　　　　　　　　　　　　│　│
├─┼　┼─┼─┼─┼─┼　┼　┤
│　│　　　│　　　　　│　│　│
├　┼　┼─┼　┼　┼─┼　┼　┤
│　　♚　　│　│　│　　　│　│
├─┼　┼　┼　┼　┼─┼　┼　┤
│　　　│　　　│　│　　　　　│
├─┼　┼─┼　┼─┼─┼　┼─┤
│　│　│　　　│　　　　　│　│
├　┼　┼─┼─┼　┼　┼─┼　┤
│　│　　　│　　　│　　　│　│
├　┼　┼─┼─┼　┼　┼　┼　┤
│　│　│　　　　　│　│　　　│
├　┼　┼─┼　┼　┼　┼　┼─┤
│　　　│　　　│　│　│　　★│
└─┴─┴─┴─┴─┴─┴─┴─┘
"""
#♚为当前位置，★为出口

class Maze:
    def __init__(self):
        self.RoomPrefix="maze-"
        self.RoomKeys=[]
        self.Rooms=[]
        self.Paths=[]
        self.EntryRoom=""
        self.ExitRooms=[]
    def buildRoomKey(self,x,y):
        return self.RoomPrefix+str(x)+"-"+str(y)
    def buildExit(self,room,tokey,command):
        exit1=hmmpy.Exit()
        exit1.Command=command
        exit1.To=tokey
        room.Exits.append(exit1)
    def readmap(self,maptxt):
        maplines=maptxt.strip().split("\n")
        roomwidth=(len(maplines[0])-1)//2
        roomheight=(len(maplines)-1)//2
        currentY=0
        #按行处理
        while currentY<roomheight:
            currentX=0
            #处理每个房间
            while currentX<roomwidth:
                roomkey=self.buildRoomKey(currentX, currentY)
                self.RoomKeys.append(roomkey)
                room=hmmpy.Room()
                room.Key=roomkey
                if maplines[currentY*2+1][currentX*2+1]=="♚":
                    self.EntryRoom=roomkey
                if maplines[currentY*2+1][currentX*2+1]=="★":
                    self.ExitRooms.append(roomkey)
                if maplines[currentY*2+1][currentX*2+2]=="　":
                    self.buildExit(room,self.buildRoomKey(currentX+1,currentY),"e")
                if maplines[currentY*2+1][currentX*2]=="　":
                    self.buildExit(room,self.buildRoomKey(currentX-1,currentY),"w")                    
                if maplines[currentY*2+2][currentX*2+1]=="　":
                    self.buildExit(room,self.buildRoomKey(currentX,currentY+1),"s")
                if maplines[currentY*2][currentX*2+1]=="　":
                    self.buildExit(room,self.buildRoomKey(currentX,currentY-1),"n")                    
                self.Rooms.append(room)
                currentX+=1
            currentY+=1
    def applyTo(self,environment):
        environment.Rooms.extend(self.Rooms)
        environment.Paths.extend(self.Paths)

mymaze=Maze()
mymaze.readmap(mazemaptxt)

query=hmmpy.QueryPathAny()
query.From=["2522"]
query.Target=["myroom-home"]
query.Environment=hmmpy.Environment()
query.Environment.Rooms=extrooms
query.Environment.Paths=extpaths
result=json.loads(hmm.call("querypathany", hmm.encode(query)))
if (result==None):
    print("No home path found")
else:
    print("Home path found:")
    print(result)
## 遍历迷宫后离开
queryall=hmmpy.QueryPath()
queryall.Start=mymaze.EntryRoom
queryall.Target=mymaze.RoomKeys
queryall.Environment=hmmpy.Environment()
mymaze.applyTo(queryall.Environment)
result=json.loads(hmm.call("querypathall", hmm.encode(queryall)))
if (result==None):
    print("No full maze path found")
else:
    print("Full maze path found:")
    print(result)
##遍历完成，离开路径
query=hmmpy.QueryPathAny()
query.From=[result["To"]]
query.Target=mymaze.ExitRooms
query.Environment=hmmpy.Environment()
mymaze.applyTo(query.Environment)
result=json.loads(hmm.call("querypathany", hmm.encode(query)))
if (result==None):
    print("No maze path found")
else:
    print("Maze path found:")
    print(result)
```

## lua代码

```lua
local hmmlib=require('hmm')
local hmm=hmmlib.new()
hmm.DllEncoding=0 --0 for utf-8, 1 for gbk
local json=require('json')
local file=assert(io.open("hongchen.hmm","r"))
local data=file:read("*a")
file:close();
hmm:call("import",data)
local extrooms={}
local extpaths={}
local function buildMyRoom(entry,roomid,roomname)
    local myroom1=hmmlib.Room.new()
    myroom1.Key="myroom-entry"
    myroom1.Name=roomname.."大厅"
    local myroomexit1=hmmlib.Exit.new()
    myroomexit1.Command="open gate;n"
    myroomexit1.To="myroom-home"
    local myroomexit2=hmmlib.Exit.new()
    myroomexit2.Command="out"
    myroomexit2.To=entry
    myroom1.Exits={myroomexit1,myroomexit2}
    local myroom2=hmmlib.Room.new()
    myroom2.Key="myroom-home"
    myroom2.Name=roomname.."卧室"
    local myroomexit3=hmmlib.Exit.new()
    myroomexit3.Command="open gate;s"
    myroomexit3.To="myroom-entry"
    myroom2.Exits={myroomexit3}
    table.insert(extrooms, myroom1)
    table.insert(extrooms, myroom2)
    local entrypath=hmmlib.Path.new()
    entrypath.From=entry
    entrypath.Command="go "..roomid
    entrypath.To="myroom-entry"
    table.insert(extpaths, entrypath)
end

buildMyRoom("2440","myhouse","大别野")

local mazemaptxt=[[
┌─┬─┬─┬─┬─┬─┬─┬─┐
│　　　　　　　　　　　　　│　│
├─┼　┼─┼─┼─┼─┼　┼　┤
│　│　　　│　　　　　│　│　│
├　┼　┼─┼　┼　┼─┼　┼　┤
│　　♚　　│　│　│　　　│　│
├─┼　┼　┼　┼　┼─┼　┼　┤
│　　　│　　　│　│　　　　　│
├─┼　┼─┼　┼─┼─┼　┼─┤
│　│　│　　　│　　　　　│　│
├　┼　┼─┼─┼　┼　┼─┼　┤
│　│　　　│　　　│　　　│　│
├　┼　┼─┼─┼　┼　┼　┼　┤
│　│　│　　　　　│　│　　　│
├　┼　┼─┼　┼　┼　┼　┼─┤
│　　　│　　　│　│　│　　★│
└─┴─┴─┴─┴─┴─┴─┴─┘
]]
--♚为当前位置，★为出口

local Maze={}
Maze.__index=Maze
Maze.new=function()
    local self=setmetatable({},Maze)
        self.RoomPrefix="maze-"
        self.RoomKeys={}
        self.Rooms={}
        self.Paths={}
        self.EntryRoom=""
        self.ExitRooms={}
    return self
end
function Maze:buildRoomKey(x,y)
    return self.RoomPrefix..tostring(x).."-"..tostring(y)
end
function Maze:buildExit(room,tokey,command)
    local exit1=hmmlib.Exit.new()
    exit1.Command=command
    exit1.To=tokey
    table.insert(room.Exits, exit1)
end
function Maze:readmap(maptxt)
    local maplines={}
    for line in string.gmatch(maptxt, "[^\r\n]+") do
        if #line>0 then
        --因为utf8不定长，进行处理
        line = string.gsub(line,"　"," ")
        line = string.gsub(line,"│","+")
        line = string.gsub(line,"─","+")
        line = string.gsub(line,"├","+")
        line = string.gsub(line,"┤","+")
        line = string.gsub(line,"┴","+")
        line = string.gsub(line,"┬","+")
        line = string.gsub(line,"└","+")
        line = string.gsub(line,"┘","+")
        line = string.gsub(line,"┌","+")
        line = string.gsub(line,"┐","+")
        line = string.gsub(line,"┼","+")
        line = string.gsub(line,"♚","I")
        line = string.gsub(line,"★","O")
        table.insert(maplines, line)
        end
    end
    local roomwidth=(#maplines[1]-1)/2
    local roomheight=(#maplines-1)/2
    local currentY=1
    --按行处理
    while currentY<=roomheight do
        local currentX=1
        --处理每个房间
            while currentX<=roomwidth do
                local roomkey=self:buildRoomKey(currentX-1, currentY-1)
                table.insert(self.RoomKeys, roomkey)
                local room=hmmlib.Room.new()
                room.Key=roomkey
                if maplines[currentY*2]:sub(currentX*2, currentX*2)=="I" then
                    self.EntryRoom=roomkey
                end
                if maplines[currentY*2]:sub(currentX*2, currentX*2)=="O" then
                    table.insert(self.ExitRooms, roomkey)
                end
                if maplines[currentY*2]:sub(currentX*2+1, currentX*2+1)==" " then
                    self:buildExit(room,self:buildRoomKey(currentX,currentY-1),"e")
                end
                if maplines[currentY*2]:sub(currentX*2-1, currentX*2-1)==" " then
                    self:buildExit(room,self:buildRoomKey(currentX-2,currentY-1),"w")                    
                end
                if maplines[currentY*2+1]:sub(currentX*2, currentX*2)==" " then
                    self:buildExit(room,self:buildRoomKey(currentX-1,currentY),"s")
                end
                if maplines[currentY*2-1]:sub(currentX*2, currentX*2)==" " then
                    self:buildExit(room,self:buildRoomKey(currentX-1,currentY-2),"n")                    
                end
                table.insert(self.Rooms, room)
                currentX=currentX+1
            end
        currentY=currentY+1
    end
end
function Maze:applyTo(environment)
    for _, room in ipairs(self.Rooms) do
        table.insert(environment.Rooms, room)
    end
    for _, path in ipairs(self.Paths) do
        table.insert(environment.Paths, path)
    end
end

local mymaze=Maze.new()
mymaze:readmap(mazemaptxt)
local query=hmmlib.QueryPathAny.new()
query.From={"2522"}
query.Target={"myroom-home"}
query.Environment=hmmlib.Environment.new()
query.Environment.Rooms=extrooms
query.Environment.Paths=extpaths
local result=json.decode(hmm:call("querypathany", json.encode(query)))
if (result==nil) then
    print("No home path found")
else
    print("Home path found:")
    print(json.encode(result))
end

-- 遍历迷宫后离开
local queryall=hmmlib.QueryPath.new()
queryall.Start=mymaze.EntryRoom
queryall.Target=mymaze.RoomKeys
queryall.Environment=hmmlib.Environment.new()

mymaze:applyTo(queryall.Environment)
result=json.decode(hmm:call("querypathall", json.encode(queryall)))
if (result==nil) then
    print("No full maze path found")
else
    print("Full maze path found:")
    print(json.encode(result))
end
--遍历完成，离开路径
query=hmmlib.QueryPathAny.new()
query.From={result["To"]}
query.Target=mymaze.ExitRooms
query.Environment=hmmlib.Environment.new()
mymaze:applyTo(query.Environment)
result=json.decode(hmm:call("querypathany", json.encode(query)))
if (result==nil) then
    print("No maze path found")
else
    print("Maze path found:")
    print(json.encode(result))
end
```

## 输出

```
Home path found:
{'From': '2522', 'To': 'myroom-home', 'Cost': 3, 'Steps': [{'Command': 'e', 'Target': '2440', 'Cost': 1}, {'Command': 'go myhouse', 'Target': 'myroom-entry', 'Cost': 1}, {'Command': 'open gate;n', 'Target': 'myroom-home', 'Cost': 1}], 'Unvisited': []}
Full maze path found:
{'From': 'maze-1-2', 'To': 'maze-0-0', 'Cost': 123, 'Steps': [{'Command': 'e', 'Target': 'maze-2-2', 'Cost': 1}, {'Command': 's', 'Target': 'maze-2-3', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-3-3', 'Cost': 1}, {'Command': 's', 'Target': 'maze-3-4', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-2-4', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-3-4', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-3-3', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-3-2', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-3-1', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-4-1', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-5-1', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-4-1', 'Cost': 1}, {'Command': 's', 'Target': 'maze-4-2', 'Cost': 1}, {'Command': 's', 'Target': 'maze-4-3', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-4-2', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-4-1', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-3-1', 'Cost': 1}, {'Command': 's', 'Target': 'maze-3-2', 'Cost': 1}, {'Command': 's', 'Target': 'maze-3-3', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-2-3', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-2-2', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-1-2', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-0-2', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-0-1', 'Cost': 1}, {'Command': 's', 'Target': 'maze-0-2', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-1-2', 'Cost': 1}, {'Command': 's', 'Target': 'maze-1-3', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-0-3', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-1-3', 'Cost': 1}, {'Command': 's', 'Target': 'maze-1-4', 'Cost': 1}, {'Command': 's', 'Target': 'maze-1-5', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-2-5', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-1-5', 'Cost': 1}, {'Command': 's', 'Target': 'maze-1-6', 'Cost': 1}, {'Command': 's', 'Target': 'maze-1-7', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-0-7', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-0-6', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-0-5', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-0-4', 'Cost': 1}, {'Command': 's', 'Target': 'maze-0-5', 'Cost': 1}, {'Command': 's', 'Target': 'maze-0-6', 'Cost': 1}, {'Command': 's', 'Target': 'maze-0-7', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-1-7', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-1-6', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-1-5', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-1-4', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-1-3', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-1-2', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-1-1', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-2-1', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-1-1', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-1-0', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-2-0', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-3-0', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-4-0', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-5-0', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-6-0', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-1', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-2', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-5-2', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-6-2', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-3', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-7-3', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-7-2', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-7-1', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-7-0', 'Cost': 1}, {'Command': 's', 'Target': 'maze-7-1', 'Cost': 1}, {'Command': 's', 'Target': 'maze-7-2', 'Cost': 1}, {'Command': 's', 'Target': 'maze-7-3', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-6-3', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-5-3', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-6-3', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-4', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-5-4', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-4-4', 'Cost': 1}, {'Command': 's', 'Target': 'maze-4-5', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-3-5', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-4-5', 'Cost': 1}, {'Command': 's', 'Target': 'maze-4-6', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-3-6', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-2-6', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-3-6', 'Cost': 1}, {'Command': 's', 'Target': 'maze-3-7', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-2-7', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-3-7', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-3-6', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-4-6', 'Cost': 1}, {'Command': 's', 'Target': 'maze-4-7', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-4-6', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-4-5', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-4-4', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-5-4', 'Cost': 1}, {'Command': 's', 'Target': 'maze-5-5', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-6-5', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-6', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-7-6', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-7-5', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-7-4', 'Cost': 1}, {'Command': 's', 'Target': 'maze-7-5', 'Cost': 1}, {'Command': 's', 'Target': 'maze-7-6', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-6-6', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-7', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-7-7', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-6-7', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-6-6', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-6-5', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-5-5', 'Cost': 1}, {'Command': 's', 'Target': 'maze-5-6', 'Cost': 1}, {'Command': 's', 'Target': 'maze-5-7', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-5-6', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-5-5', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-5-4', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-6-4', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-6-3', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-6-2', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-6-1', 'Cost': 1}, {'Command': 'n', 'Target': 'maze-6-0', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-5-0', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-4-0', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-3-0', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-2-0', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-1-0', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-0-0', 'Cost': 1}], 'Unvisited': []}
Maze path found:
{'From': 'maze-0-0', 'To': 'maze-7-7', 'Cost': 16, 'Steps': [{'Command': 'e', 'Target': 'maze-1-0', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-2-0', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-3-0', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-4-0', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-5-0', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-6-0', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-1', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-2', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-3', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-4', 'Cost': 1}, {'Command': 'w', 'Target': 'maze-5-4', 'Cost': 1}, {'Command': 's', 'Target': 'maze-5-5', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-6-5', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-6', 'Cost': 1}, {'Command': 's', 'Target': 'maze-6-7', 'Cost': 1}, {'Command': 'e', 'Target': 'maze-7-7', 'Cost': 1}], 'Unvisited': []}
```