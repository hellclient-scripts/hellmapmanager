# 脚本支持

HellMapManager(HMM) 准备了相同算法的Typescript项目 HellMapManager.ts用于在支持脚本的客户端里提供原生的算法支持。

[项目地址](https://github.com/hellclient-scripts/hellmapmanager.ts)

目前HellMapManager.ts提供原生的Typescript库，以及编译生成的javascript以及lua库的支持。

*由于涉及大量的计算，HellMapManager.ts的javascript库需要现代化的js引擎，比如v8/JavaScriptCode/SpiderMonkey*

目前HMM也提供了HTTP接口。由于脚本是用TS转写了C#的对应代码，绝大部分功能与脚本是完全一致的。

目前仅有三处不同

* 脚本有DecodeRoomHook,EncodeRoomHook,DecodeShortcutHook,EncodeShortcutHook4个钩子函数在解码/编码HMM文件时提供处理数据的机会。HMM的代码中有这个机制，但没有合适的引入机会。
* 脚本使用的是上下文Context,客户端接口使用的是更容易做JSON编码的Environment。实际功能是等价的，仅为传输格式不同。
* 脚本使用的APIListOption，客户使用的是更容易做JSON编码的ListOption。实际功能是等价的，仅为传输格式不同。