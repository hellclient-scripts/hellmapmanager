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
query.Start="2522"
query.Target={"2440","2544","2494"}

local result=json.decode(hmm:call("querypathordered", json.encode(query)))
if (result==nil) then
    print("No path found")
else
    print("Path found:")
    print(json.encode(result))
end

blocker:block(blockedroom, blockedStep)
query.Environment=hmmlib.Environment.new()
query.Environment.BlockedLinks=blocker:list()
result=json.decode(hmm:call("querypathordered", json.encode(query)))
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

query=hmmpy.QueryPath()
query.Start="2522"
query.Target=["2440","2544","2494"]

result=json.loads(hmm.call("querypathordered", hmm.encode(query)))
if (result==None):
    print("No path found")
else:
    print("Path found:")
    print(result)

blocker.block(blockedroom, blockedStep)
query.Environment=hmmpy.Environment()
query.Environment.BlockedLinks=blocker.list()
result=json.loads(hmm.call("querypathordered", hmm.encode(query)))
if (result==None):
    print("No blocked path found")
else:
    print("Blocked path found:")
    print(result)
```

## 输出
```
Path found:
{'From': '2522', 'To': '2494', 'Cost': 3, 'Steps': [{'Command': 'e', 'Target': '2440', 'Cost': 1}, {'Command': 'e', 'Target': '2544', 'Cost': 1}, {'Command': 'e', 'Target': '2494', 'Cost': 1}], 'Unvisited': []}
Blocked path found:
{'From': '2522', 'To': '2494', 'Cost': 35, 'Steps': [{'Command': 'e', 'Target': '2440', 'Cost': 1}, {'Command': 's', 'Target': '2439', 'Cost': 1}, {'Command': 's', 'Target': '2460', 'Cost': 1}, {'Command': 'e', 'Target': '2448', 'Cost': 1}, {'Command': 'e', 'Target': '2449', 'Cost': 1}, {'Command': 'e', 'Target': '2450', 'Cost': 1}, {'Command': 'e', 'Target': '4033', 'Cost': 1}, {'Command': 'e', 'Target': '4034', 'Cost': 1}, {'Command': 'se', 'Target': '4151', 'Cost': 1}, {'Command': 's', 'Target': '4152', 'Cost': 1}, {'Command': 'e', 'Target': '4137', 'Cost': 1}, {'Command': 's', 'Target': '4140', 'Cost': 1}, {'Command': 'sw', 'Target': '4142', 'Cost': 1}, {'Command': 'goto yangzhou', 'Target': '2494', 'Cost': 20}, {'Command': 'w', 'Target': '2544', 'Cost': 1}, {'Command': 'e', 'Target': '2494', 'Cost': 1}], 'Unvisited': []}
```