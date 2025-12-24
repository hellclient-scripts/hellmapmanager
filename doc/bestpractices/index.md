# 最佳实践

对于新使用HellMapManager(HMM)的用户，HMM提供以下建议，作为最佳实践，方便用户尽快的上手HMM。

## 干净的房间数据

HMM的核心功能 路径规划与搜索，最简单的场景下只需要房间(Room)信息和捷径(Shortcut)既能实现。

对于HMM来说，最理想的情况是Room中只有单纯的编号(Key)和完整的出口(Exit)信息，其他信息都储存在标记(Marker),路线(Route),足迹(Trace),区域(Reigon),定位(Landmark),快照(Snapshot)。

当然，房间对象也提供了Name,Date等数据空间可以放置一定的信息。但建有可能的情况下尽可能的使用其他数据结构。

### 合适的Key与Marker

从一般的代码经验来看，对象的主键(Key)应该不带任何业务信息。一个房间是否有任务，是否是区域中心，是否有特殊功能，不应该体现在Key里。

所以HMM提供了Marker功能。代码里的业务信息，比如特殊NPC,特殊功能，区域中心等。都建议放在Marker里。这样代码便于调整，弹性更大。

Marker一般配合QueryPathAny接口使用。

Marker还提供了Message 字段，可以配合代码实现针对不同地点的操作，比如记录NPC信息等，任务信息等。

### Route,Trace与Region

Route是一系列固定路线的路径，一般是经验总结的最优路线，或者是任务强制的路径，一般配合QueryPathOrdered接口使用。

Trace是基于通过代码收集随机/固定的NPC和对象手机数据来使用的。可以根据代码逼近完整的NPC触摸信息。一般配合QueryPathAll使用。

Region是个更复杂的概念。一般Room会有一个直接的归属，比如城市，放在Group里。但有些人物可能会跨几个城市，附带一些零星的房间。Region就是为了匹配这些数据来使用的。每次使用Region都会需要进行一系列计算，建议缓存使用。一般配合QueryPathAll使用。

Route,Trace,Region都提供Message字段。可以供代码使用。

### Landmark与Snapshot

Landmark和Snapshot其实都是为了定位房间使用的。

Landmark是需要手工收集，在代码里调用，预设一些特殊的地点标志，可以是房间名，特殊的房间对象，房间描述的正则匹配。一个地点可以有多个Type的Landmark，建议更新频繁的MUD使用时要多使用不同的机制，避免所有Landmark同时失效。

Snapshot则是以来机器编辑时，将经过的信息分门别类的储存。在需要是进行对比，找到唯一匹配的信息来定位。

## 数据的维护

数据维护分为3个部分，初期导入，定期更新，手动修正

### 数据的初期导入

数据初期导入建议先把实现整理好的数据，整理成数组形式，通过InsertRooms和InsertShortcuts导入HMM。

导入时，一些特殊指令比如ride,yell boat可以实现设置Cost。

特殊有busy地形可以手动进行维护。

再维护一定的Landmark,确保机器可以正常运行

确保正常使用后再导入Marker,Route,Trace,Region等资料。

### 数据的定期更新。

有活跃更新的MUD,建议用不同的ID, 定期遍历全地图。

遍历地图时可以利用以下接口

* TagRoom 为房间更新Tag,比如室内室外，是否能战斗
* GroupRoom 更新房间最新所属的区域
* SetRoomData 更新房间数据
* TraceLocation 更新足迹
* TakeSnapshot 更新快照

### 手动维护数据

在MUD有特别大的更新时，手动维护数据建议同时使用脚本和HMM客户端。

HMM客户端可以先去除已经失效的区域信息。

脚本可以以手动触发/自动抓取的形式，将房间信息输入HMM内，并进行出口连接。

然后利用HMM客户端的差异对比功能，检查更新的信息是否正确，并可以通过反向对比的方式，选择性的去除有问题的更新。

### 使用虚拟出口指令。

虚拟出口指令指在发送时会进行转换处理的指令。

一般建议在合适的位置加入#号代表指令

主要用来解决几个问题。

#### 开启一整组触发和代码。

比如常见的ask场景和乘船场景。

Ask场景需要开启重试功能，开启相应出发。

一般建议使用 #ask xxx about 出海 这样的形式

乘船的话，往往需要等待，开启下船的出发，等待靠岸

一般建议 #sail或者#wait这样的形式

#### 用于动态计算目的。

这种指令建议在原始指令后加入 #备注

比如 east#busy,west#fight 这样。

这样可以在环境 Environment 的CommandCost 中动态的调节Cost。

也能在MapperOptions中进行白名单的排除，实现规划驾车运镖等只能进入有限几个指令的情况。