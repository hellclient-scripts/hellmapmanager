# HMM格式

HellMapManager(HMM)一共使用3种格式的数据文件

* .hmm 文本格式的地图信息
* .hmz 2进制(zip压缩)格式的地图信息
* .hmp 2进制(zip压缩)格式的补丁信息

除了是否通过zip压缩以外，这3种格式都是以行为单位，第一行为文件格式头，之后每行是特殊多层带分隔符的字符串数组。

编码与解码方式是一致的

## 转义

HMM格式中使用了多个保留字符作为控制符。包括：
* 换行 转义为 \\n
* \\ 转义为 \\\\
* \> 转义为 \\>
* | 转义为 \\|
* : 转义为\\:
* ; 转义为 \\;
* = 转义为 \\=
* , 转义为 \\,
* @ 转义为 \\@
* & 转义为 \\&
* ^ 转义为 \\^
* \` 转义为 \\\`
* ! 转义为 \\!

## 控制符

控制符分为 键值控制符和列表控制符。为了在脚本里能方便的解码和编码，HMM预设了最大5层控制符，每一层有一个建值控制符和列表控制符

* 第1层: 键值控制符\> 列表控制符 |
* 第2层: 键值控制符: 列表控制符 ;
* 第3层: 键值控制符= 列表控制符 ,
* 第4层: 键值控制符@ 列表控制符 &
* 第5层: 键值控制符^ 列表控制符 `

此外，还有统一的取否控制符 !

### 数据结构

HMM格式中的数据结构有5种

**列表**

字符串数组，所有的值由对应层的列表控制符控制。

例

```
a|b|c|d
```

**键值对(KeyValue)**

由一个字符串Key和一个字符串Value组成，由对应层的键值控制符分割。

例
```
KeyA>ValueA
```


**开关值**

由一个bool值和一个字符串值组成

bool值为true,就是字符串值本身。bool值为false,就是取否控制符拼接字符串

例
```
ValueTrue
!ValueNot
```

**开关键值对**

由一个bool值，一个字符串键，一个字符串值组成

先将键值拼接为键值对，再和bool值一起拼接为开关值

例
```
KeyTrue>ValueTrue
!KeyFalse>ValueFalse
```

## 顶级主键

HMM格式的数据每条记录都是一个键值对，顶级的主键在解析的时候决定了这是一条什么记录

顶级主键包括
* Info 信息数据
* Room 房间数据
* Marker 标记数据
* Route 路线数据
* Trace 足迹数据
* Region 地区数据
* Landmark 定位数据
* Shortcut 捷径数据
* Variable 变量数据
* Snapshot 快照数据
* RemoveRoom 移除房间,补丁格式中使用
* RemoveMarker 移除标记,补丁格式中使用
* RemoveRoute 移除路线,补丁格式中使用
* RemoveTrace 移除足迹,补丁格式中使用
* RemoveRegion 移除地区,补丁格式中使用
* RemoveLandmark 移除定位,补丁格式中使用
* RemoveShortcut 移除捷径,补丁格式中使用
* RemoveVariable 移除变量,补丁格式中使用
* RemoveSnapshot 移除快照,补丁格式中使用

## 文件头

地图和补丁文件的第一行文本是文件的文件头，程序是通过文件头来判断文件是否有效的

UTF8地图信息的文件头

```
HMM1.0>UTF8
```

GB18030地图信息的文件头
```
HMM1.0>GB18030
```
补丁文件的文件头
```
HMP1.0>
```

## 地图信息

hmm和hmz文件紧跟在文件头之后的应该是地图信息

地图信息的格式为

* 1级列表:地图名，地图保存unix时间戳,地图描述

例
```
Info>MapName|1700000000|MapDesc
```

## 房间信息记录

* 1级列表 Key,Name,Group,Desc，标签列表子信息，出口列表子信息，房间数据列表子信息
* 标签列表子信息为2级列表 ,内容为Tags的键值对（当值为1时以空字符串保存）
* 出口列表子信息为2级列表，内容为出口信息
* 房间数据子信息为2级列表，内容为房间数据的键值对
* 出口信息为3级列表。内容为 出口指令，出口目标，出口条件信息，出口消耗
* 出口条件为4级列表，内容为代表条件的5级的开关键值对，值为1是按空字符串保存
例
```
Room>1|西大街|扬州|描述|室外=2|n,1221,,1;enter dong,2317,gb^,1|
```

## 标记信息记录

* 1级列表 Key,Value,Group,Desc,Message

例
```
Marker>a zi|1160|npc||阿紫\|1160\|a zi
```

## 路线信息记录

* 1级列表 Key,Group,Desc,房间列表信息,Message
* 房间列表信息为2级列表，内容为转义后的Rooms信息

例
```
Route>Route1|RouteGroup|RougeDesc|394;396;394;393;392;414;415;414;392;393;394|
```

## 足迹信息记录

* 1级列表 Key,Group,Desc,位置列表信息,Message
* 位置列表信息为2级列表，内容为转义后的Locations信息

例
```
Trace>TraceKey|TraceGroup|TraceGroup|1;2;3;4;5;6;7;8|
```

## 地区信息记录

* 1级列表 Key,Group,Desc,地区元素列表信息，Mesage
* 地区元素列表信息为2级列表，内容为地区元素的开关键值对，房间类型的键为Room,区域类型的键为Zone

例
```
Region>RegionKeyA|RegionGroupA|RegionDescA|Room:1;Zone:aaaa;!Room:2220|
```

## 定位信息记录

* 1级列表 Key,Type,Value,Group,Desc

例
```
Landmark>2400|regexp|\^象一蓬蓬巨伞般伸向天空，把阳光遮得丝毫也无。尺把厚的松针|LandmarkDesc|
```

## 捷径信息记录

* 1级列表 Key,Group,Desc,房间条件列表，Command,To,环境条件列表，Cost
* 房间条件列表为2级列表，内容为RoomConditions的开关键值对，值为1按空字符串保存
* 环境条件列表为2级列表，内容为Conditions的开关键值对，值为1按空字符串保存

例
```
Shortcut>rideto_changan|ride|ShortcutDesc|室外=|rideto changan|370|ride=|2
```

## 变量信息记录

* 1级列表 Key,Value,Group,Desc

例
```
Variable>Key|Value|Group|Desc
```

## 快照信息记录

* 1级列表 Key,Type,Value,Group,Timestamp,Count
  
例
```
Snapshot>SnapshotKey|SnapshotType|SnapshotValue|SnapshotGroup|1700000000|1
```

## 移除类信息记录

移除房间,移除标记，移除路线，移除足迹，移除地区，移除捷径，移除变量

* 1级值为 Key

移除定位

* 1级列表为 Key,Type

移除快照
* 1级列表为 Key,Type,Value

例
```
RemoveRoom>0
RemoveMarker>a zi
RemoveRoute>成都
RemoveTrace>TraceKey
RemoveRegion>RegionKey
RemoveLandmark>Key|Type
RemoveShortcut>rideto_beijing
RemoveVariable>VariableKey
RemoveSnapshot>SnapthotKey|SnapshotType|SnapshotValue
```
