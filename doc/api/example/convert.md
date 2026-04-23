# 数据转转

数据转换指将其他地图格式的文件转换为hmm格式的地图信息。

具体来说，分为几步

* 使用create 接口 新建地图库
* 建立所有的房间Room，至少保证有 Key(主键) Name(房间名),如果有区域的话设置为Group
* 其他数据，比如小地图，特殊描述等,作为房间的Data，添加进房间的对应信息
* 将房间以Key为主键放在一个字典/对象/表内
* 将房间关系(出口)建立Exit，以 出发点，指令，目标 的形式设置为为 Exit的From,Command,To,如果可能的话，根据对Command进行判断，确定下具体的Exit Cost
* 将Exit添加到对应的Room内
* 将Room table转换为Room list
* 使用 insertrooms接口将所有room添加到 地图库
* 将所有房间的别名，创建Marker,Key为别名名称，Value为房间号。如果有附加信息，放在Marker的Message里
* 使用insertmarkers接口将所有marked添加到 地图库
* 如果一个房间的属性可能会变化，比如描述，则创建一个TakeSnapshot,Key为房间ID,Type为同一的类型(比如Desc),Value为具体描述，通过takesnapshot接口添加到地图库内
* 通过export导出数据库文件，保存到本地，使用HMM程序打开观察细调

## lua 代码
```lua
local hmmlib=require('hmm')
local hmm=hmmlib.new()
hmm.DllEncoding=0 --0 for utf-8, 1 for gbk
local json=require('json')
local roomsdata=readroomsdata()
local roomlinks=readroomlinks()
rooms={}
for key,value in ipairs(roomsdata)
    local key=value[1]
    local takesnapshot=hmmlib.TakeSnapshot.new()
    takesnapshot.Key=key
    takesnapshot.Type="desc"
    takesnapshot.Value=value[5]
    hmm.call("takesnapshot",json.encode(takesnapshot))
    if rooms[key]==nil then
        room=hmmlib.Room.new()
        room.Key=row[1]
        room.Group=row[2]
        room.Name=row[3]
        relation=hmmlib.Data.new()
        relation.Key="relation"
        relation.Value=row[4]
        table.insert(room.Data,relation)
        chukou=hmmlib.Data()
        chukou.Key="chukou"
        chukou.Value=row[6]
        table.insert(room.Data,chukou)
        rooms[key]=room
    end
end
for key,value in ipairs(roomlinks)
    from_room=value[1]
    command=value[2]
    to_room=value[3]
    exit=hmmlib.Exit.new()
    exit.Command=command
    exit.To=to_room
    exit.From=from_room
    exit.Cost=1
    table.insert(rooms[from_room].Exits,exit)
end
inputrooms=hmmpy.Rooms.new()
for key,value in rooms()
    table.insert(inputrooms.Rooms,room)
end
hmm.call("insertrooms", json.encode(inputrooms))
output=hmm.call("export")
print(output)
local file = assert(io.open("my.hmm", "w"))
file:write(output)
file:close()
```
## python 代码

```python
import hmmpy
hmm=hmmpy.HMMDll("./HellMapManager.so")
hmm.call("create")
roomsdata=readroomsdata()
roomlinks=readroomlinks()
rooms={}
for row in roomsdata:
    key=row[0]
    takesnapshot=hmmpy.TakeSnapshot()
    takesnapshot.Key=key
    takesnapshot.Type="desc"
    takesnapshot.Value=row[4]
    hmm.call("takesnapshot",hmm.encode(takesnapshot))
    if rooms.get(key) is not None:
        continue
    room=hmmpy.Room()
    room.Key=row[0]
    room.Group=row[1]
    room.Name=row[2]
    relation=hmmpy.Data()
    relation.Key="relation"
    relation.Value=row[3]
    room.Data.append(relation)
    chukou=hmmpy.Data()
    chukou.Key="chukou"
    chukou.Value=row[5]
    room.Data.append(chukou)
    rooms[key]=room
for row in roomlinks:
    from_room=row[0]
    command=row[1]
    to_room=row[2]
    exit=hmmpy.Exit()
    exit.Command=command
    exit.To=to_room
    exit.From=from_room
    exit.Cost=1
    rooms[from_room].Exits.append(exit)
inputrooms=hmmpy.Rooms()
for room in rooms():
    inputrooms.Rooms.append(room)
hmm.call("insertrooms", hmm.encode(inputrooms))
output=hmm.call("export")
print(output)
with open("my.hmm","w") as f:
    f.write(output)
```