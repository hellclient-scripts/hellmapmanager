# 查询点对点路线

作为mud寻路最常见的点对点路线，HMM支持通过 Environment 和MapperOption的方式进行寻路

实力代码为 设置 门派标签为丐帮，轻功标签为当前轻功等级的寻路。

这样，寻路会避开丐帮专属的出口，和要求一定dodge(一般是cross)的出口

## lua
```lua
local hmmlib=require('hmm')
local hmm=hmmlib.new()
hmm.DllEncoding=0 --0 for utf-8, 1 for gbk
local json=require('json')
local file=assert(io.open("hongchen.hmm","r"))
local data=file:read("*a")
file:close();
hmm:call("import",data)
local myskills={}
myskills["dodge"]=99

local query=hmmlib.QueryPathAny.new()
query.From={"2522"}
query.Target={"3431"}
-- 设置门派tag
local familytag=hmmlib.ValueTag.new()
familytag.Key="xiaoyao"
familytag.Value=1
-- 设置轻功tag
local dodgetag=hmmlib.ValueTag.new()
dodgetag.Key="skill-dodge"
dodgetag.Value=myskills["dodge"]
query.Environment=hmmlib.Environment.new()
query.Environment.Tags={familytag, dodgetag}
local result=json.decode(hmm:call("querypathany", json.encode(query)))
if (result==nil) then
    print("No path found")
else
    print("Path found:")
    print(json.encode(result))
end
```

## python

```python
import json
import hmmpy

myskills={}
myskills["dodge"]=99

hmm = hmmpy.HMMDll("./HellMapManager.so")
with open('hongchen.hmm', 'r', encoding='utf-8') as f:
    hmm.call("import",f.read())

query=hmmpy.QueryPathAny()
query.From=["2522"]
query.Target=["3431"]
# 设置门派tag
familytag=hmmpy.ValueTag()
familytag.Key="xiaoyao"
familytag.Value=1
# 设置轻功tag
dodgetag=hmmpy.ValueTag()
dodgetag.Key="skill-dodge"
dodgetag.Value=myskills["dodge"]
query.Environment=hmmpy.Environment()
query.Environment.Tags=[familytag, dodgetag]
result=json.loads(hmm.call("querypathany", hmm.encode(query)))
if (result==None):
    print("No path found")
else:
    print("Path found:")
    print(result)
```

## 输出

```
Path found:
{'From': '2522', 'To': '3431', 'Cost': 29, 'Steps': [{'Command': 'e', 'Target': '2440', 'Cost': 1}, {'Command': 'e', 'Target': '2544', 'Cost': 1}, {'Command': 'e', 'Target': '2494', 'Cost': 1}, {'Command': 'goto beijing', 'Target': '3432', 'Cost': 20}, {'Command': 'e', 'Target': '3436', 'Cost': 1}, {'Command': 'e', 'Target': '3500', 'Cost': 1}, {'Command': 'e', 'Target': '3501', 'Cost': 1}, {'Command': 'e', 'Target': '3404', 'Cost': 1}, {'Command': 'e', 'Target': '3430', 'Cost': 1}, {'Command': 'n', 'Target': '3431', 'Cost': 1}], 'Unvisited': []}
```