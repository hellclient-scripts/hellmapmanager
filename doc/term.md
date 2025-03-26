# 术语

## 区域(Zone)
将房间以树状结构进行分类，用于一次取出某个区域及其子区域下的所有房间。

### 属性列表
* Key 区域key
* Name 区域名称
* Desc 区域描述
* Parent 父区域，为空则为顶级区域
* Version 版本 每次更新会发生变化
* UpdatedTime 最后更新时间

## 房间(Room)
具体的房间
### 属性列表
* Key 房间Key
* Name 房间名称
* Zone 房间所属区域
* Disabled 禁用
* Group 分组
* Tags 标签，字符串数组
* Version 版本 每次更新会发生变化
* UpdatedTime 最后更新时间

## 路径(Path)
路径信息
### 属性列表
* ID 内部ID
* Command 指令
* From 出发房间
* To 目标房间
* Tags 必须标签
* ExTags 排除标签
* Cost 花费
* Disabled 禁用
* Version 版本
* UpdatedTime 最后更新时间

## 房间信息 InfoMation
房间的额外信息
### 属性列表
* Room 对应的房间
* Version 版本
* Data 数据键值对
* UpdatedTime 最后更新时间

## 位置(Position)
供相对引用的位置信息
### 属性列表
* Key 位置Key
* Rooms 位置对应的房间列表
* Version 版本
* UpdatedTime 最后更新时间