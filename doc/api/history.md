# API变更记录

## Version 1006

* Environment 加入RoomTags属性
* Context 加入RoomsTags属性，WithRoomsTags方法和RoomsTags方法

## Version 1005

* MapperOptions加入 CommandNotContains属性

## Version 1004

与hellmapmanager.ts的代码同步，对接口无影响。

## Version 1003

* SearchSnapshots 接口加入MaxNoise

## Version 1002

* MapperOptions加入 CommandWhitelist属性
* CommandCost允许以空字符串To作为通配符
* AddLocations中的空格会被忽略
* SnapshotFilter 加入MaxCount属性

## Version 1001

* RoomFilter 中加入 HasAnyGroup

## Version 1000

* 加入接口GetVersion