local hmmlib=require('hmm')
local hmm=hmmlib.new()
hmm.DllEncoding=0 --0 for utf-8, 1 for gbk
local json=require('json')
print(hmm:call("version"))
local file=assert(io.open("hongchen.hmm","r"))
local data=file:read("*a")
file:close();
-- 加载地图文件，注意如果已经加载过(用同一个dll的实例共享)，会无效并返回false
print(hmm:call("import",data))
-- 打印地图信息
print(hmm:call("info"))
--创建点对点查询
local query=hmmlib.QueryPathAny.new()
query.From={"2522"}
query.Target={"3431"}

local start=os.time()
local result
for i=1,1000 do
    local querydata=json.encode(query)
    --查询
    result=json.decode(hmm:call("querypathany",querydata))
end
local finish=os.time()
print("Time taken for all query: ", (finish - start))
print("Time per query: ", (finish - start)/1000)
print(json.encode(result))