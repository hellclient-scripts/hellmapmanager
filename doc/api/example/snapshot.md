# 快照

快照指对房间的某个状态的一个抓取。一般用在房间会变化的属性上，比如带时间天气的描述，会移动的npc等

同房间和值的快照会堆叠。

快照的主要用途有两个，一个是作为原始数据导出分析，另一个就是对快照进行搜索，过滤符合条件的房间

## lua 代码
```lua
local hmmlib=require('hmm')
local hmm=hmmlib.new()
hmm.DllEncoding=0 --0 for utf-8, 1 for gbk
local json=require('json')
local file=assert(io.open("hongchen.hmm","r"))
local data=file:read("*a")
file:close();
hmm:call("import",data)

RoomInfos={}
table.insert(RoomInfos,{["Key"]="room1",["Desc"]="房间一，牛马们工作的地方，早晨",["objects"]={"阿牛(a niu)","阿宝(a bao)","阿三(a san)"}})
table.insert(RoomInfos,{["Key"]="room1",["Desc"]="房间一，牛马们工作的地方，晚上",["objects"]={"阿宝(a bao)","阿三(a san)"}})
table.insert(RoomInfos,{["Key"]="room2",["Desc"]="阿牛家，在工厂门口，一眼能看到工厂的大门，早晨",["objects"]={"牛嫂(niu sao)","老王(lao wang)"}})
table.insert(RoomInfos,{["Key"]="room2",["Desc"]="阿牛家，在工厂门口，一眼能看到工厂的大门，晚上",["objects"]={"牛嫂(niu sao)","阿牛(a niu)"}})
table.insert(RoomInfos,{["Key"]="room3",["Desc"]="老王家，背对工厂，在阿牛家隔壁，早晨",["objects"]={}})
table.insert(RoomInfos,{["Key"]="room3",["Desc"]="老王家，背对工厂，在阿牛家隔壁，晚上",["objects"]={"老王(lao wang)","三嫂(san sao)"}})

for _, info in pairs(RoomInfos) do
    local sanpdesc=hmmlib.TakeSnapshot.new()
    sanpdesc.Key=info["Key"]
    sanpdesc.Type="desc"
    sanpdesc.Value=info["Desc"]
    hmm:call("takesnapshot",json.encode(sanpdesc))
    local snapobj=hmmlib.TakeSnapshot.new()
    snapobj.Key=info["Key"]
    snapobj.Type="objects"
    if #(info["objects"])>0 then
        snapobj.Value="|"..table.concat(info["objects"],"|").."|"
    else
        snapobj.Value=""
    end
    hmm:call("takesnapshot",json.encode(snapobj))
end

local search
local result
-- 部分匹配，包含工厂，早晨
search=hmmlib.SnapshotSearch.new()
search.Type="desc"
search.Keywords={"工厂","早晨"}
search.PartialMatch=true

result=json.decode(hmm:call("searchsnapshots",json.encode(search)))
print("Search 工厂 , 早晨 result:")
print(json.encode(result))

-- 部分匹配，包含 阿三，三嫂，任意一个

search=hmmlib.SnapshotSearch.new()
search.Type="objects"
search.Keywords={"|阿三(a san)|","|三嫂(san sao)|"}
search.PartialMatch=true
search.Any=true

result=json.decode(hmm:call("searchsnapshots",json.encode(search)))
print("Search 阿三 , 三嫂 result:")
print(json.encode(result))

-- 部分匹配，包含阿牛，牛嫂，最大噪音0
search=hmmlib.SnapshotSearch.new()
search.Type="objects"
search.Keywords={"|阿牛(a niu)|","|牛嫂(niu sao)|"}
search.PartialMatch=true
search.MaxNoise=0

result=json.decode(hmm:call("searchsnapshots",json.encode(search)))
print("Search 阿牛 , 牛嫂 0 result:")
print(json.encode(result))

-- 部分匹配，包含阿牛，牛嫂，最大噪音1,允许一个关键词不匹配
search=hmmlib.SnapshotSearch.new()
search.Type="objects"
search.Keywords={"|阿牛(a niu)|","|牛嫂(niu sao)|"}
search.PartialMatch=true
search.MaxNoise=1

result=json.decode(hmm:call("searchsnapshots",json.encode(search)))
print("Search 阿牛 , 牛嫂 any 1 result:")
print(json.encode(result))

-- 完整匹配
search=hmmlib.SnapshotSearch.new()
search.Type="objects"
search.Keywords={"|牛嫂(niu sao)|老王(lao wang)|","|三嫂(san sao)|老王(lao wang)|"}
search.PartialMatch=false
search.Any=true

result=json.decode(hmm:call("searchsnapshots",json.encode(search)))
print("Search 牛嫂 , 老王 |三嫂 , 老王 result:")
print(json.encode(result))
```
## python 代码

```python
import json
import hmmpy

hmm = hmmpy.HMMDll("./HellMapManager.so")
with open('hongchen.hmm', 'r', encoding='utf-8') as f:
    hmm.call("import",f.read())
    
RoomInfos=[]
RoomInfos.append({"Key":"room1","Desc":"房间一，牛马们工作的地方，早晨","objects":["阿牛(a niu)","阿宝(a bao)","阿三(a san)"]})
RoomInfos.append({"Key":"room1","Desc":"房间一，牛马们工作的地方，晚上","objects":["阿宝(a bao)","阿三(a san)"]})
RoomInfos.append({"Key":"room2","Desc":"阿牛家，在工厂门口，一眼能看到工厂的大门，早晨","objects":["牛嫂(niu sao)","老王(lao wang)"]})
RoomInfos.append({"Key":"room2","Desc":"阿牛家，在工厂门口，一眼能看到工厂的大门，晚上","objects":["牛嫂(niu sao)","阿牛(a niu)"]})
RoomInfos.append({"Key":"room3","Desc":"老王家，背对工厂，在阿牛家隔壁，早晨","objects":[]})
RoomInfos.append({"Key":"room3","Desc":"老王家，背对工厂，在阿牛家隔壁，晚上","objects":["老王(lao wang)","三嫂(san sao)"]})

for info in RoomInfos:
    sanpdesc=hmmpy.TakeSnapshot()
    sanpdesc.Key=info["Key"]
    sanpdesc.Type="desc"
    sanpdesc.Value=info["Desc"]
    hmm.call("takesnapshot",hmm.encode(sanpdesc))
    snapobj=hmmpy.TakeSnapshot()
    snapobj.Key=info["Key"]
    snapobj.Type="objects"
    if len(info["objects"])>0:
        snapobj.Value="|"+"|".join(info["objects"])+"|"
    else:
        snapobj.Value=""
    hmm.call("takesnapshot",hmm.encode(snapobj))

# 部分匹配，包含工厂，早晨
search=hmmpy.SnapshotSearch()
search.Type="desc"
search.Keywords=["工厂","早晨"]
search.PartialMatch=True

result=json.loads(hmm.call("searchsnapshots",hmm.encode(search)))
print("Search 工厂 , 早晨 result:")
print(result)

# 部分匹配，包含 阿三，三嫂，任意一个

search=hmmpy.SnapshotSearch()
search.Type="objects"
search.Keywords=["|阿三(a san)|","|三嫂(san sao)|"]
search.PartialMatch=True
search.Any=True

result=json.loads(hmm.call("searchsnapshots",hmm.encode(search)))
print("Search 阿三 , 三嫂 result:")
print(result)

# 部分匹配，包含阿牛，牛嫂，最大噪音0
search=hmmpy.SnapshotSearch()
search.Type="objects"
search.Keywords=["|阿牛(a niu)|","|牛嫂(niu sao)|"]
search.PartialMatch=True
search.MaxNoise=0

result=json.loads(hmm.call("searchsnapshots",hmm.encode(search)))
print("Search 阿牛 , 牛嫂 0 result:")
print(result)

# 部分匹配，包含阿牛，牛嫂，最大噪音1,允许一个关键词不匹配
search=hmmpy.SnapshotSearch()
search.Type="objects"
search.Keywords=["|阿牛(a niu)|","|牛嫂(niu sao)|"]
search.PartialMatch=True
search.MaxNoise=1

result=json.loads(hmm.call("searchsnapshots",hmm.encode(search)))
print("Search 阿牛 , 牛嫂 any 1 result:")
print(result)

# 完整匹配
search=hmmpy.SnapshotSearch()
search.Type="objects"
search.Keywords=["|牛嫂(niu sao)|老王(lao wang)|","|三嫂(san sao)|老王(lao wang)|"]
search.PartialMatch=False
search.Any=True

result=json.loads(hmm.call("searchsnapshots",hmm.encode(search)))
print("Search 牛嫂 , 老王 |三嫂 , 老王 result:")
print(result)
```

## 输出
```
Search 工厂 , 早晨 result:
[{'Key': 'room2', 'Sum': 4, 'Count': 1, 'Items': [{'Key': 'room2', 'Timestamp': 1777019190, 'Group': '', 'Type': 'desc', 'Count': 1, 'Value': '阿牛家，在工厂门口，一眼能看到工厂的大门，早晨'}]}, {'Key': 'room3', 'Sum': 4, 'Count': 1, 'Items': [{'Key': 'room3', 'Timestamp': 1777019190, 'Group': '', 'Type': 'desc', 'Count': 1, 'Value': '老王家，背对工厂，在阿牛家隔壁，早晨'}]}]
Search 阿三 , 三嫂 result:
[{'Key': 'room1', 'Sum': 4, 'Count': 2, 'Items': [{'Key': 'room1', 'Timestamp': 1777019190, 'Group': '', 'Type': 'objects', 'Count': 1, 'Value': '|阿牛(a niu)|阿宝(a bao)|阿三(a san)|'}, {'Key': 'room1', 'Timestamp': 1777019190, 'Group': '', 'Type': 'objects', 'Count': 1, 'Value': '|阿宝(a bao)|阿三(a san)|'}]}, {'Key': 'room3', 'Sum': 4, 'Count': 1, 'Items': [{'Key': 'room3', 'Timestamp': 1777019190, 'Group': '', 'Type': 'objects', 'Count': 1, 'Value': '|老王(lao wang)|三嫂(san sao)|'}]}]
Search 阿牛 , 牛嫂 0 result:
[{'Key': 'room2', 'Sum': 4, 'Count': 1, 'Items': [{'Key': 'room2', 'Timestamp': 1777019190, 'Group': '', 'Type': 'objects', 'Count': 1, 'Value': '|牛嫂(niu sao)|阿牛(a niu)|'}]}]
Search 阿牛 , 牛嫂 any 1 result:
[{'Key': 'room1', 'Sum': 4, 'Count': 1, 'Items': [{'Key': 'room1', 'Timestamp': 1777019190, 'Group': '', 'Type': 'objects', 'Count': 1, 'Value': '|阿牛(a niu)|阿宝(a bao)|阿三(a san)|'}]}, {'Key': 'room2', 'Sum': 4, 'Count': 2, 'Items': [{'Key': 'room2', 'Timestamp': 1777019190, 'Group': '', 'Type': 'objects', 'Count': 1, 'Value': '|牛嫂(niu sao)|老王(lao wang)|'}, {'Key': 'room2', 'Timestamp': 1777019190, 'Group': '', 'Type': 'objects', 'Count': 1, 'Value': '|牛嫂(niu sao)|阿牛(a niu)|'}]}]
Search 牛嫂 , 老王 |三嫂 , 老王 result:
[{'Key': 'room2', 'Sum': 4, 'Count': 1, 'Items': [{'Key': 'room2', 'Timestamp': 1777019190, 'Group': '', 'Type': 'objects', 'Count': 1, 'Value': '|牛嫂(niu sao)|老王(lao wang)|'}]}]
```