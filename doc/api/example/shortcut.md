# 直达捷径

很多Mud里都有一种高级移动方式，模拟wiz的fly指令，可以从任何房间或者符合条件的房间，直达几个固定的房间(机场)

体现的形式可以是fly,可以是宠物的rideto,或者武器的miss等。

在HMM的路径计算里这种路径称为Shortcut,可以在地图的Shortcut表中维护，或者在环境变量的shortcuts中加入临时的直达捷径。

当然，实际使用时往往更复杂，可能需要 Tag来限制使用的条件，RoomTag来确定安全/室外房间才能使用等

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

local shorcut=hmmlib.Shortcut.new()
shorcut.From="2522"
shorcut.To="3431"
shorcut.Command="miss myweapon"


local query=hmmlib.QueryPathAny.new()
query.From={"2522"}
query.Target={"3431"}
query.Environment=hmmlib.Environment.new()
query.Environment.Shortcuts={shorcut}
local result=json.decode(hmm:call("querypathany", json.encode(query)))
if (result==nil) then
    print("No path found")
else
    print("Path found:")
    print(json.encode(result))
end
query.Options=hmmlib.MapperOptions.new()
query.Options.DisableShortcuts=true
local result=json.decode(hmm:call("querypathany", json.encode(query)))
if (result==nil) then
    print("No non-fly path found")
else
    print("Non-fly path found:")
    print(json.encode(result))
end
```

## python代码

```python
import json
import hmmpy

hmm = hmmpy.HMMDll("./HellMapManager.so")
with open('hongchen.hmm', 'r', encoding='utf-8') as f:
    hmm.call("import",f.read())

shorcut=hmmpy.Shortcut()
shorcut.From="2522"
shorcut.To="3431"
shorcut.Command="miss myweapon"

query=hmmpy.QueryPathAny()
query.From=["2522"]
query.Target=["3431"]
query.Environment=hmmpy.Environment()
query.Environment.Shortcuts=[shorcut]
result=json.loads(hmm.call("querypathany", hmm.encode(query)))
if (result==None):
    print("No path found")
else:
    print("Path found:")
    print(result)
query.Options=hmmpy.MapperOptions()
query.Options.DisableShortcuts=True
result=json.loads(hmm.call("querypathany", hmm.encode(query)))
if (result==None):
    print("No non-fly path found")
else:    
    print("Non-fly path found:")
    print(result)
    
```

## 输出

```
Path found:
{'From': '2522', 'To': '3431', 'Cost': 1, 'Steps': [{'Command': 'miss myweapon', 'Target': '3431', 'Cost': 1}], 'Unvisited': []}
Non-fly path found:
{'From': '2522', 'To': '3431', 'Cost': 29, 'Steps': [{'Command': 'e', 'Target': '2440', 'Cost': 1}, {'Command': 'e', 'Target': '2544', 'Cost': 1}, {'Command': 'e', 'Target': '2494', 'Cost': 1}, {'Command': 'goto beijing', 'Target': '3432', 'Cost': 20}, {'Command': 'e', 'Target': '3436', 'Cost': 1}, {'Command': 'e', 'Target': '3500', 'Cost': 1}, {'Command': 'e', 'Target': '3501', 'Cost': 1}, {'Command': 'e', 'Target': '3404', 'Cost': 1}, {'Command': 'e', 'Target': '3430', 'Cost': 1}, {'Command': 'n', 'Target': '3431', 'Cost': 1}], 'Unvisited': []}
```