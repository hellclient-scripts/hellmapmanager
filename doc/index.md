# 文档

## 基本概念和数据结构

HMM的基本数据分为房间(Room),标记(Marker),路线(Route),足迹(Trace),地区(Region),定位(Landmark),捷径(Shortcut),变量(Variable),快照(Snapshot)等9中基本数据机构和一些子结构

[查看详情](term/index.md)

## 伪动态路线规划

HMM可以通过动态传入临时房间，路径，捷径和各种设置构成的上下文(Context)来实现路线的伪动态规划功能。

[查看详情](dynamic/index.md)

## API接口文档

HMM提供了http协议接口提供服务，让任何支持http协议和json格式的客户端都能访问完整的功能。

[查看详情](api/index.md)

## 数据格式

HMM文件格式的详细格式，您可以按这个格式在自己的代码里解析或生成HMM文件。

* [查看详情](format/hmm.md)

## 最佳实践

一系列建议的HMM使用方式。

* [查看详情](bestpractices/index.md)