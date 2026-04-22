import json
import hmmpy
from datetime import datetime
#加载dll库
hmm = hmmpy.HMMDll("./HellMapManager.so")
print(hmm.call("version"))
# 加载地图文件，注意如果已经加载过(用同一个dll的实例共享)，会无效并返回false
with open('hongchen.hmm', 'r', encoding='utf-8') as f:
    hmm.call("import",f.read())
# 打印地图信息
print(json.loads(hmm.call("info")))

#创建点对点查询
query=hmmpy.QueryPathAny()
query.From=["2522"]
query.Target=["3431"]
start = datetime.now()
result=None
for i in range (1000):
    querydata=json.dumps(query.__dict__)
    #查询
    result=json.loads(hmm.call("querypathany", querydata))
end=datetime.now()
print("Time taken for all query: ", (end - start))
print("Time per query: ", (end - start)/1000)
print(result)