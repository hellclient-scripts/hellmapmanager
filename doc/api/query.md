# 查询计算接口

查询计算接口的特点是都会加入环境Environment和地图选项Option来动态查询和计算


## 通用数据结构
### 环境 Environment

环境指在计算和查询地图时传入的对地图的临时修正，参考术语中的上下文Context,是上下文信息以适合HTTP调用的方式的变形。

环境往往会在多个查询计算里复用

**数据结构：**

| 字段                                         | 类型      | 说明             |
| -------------------------------------------- | --------- | ---------------- |
| Tags                                         | []object? | 环境标签列表     |
| --Tags.Key                                   | string    | 标签主键         |
| --Tags.Value                                 | int       | 标签值           |
| RoomConditions                               | []object? | 房间条件列表     |
| --RoomConditions.Key                         | string    | 条件主键         |
| --RoomConditions.Not                         | bool      | 条件取否         |
| --RoomConditions.Value                       | int       | 条件值           |
| Rooms                                        | []object? | 临时房间列表     |
| --Rooms.Key                                  | string    | 房间主键         |
| --Rooms.Name                                 | string    | 房间名           |
| --Rooms.Desc                                 | string    | 房间描述         |
| --Rooms.Group                                | string    | 房间分组         |
| --Rooms.Tags                                 | []object  | 房间标签         |
| ----Rooms.Tags.Key                           | string    | 标签主键         |
| ----Rooms.Tags.Value                         | int       | 标签值           |
| --Rooms.Exits                                | []object  | 房间出口列表     |
| ----Rooms.Exits.Command                      | string    | 出口指令         |
| ----Rooms.Exits.To                           | string    | 出口房间         |
| ----Rooms.Exits.Cost                         | int       | 出口消耗         |
| ----Rooms.Exits.Conditions                   | []object  | 出口条件列表     |
| ------Rooms.Exits.Conditions.Key             | string    | 条件主键         |
| ------Rooms.Exits.Conditions.Not             | bool      | 条件取否         |
| ------Rooms.Exits.Conditions.Value           | int       | 条件值           |
| --Rooms.Data                                 | []object? | 房间数据列表     |
| ----Rooms.Data.Key                           | string    | 数据主键         |
| ----Rooms.Data.Value                         | string    | 数据值           |
| Paths                                        | []object? | 临时路径列表     |
| --Paths.Command                              | string    | 路径指令         |
| --Paths.To                                   | string    | 路径目标         |
| --Paths.Cost                                 | int       | 路径消耗         |
| --Paths.Conditions                           | []object  | 路径条件列表     |
| ----Paths.Conditions.Key                     | string    | 条件主键         |
| ----Paths.Conditions.Not                     | bool      | 条件取否         |
| ----Paths.Conditions.Value                   | int       | 条件值           |
| Shortcuts                                    | []object? | 临时捷径列表     |
| --Shortcuts.Command                          | string    | 捷径指令         |
| --Shortcuts.To                               | string    | 捷径目标         |
| --Shortcuts.Cost                             | int       | 捷径消耗         |
| ----Shortcuts.Conditions                     | []object  | 捷径条件         |
| ------Shortcuts.Conditions.Key               | string    | 条件主键         |
| ------Shortcuts.Conditions.Not               | bool      | 条件取否         |
| ------Shortcuts.Conditions.Value             | int       | 条件值           |
| ----Shortcuts.RoomConditionExitModel         | []object  | 房间条件列表     |
| ------Shortcuts.RoomConditionExitModel.Key   | string    | 条件主键         |
| ------Shortcuts.RoomConditionExitModel.Not   | bool      | 条件取否         |
| ------Shortcuts.RoomConditionExitModel.Value | int       | 条件值           |
| Whitelist                                    | []string? | 白名单房间列表   |
| Blacklist                                    | []string? | 黑名单房间列表   |
| BlockedLinks                                 | []object? | 拦截连接列表     |
| --Blockedlinks.From                          | string    | 连接起点         |
| --Blockedlinks.To                            | string    | 连接终点         |
| CommandCosts                                 | []object? | 临时指令消耗列表 |
| --CommandCosts.Command                       | string    | 指令             |
| --CommandCosts.To                            | string    | 指令目标         |
| --CommandCosts.Cost                          | int       | 指令消耗         |

### 地图选项 MapperOptions

地图选项指一次计算时的特殊设置

**数据结构：**

| 字段             | 类型  | 说明           |
| ---------------- | ----- | -------------- |
| MaxExitCost      | int?  | 最大单出口消耗 |
| MaxTotalCost     | int?  | 最大总路线消耗 |
| DisableShortcuts | bool? | 禁用捷径标志   |

### 路线规划查询结果 QueryResult

路线规划查询结果 是标准的路线规划的查询结果

**数据结构：**

| 字段            | 类型     | 说明           |
| --------------- | -------- | -------------- |
| From            | string   | 路线起点       |
| To              | string   | 路线终点       |
| Cost            | int      | 路线总消耗     |
| Steps           | []object | 路线步骤列表   |
| --Steps.Command | string   | 步骤指令       |
| --Steps.Target  | string   | 步骤目标       |
| --Steps.Cost    | int      | 步骤消耗       |
| Unvisited       | []string | 未经过房间列表 |

## 点对点规划接口

规划不定数量个起点到不定数量个终点之间的最近路线。

一般常见的是一个起点到一个终点，或者一个起点到多个终点的规划

**请求地址:**

/api/db/querypathany

**请求正文:**

| 字段        | 类型          | 说明     |
| ----------- | ------------- | -------- |
| From        | []string      | 起点列表 |
| Target      | []string      | 终点列表 |
| Environment | Environment   | 环境信息 |
| Options     | MapperOptions | 地图选项 |

**返回结果：**

成功返回 QueryResult

失败返回 null

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/querypathany HTTP/1.1

{
    "From" :["0"],
    "Target":["1"],
    "Environment":{
    },
    "Options":{}
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 14:32:25 GMT
Server: HellMapManager
Transfer-Encoding: chunked

{
  "From": "0",
  "To": "1",
  "Cost": 1,
  "Steps": [
    {
      "Command": "w",
      "Target": "1",
      "Cost": 1
    }
  ],
  "Unvisited": []
}
```

## 点对点规划接口

规划不定数量个起点到不定数量个终点之间的最近路线。

一般常见的是一个起点到一个终点，或者一个起点到多个终点的规划

**请求地址:**

/api/db/querypathany

**请求正文:**

| 字段        | 类型          | 说明     |
| ----------- | ------------- | -------- |
| From        | []string      | 起点列表 |
| Target      | []string      | 终点列表 |
| Environment | Environment   | 环境信息 |
| Options     | MapperOptions | 地图选项 |

**返回结果：**

成功返回 QueryResult

失败返回 null

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/querypathany HTTP/1.1

{
    "From" :["0"],
    "Target":["1"],
    "Environment":{
    },
    "Options":{}
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 14:32:25 GMT
Server: HellMapManager
Transfer-Encoding: chunked

{
  "From": "0",
  "To": "1",
  "Cost": 1,
  "Steps": [
    {
      "Command": "w",
      "Target": "1",
      "Cost": 1
    }
  ],
  "Unvisited": []
}
```

## 点对点规划接口

规划不定数量个起点到不定数量个终点之间的最近路线。

一般常见的是一个起点到一个终点，或者一个起点到多个终点的规划

未经过的终点会记录在查询结果的Unvisited里


**请求地址:**

/api/db/querypathany

**请求正文:**

| 字段        | 类型          | 说明     |
| ----------- | ------------- | -------- |
| From        | []string      | 起点列表 |
| Target      | []string      | 终点列表 |
| Environment | Environment   | 环境信息 |
| Options     | MapperOptions | 地图选项 |

**返回结果：**

成功返回 QueryResult

失败返回 null

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/querypathany HTTP/1.1

{
    "From" :["0"],
    "Target":["1"],
    "Environment":{},
    "Options":{}
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 14:32:25 GMT
Server: HellMapManager
Transfer-Encoding: chunked

{
  "From": "0",
  "To": "1",
  "Cost": 1,
  "Steps": [
    {
      "Command": "w",
      "Target": "1",
      "Cost": 1
    }
  ],
  "Unvisited": []
}
```

## 范围遍历规划接口

规划一个起点到最终经过所有目标的路线

无法经过的目标会记录在查询结果的Unvisited里

**请求地址:**

/api/db/querypathall

**请求正文:**

| 字段        | 类型          | 说明     |
| ----------- | ------------- | -------- |
| Start       | string        | 起点     |
| Target      | []string      | 终点列表 |
| Environment | Environment   | 环境信息 |
| Options     | MapperOptions | 地图选项 |

**返回结果：**

成功返回 QueryResult

失败返回 null

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/querypathall HTTP/1.1

{
    "Start" :"0",
    "Target":["1","25","26"],
    "Environment":{},
    "Options":{}
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 14:39:07 GMT
Server: HellMapManager
Transfer-Encoding: chunked

{
  "From": "0",
  "To": "25",
  "Cost": 7,
  "Steps": [
    {
      "Command": "w",
      "Target": "1",
      "Cost": 1
    },
    {
      "Command": "e",
      "Target": "0",
      "Cost": 1
    },
    {
      "Command": "n",
      "Target": "22",
      "Cost": 1
    },
    {
      "Command": "e·",
      "Target": "26",
      "Cost": 1
    },
    {
      "Command": "w",
      "Target": "22",
      "Cost": 1
    },
    {
      "Command": "n",
      "Target": "24",
      "Cost": 1
    },
    {
      "Command": "w·",
      "Target": "25",
      "Cost": 1
    }
  ],
  "Unvisited": []
}
```

## 顺序遍历规划接口

规划一个起点，按顺序经过所有目标的路线

无法经过的目标会记录在查询结果的Unvisited里

**请求地址:**

/api/db/querypathordered

**请求正文:**

| 字段        | 类型          | 说明     |
| ----------- | ------------- | -------- |
| Start       | string        | 起点     |
| Target      | []string      | 终点列表 |
| Environment | Environment   | 环境信息 |
| Options     | MapperOptions | 地图选项 |

**返回结果：**

成功返回 QueryResult

失败返回 null

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/querypathordered HTTP/1.1

{
    "Start" :"0",
    "Target":["1","25","26"],
    "Environment":{},
    "Options":{}
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 14:40:34 GMT
Server: HellMapManager
Transfer-Encoding: chunked

{
  "From": "0",
  "To": "26",
  "Cost": 8,
  "Steps": [
    {
      "Command": "w",
      "Target": "1",
      "Cost": 1
    },
    {
      "Command": "e",
      "Target": "0",
      "Cost": 1
    },
    {
      "Command": "n",
      "Target": "22",
      "Cost": 1
    },
    {
      "Command": "n",
      "Target": "24",
      "Cost": 1
    },
    {
      "Command": "w·",
      "Target": "25",
      "Cost": 1
    },
    {
      "Command": "e",
      "Target": "24",
      "Cost": 1
    },
    {
      "Command": "s",
      "Target": "22",
      "Cost": 1
    },
    {
      "Command": "e·",
      "Target": "26",
      "Cost": 1
    }
  ],
  "Unvisited": []
}
```

## 膨胀计算接口 Dilate

计算从初始房间列表膨胀指定次数后的新的房间列表。

一般用于获取地图上给点的房间，扩展几个房间后的区域。

**请求地址:**

/api/db/dilate

**请求正文:**

| 字段        | 类型          | 说明         |
| ----------- | ------------- | ------------ |
| Source      | []string      | 初始房间列表 |
| Iterations  | int           | 膨胀次数     |
| Environment | Environment   | 环境信息     |
| Options     | MapperOptions | 地图选项     |


**返回结果：**
| 字段 | 类型     | 说明             |
| ---- | -------- | ---------------- |
|      | []string | 跨站后的房间列表 |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/dilate HTTP/1.1

{
    "Source" :["0","1,","2"],
    "Iterations": 2 ,
    "Environment":{},
    "Options":{}
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 15:40:44 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  "0",
  "2",
  "59",
  "1927",
  "22",
  "40",
  "1",
  "3",
  "64",
  "60",
  "62",
  "26",
  "24",
  "23",
  "48",
  "49",
  "41",
  "5",
  "7",
  "1550",
  "4",
  "21"
]

```

## 跟踪出口 TrackExit

追踪从制定的起点，通过Command指令能到的房间。

成功返回房间Key

失败返回空字符串

**请求地址:**

/api/db/trackexit

**请求正文:**

| 字段        | 类型          | 说明     |
| ----------- | ------------- | -------- |
| Start       | string        | 起点     |
| Command     | string        | 指令     |
| Environment | Environment   | 环境信息 |
| Options     | MapperOptions | 地图选项 |


**返回结果：**
| 字段 | 类型   | 说明                                 |
| ---- | ------ | ------------------------------------ |
|      | string | 出口对应的房间。追踪失败返回空字符串 |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/trackexit HTTP/1.1

{
    "Start" :"0",
    "Command": "n" ,
    "Environment":{},
    "Options":{}
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 15:47:49 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"22"

```

## 获取房间 GetRoom

获取指定Key的房间。

包含环境中的临时房间。

成功返回房间对象，失败返回空。

**请求地址:**

/api/db/getroom

**请求正文:**

| 字段        | 类型          | 说明     |
| ----------- | ------------- | -------- |
| Key         | string        | 房间主键 |
| Environment | Environment   | 环境信息 |
| Options     | MapperOptions | 地图选项 |


**返回结果：**
| 字段                     | 类型      | 说明         |
| ------------------------ | --------- | ------------ |
|                          | object?   | 房间         |
| Key                      | string    | 房间主键     |
| Name                     | string    | 房间名       |
| Desc                     | string    | 房间描述     |
| Group                    | string    | 房间分组     |
| Tags                     | []object  | 房间标签     |
| --Key                    | string    | 标签主键     |
| --Value                  | int       | 标签值       |
| Exits                    | []object  | 房间出口列表 |
| --Exits.Command          | string    | 出口指令     |
| --Exits.To               | string    | 出口房间     |
| --Exits.Cost             | int       | 出口消耗     |
| --Exits.Conditions       | []object  | 出口条件列表 |
| --Exits.Conditions.Key   | string    | 条件主键     |
| --Exits.Conditions.Not   | bool      | 条件取否     |
| --Exits.Conditions.Value | int       | 条件值       |
| Data                     | []object? | 房间数据列表 |
| --Data.Key               | string    | 数据主键     |
| --Data.Value             | string    | 数据值       |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/getroom HTTP/1.1

{
    "Key" :"0",
    "Environment":{},
    "Options":{}
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 15:58:01 GMT
Server: HellMapManager
Transfer-Encoding: chunked

{
  "Key": "0",
  "Name": "中央广场",
  "Desc": "",
  "Group": "",
  "Tags": [
    {
      "Key": "室外",
      "Value": 1
    }
  ],
  "Exits": [
    {
      "Command": "e",
      "To": "59",
      "Conditions": [],
      "Cost": 1
    },
    {
      "Command": "enter dong",
      "To": "1927",
      "Conditions": [],
      "Cost": 1
    },
    {
      "Command": "n",
      "To": "22",
      "Conditions": [],
      "Cost": 1
    },
    {
      "Command": "s",
      "To": "40",
      "Conditions": [],
      "Cost": 1
    },
    {
      "Command": "w",
      "To": "1",
      "Conditions": [],
      "Cost": 1
    }
  ],
  "Data": []
}
```

## 获取房间 GetRoomExits

获取指定Key的房间的出口。

包含环境中的临时房间，捷径和临时路径。

可以通过Options的DisableShortcuts排除捷径

成功返回出口列表，失败返回空列表。

**请求地址:**

/api/db/getroomexits

**请求正文:**

| 字段        | 类型          | 说明     |
| ----------- | ------------- | -------- |
| Key         | string        | 房间主键 |
| Environment | Environment   | 环境信息 |
| Options     | MapperOptions | 地图选项 |


**返回结果：**
| 字段                | 类型     | 说明         |
| ------------------- | -------- | ------------ |
|                     | []object | 房间出口列表 |
| .Command            | string   | 出口指令     |
| .To                 | string   | 出口房间     |
| .Cost               | int      | 出口消耗     |
| .Conditions         | []object | 出口条件列表 |
| --.Conditions.Key   | string   | 条件主键     |
| --.Conditions.Not   | bool     | 条件取否     |
| --.Conditions.Value | int      | 条件值       |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/getroomexits HTTP/1.1

{
    "Key" :"26",
    "Environment":{},
    "Options":{}
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 16:02:07 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  {
    "Command": "menter0·",
    "To": "2046",
    "Conditions": [],
    "Cost": 1
  },
  {
    "Command": "s",
    "To": "1553",
    "Conditions": [],
    "Cost": 1
  },
  {
    "Command": "u·",
    "To": "-1",
    "Conditions": [],
    "Cost": 1
  },
  {
    "Command": "w",
    "To": "22",
    "Conditions": [],
    "Cost": 1
  }
]
```