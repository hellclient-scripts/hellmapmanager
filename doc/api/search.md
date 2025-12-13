# 搜索房间接口

搜索房间接口是一系列专门用于根据一定的条件搜索符合条件的房间列表的接口。

## 房间过滤器 RoomFilter

房间过滤器是一个根据实际场景需要对房间进行过滤的数据结构。基本数据结构如下

| 字段                    | 类型      | 说明                         |
| ----------------------- | --------- | ---------------------------- |
| RoomConditions          | []object? | 匹配房间条件，留空则不过滤   |
| --RoomConditions.Key    | string    | 条件的主键                   |
| --RoomConditions.Not    | bool      | 条件取否                     |
| --RoomConditions.Value  | int       | 条件值                       |
| HasAnyExitTo            | []string? | 出口目标过滤，留空则不过滤   |
| HasAnyGroup              | []string? | 房间分组过滤，留空则不过滤 |
| HasAnyName              | []string? | 完整房间名过滤，留空则不过滤 |
| ContainsAnyName         | []string? | 部分房间名过滤，留空则不过滤 |
| HasAnyData              | []obecjt? | 完整数据值过滤，留空则不过滤 |
| --HasAnyData.Key        | string    | 数据主键                     |
| --HasAnyData.Value      | string    | 数据值，需要完整匹配         |
| ContainsAnyData         | []objext? | 部分数据值过滤，留空则不过滤 |
| --ContainsAnyData.Key   | string    | 数据主键                     |
| --ContainsAnyData.Value | string    | 数据值，部分匹配             |
| ContainsAnyKey          | []string? | 部分匹配房间主键             |

所以的字段都是可选的，不设置或者空数组代表不过滤。

所有已经设置的条件必须全部符合。

具体功能为
* RoomConditions 与房间的Tag进行匹配，必须全部符合
* HasAnyExitTo 必须具有到指定房间的出口
* HasAnyGroup 必须具有指定分组过滤，一般用于限制城市
* HasAnyName 根据完整房间名的筛选，可以用来查找是否是唯一的名称的房间
* ContainsAnyName 查找房间名字，部分匹配即可
* HasAnyData 匹配含有任何指定房间数据的房间。房间数据必须完整匹配。比如查找描述
* ContainsAnyData 匹配含有任何指定房间数据的房间。房间数据部分匹配。
* ContainsAnyKey 匹配房间key部分匹配的房间。一般较少用，除非key有特殊的规律