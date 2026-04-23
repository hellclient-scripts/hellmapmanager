# 动态链接

处于性能考虑，HellMapManager提供动态链接库(dll/so)支持。

动态链接库基本是api的动态链接方式

所有的接口统一的参数为
* input正文c字符串
* encoding 编码 int,0为utf8,1为gbk

返回值为c字符串。

当发生错误/意外/失败时，返回空字符串

## 注意事项

一般同一个客户端的不同游戏/脚本调用同一个dll时会共用地图数据，即地图数据在一个客户端内是通用的

## 动态链接专用接口

### import 导入

导入地图数据,必须没有已经导入的地图。

* 参数1为hmm文件正文
* 参数2为编码
* 返回值为 json bool值是否成功(解析失败或者已经有地图返回false)

### close 关闭

关闭地图，可以重新import

* 参数1 随意
* 参数2为编码
* 返回值为 json bool值是否成功(没有import地图为false)

### create 创建地图

创建地图

* 参数1 随意
* 参数2为编码
* 返回值为 json bool值是否成功(已经打开地图为false)

### export 导出当前地图

导出地图，可以用来import

* 参数1 随意
* 参数2为编码
* 返回值为 到处结果。到处失败为空字符串

## 标准接口

参考api中的对应接口，参数1为请求正文，参数2为 编码，返回值为接口响应

* version
* info
* listrooms
* removerooms
* insertrooms
* listmarkers
* insertmarkers
* removemarkers
* listroutes
* removeroutes
* insertroutes
* listtraces
* removetraces
* inserttraces
* listregions
* removeregions
* insertregions
* listshortcuts
* removeshortcuts
* insertshortcuts
* listvariables
* removevariables
* insertvariables
* listlandmarks
* removelandmarks
* insertlandmarks
* listsnapshots
* removesnapshots
* insertsnapshots
* querypathany
* querypathall
* querypathordered
* dilate
* trackexit
* getvariable
* queryregionrooms
* getroom
* clearsnapshots
* takesnapshot
* searchsnapshots
* searchrooms
* filterrooms
* grouproom
* tagroom
* setroomdata
* tracelocation
* getroomexits

## 范例代码

参考[范例代码](./example/index.md)