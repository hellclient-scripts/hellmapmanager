# 临时拦截处理

临时拦截处理是指路线有用，但是由于某些情况，比如NPC栏目，出口不可用，需要临时禁用的情况

一般来说，由于拦路都会持续一段时间，为了避免多个出口被拦路造成两头循环跑的状况，会对拦路加以记录，一段时间都禁用该出口，而非仅一次拦路生效

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

-- 模拟被拦截的房间
local blockedroom="2440"
-- 模拟被拦截的移动
local blockedStep={["Command"]="e",["Target"]="2544",["Cost"]=1}

-- 拦截管理器
Blocker={}
Blocker.__index = Blocker
function Blocker:new()
        local self = {}
        self.Blocked={}
        self.blockDuration=5*60 --默认封锁5分钟
        return setmetatable(self, {__index = Blocker})
end
Blocker.block=function (self, room, step)
    local data={}
    data["from"]=room
    data["to"]=step["Target"]
    data["time"]=os.time()
    self.Blocked[room.."\n"..step["Target"]]=data
end
Blocker.list=function (self)
    local now=os.time()
    local result={}
    for key, value in pairs(self.Blocked) do
        if now-value["time"]>self.blockDuration then
            self.Blocked[key]=nil
        else
            local link=hmmlib.Link.new()
            link.From=value["from"]
            link.To=value["to"]
            table.insert(result, link)
        end
    end
    return result
end
local blocker=Blocker:new()

local query=hmmlib.QueryPathAny.new()
query.From={"2522"}
query.Target={"3431"}

local result=json.decode(hmm:call("querypathany", json.encode(query)))
if (result==nil) then
    print("No path found")
else
    print("Path found:")
    print(json.encode(result))
end

blocker:block(blockedroom, blockedStep)
query=hmmlib.QueryPathAny.new()
query.From={"2522"}
query.Target={"3431"}
query.Environment=hmmlib.Environment.new()
query.Environment.BlockedLinks=blocker:list()
result=json.decode(hmm:call("querypathany", json.encode(query)))
if (result==nil) then
    print("No blocked path found")
else
    print("Blocked path found:")
    print(json.encode(result))
end
```

## python代码

```python
import json
import hmmpy
import datetime
# 模拟被拦截的房间
blockedroom="2440"
# 模拟被拦截的移动
blockedStep={"Command":"e","Target":"2544","Cost":1}
# 拦截管理器
class Blocker:
    def __init__(self):
        self.Blocked={}
        self.blockDuration=datetime.timedelta(minutes=5)
    def block(self,room,step):
        self.Blocked[room+"\n"+step["Target"]]={"time":datetime.datetime.now(),"from":room,"to":step["Target"]}
    def list(self):
        now=datetime.datetime.now()
        result=[]
        for key in list(self.Blocked.keys()):
            if now-self.Blocked[key]["time"]>self.blockDuration:
                del self.Blocked[key]
            else:
                link=hmmpy.Link()
                link.From=self.Blocked[key]["from"]
                link.To=self.Blocked[key]["to"]
                result.append(link)
        return result
blocker=Blocker()

hmm = hmmpy.HMMDll("./HellMapManager.so")
with open('hongchen.hmm', 'r', encoding='utf-8') as f:
    hmm.call("import",f.read())

query=hmmpy.QueryPathAny()
query.From=["2522"]
query.Target=["3431"]

result=json.loads(hmm.call("querypathany", hmm.encode(query)))
if (result==None):
    print("No path found")
else:
    print("Path found:")
    print(result)

blocker.block(blockedroom, blockedStep)
query=hmmpy.QueryPathAny()
query.From=["2522"]
query.Target=["3431"]
query.Environment=hmmpy.Environment()
query.Environment.BlockedLinks=blocker.list()
result=json.loads(hmm.call("querypathany", hmm.encode(query)))
if (result==None):
    print("No blocked path found")
else:
    print("Blocked path found:")
    print(result)
```

## 输出
```
Path found:
{'From': '2522', 'To': '3431', 'Cost': 29, 'Steps': [{'Command': 'e', 'Target': '2440', 'Cost': 1}, {'Command': 'e', 'Target': '2544', 'Cost': 1}, {'Command': 'e', 'Target': '2494', 'Cost': 1}, {'Command': 'goto beijing', 'Target': '3432', 'Cost': 20}, {'Command': 'e', 'Target': '3436', 'Cost': 1}, {'Command': 'e', 'Target': '3500', 'Cost': 1}, {'Command': 'e', 'Target': '3501', 'Cost': 1}, {'Command': 'e', 'Target': '3404', 'Cost': 1}, {'Command': 'e', 'Target': '3430', 'Cost': 1}, {'Command': 'n', 'Target': '3431', 'Cost': 1}], 'Unvisited': []}
Blocked path found:
{'From': '2522', 'To': '3431', 'Cost': 51, 'Steps': [{'Command': 'e', 'Target': '2440', 'Cost': 1}, {'Command': 's', 'Target': '2439', 'Cost': 1}, {'Command': 's', 'Target': '2460', 'Cost': 1}, {'Command': 's', 'Target': '2504', 'Cost': 1}, {'Command': 's', 'Target': '2505', 'Cost': 1}, {'Command': 's', 'Target': '2506', 'Cost': 1}, {'Command': 's', 'Target': '1600', 'Cost': 1}, {'Command': 's', 'Target': '1602', 'Cost': 1}, {'Command': 's', 'Target': '1603', 'Cost': 1}, {'Command': 's', 'Target': '1604', 'Cost': 1}, {'Command': 'w', 'Target': '1605', 'Cost': 1}, {'Command': 'n', 'Target': '2560', 'Cost': 1}, {'Command': 'n', 'Target': '2616', 'Cost': 1}, {'Command': 'n', 'Target': '2615', 'Cost': 1}, {'Command': 'n', 'Target': '2619', 'Cost': 1}, {'Command': 'n', 'Target': '2618', 'Cost': 1}, {'Command': 'n', 'Target': '2617', 'Cost': 1}, {'Command': 'n', 'Target': '2575', 'Cost': 1}, {'Command': 'e', 'Target': '2568', 'Cost': 1}, {'Command': 'e', 'Target': '2569', 'Cost': 1}, {'Command': 'n', 'Target': '2571', 'Cost': 1}, {'Command': 'n', 'Target': '2572', 'Cost': 1}, {'Command': 'n', 'Target': '2590', 'Cost': 1}, {'Command': 'w', 'Target': '2610', 'Cost': 1}, {'Command': 'n', 'Target': '2601', 'Cost': 1}, {'Command': 'goto beijing', 'Target': '3432', 'Cost': 20}, {'Command': 'e', 'Target': '3436', 'Cost': 1}, {'Command': 'e', 'Target': '3500', 'Cost': 1}, {'Command': 'e', 'Target': '3501', 'Cost': 1}, {'Command': 'e', 'Target': '3404', 'Cost': 1}, {'Command': 'e', 'Target': '3430', 'Cost': 1}, {'Command': 'n', 'Target': '3431', 'Cost': 1}], 'Unvisited': []}
```