# 数据维护接口

数据维护接口是对HellMapManager(hmm)中的9种基本数据结构 Room,Marker,Route,Trace,Region,Landmark,Shortcut,Variable,Snaphshot的List/Insert/Remove操作。

## 通用结构

### 列出数据选项 ListOption

列出数据选项是一个公用的数据列出过滤的数据结构，提供一种比较粗的过滤列出结果的方式。

**数据结构**

| 字段   | 类型      | 说明         |
| ------ | --------- | ------------ |
| Keys   | []string? | 过滤主键列表 |
| Groups | []strng?  | 过滤分组列表 |

* Keys 过滤的主键列表，为空则不过滤。
* Groups 过滤的分组列表，为空则不过滤。

## 插入房间接口 InsertRooms

批量插入房间。如果房间已经存在，会进行替换。

**请求地址:**

/api/db/insertrooms

**请求正文:**

| 字段                             | 类型      | 说明         |
| -------------------------------- | --------- | ------------ |
| Rooms                            | []object  | 房间列表     |
| --Rooms.Key                      | string    | 房间主键     |
| --Rooms.Name                     | string    | 房间名       |
| --Rooms.Desc                     | string    | 房间描述     |
| --Rooms.Group                    | string    | 房间分组     |
| --Rooms.Tags                     | []object  | 房间标签     |
| ----Rooms.TagsKey                | string    | 标签主键     |
| ----Rooms.Value                  | int       | 标签值       |
| --Rooms.Exits                    | []object  | 房间出口列表 |
| ----Rooms.Exits.Command          | string    | 出口指令     |
| ----Rooms.Exits.To               | string    | 出口房间     |
| ----Rooms.Exits.Cost             | int       | 出口消耗     |
| ----Rooms.Exits.Conditions       | []object  | 出口条件列表 |
| ----Rooms.Exits.Conditions.Key   | string    | 条件主键     |
| ----Rooms.Exits.Conditions.Not   | bool      | 条件取否     |
| ----Rooms.Exits.Conditions.Value | int       | 条件值       |
| --Rooms.Data                     | []object? | 房间数据列表 |
| ----Rooms.Data.Key               | string    | 数据主键     |
| ----Rooms.Data.Value             | string    | 数据值       |

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/insertrooms HTTP/1.1

{
  "Rooms":[
    {
        "Key":"NewRoom",
        "Name":"测试房间",
        "Desc":"这是一个测试房间",
        "Group":"测试区域",
        "Tags":[
            {
                "Key":"测试",
                "Value":1
            }
        ],
        "Exits":[
            {
                "Command":"leave",
                "To":"0",
                "Cost":10,
                "Conditions":[
                    {
                        "Key":"测试完成",
                        "Not":false,
                        "Value":1
                    }
                ]
            }
        ],
        "Data":[
            {
                "Key":"测试进度",
                "Value":"1/1"
            }
        ]
    }
  ]
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 05:08:45 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 列出房间接口

根据给到的ListOption,列出房间列表

**请求地址:**

/api/db/listrooms


**请求正文:**

| 字段 | 类型       | 说明     |
| ---- | ---------- | -------- |
|      | ListOption | 过滤选项 |

**返回结果：**

| 字段                        | 类型      | 说明         |
| --------------------------- | --------- | ------------ |
|                             | []object  | 房间列表     |
| --.Key                      | string    | 房间主键     |
| --.Name                     | string    | 房间名       |
| --.Desc                     | string    | 房间描述     |
| --.Group                    | string    | 房间分组     |
| --.Tags                     | []object  | 房间标签     |
| ----.TagsKey                | string    | 标签主键     |
| ----.Value                  | int       | 标签值       |
| --.Exits                    | []object  | 房间出口列表 |
| ----.Exits.Command          | string    | 出口指令     |
| ----.Exits.To               | string    | 出口房间     |
| ----.Exits.Cost             | int       | 出口消耗     |
| ----.Exits.Conditions       | []object  | 出口条件列表 |
| ----.Exits.Conditions.Key   | string    | 条件主键     |
| ----.Exits.Conditions.Not   | bool      | 条件取否     |
| ----.Exits.Conditions.Value | int       | 条件值       |
| --.Data                     | []object? | 房间数据列表 |
| ----.Data.Key               | string    | 数据主键     |
| ----.Data.Value             | string    | 数据值       |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/listrooms HTTP/1.1

{
  "Groups":["测试区域"]
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 05:21:52 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  {
    "Key": "NewRoom",
    "Name": "测试房间",
    "Desc": "这是一个测试房间",
    "Group": "测试区域",
    "Tags": [
      {
        "Key": "测试",
        "Value": 1
      }
    ],
    "Exits": [
      {
        "Command": "leave",
        "To": "0",
        "Conditions": [
          {
            "Key": "测试完成",
            "Not": false,
            "Value": 1
          }
        ],
        "Cost": 10
      }
    ],
    "Data": [
      {
        "Key": "测试进度",
        "Value": "1/1"
      }
    ]
  }
]
```

## 批量删除房间接口 RemoveRooms

通过给到的主键，批量删除房间。

如果主键不存在，则跳过该房间主键继续删除。

**请求地址:**

/api/db/removerooms

**请求正文:**

| 字段 | 类型     | 说明         |
| ---- | -------- | ------------ |
| Keys | []string | 房间主键列表 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/removerooms HTTP/1.1

{
    "Keys":["notexists","NewRoom"]
}
```
**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 05:43:56 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 插入标记接口 InsertMarkers

批量插入标记。如果房间已经存在，会进行替换。

**请求地址:**

/api/db/insertmarkers

**请求正文:**

| 字段              | 类型     | 说明                     |
| ----------------- | -------- | ------------------------ |
| Markers           | []object | 标记列表                 |
| --Markers.Key     | string   | 标记主键                 |
| --Markers.Value   | string   | 标记值，对应的房间Key    |
| --Markers.Group   | string   | 标记分组                 |
| --Markers.Desc    | string   | 标记描述                 |
| --Markers.Message | string   | 标记消息，一般供脚本使用 |

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/insertmarkers HTTP/1.1

{
    "Markers":[
        {
            "Key":"扬州中心",
            "Value":"0",
            "Group":"CityCenter",
            "Desc":"扬州地区中心",
            "Message":"area|yz"
        }
    ]
}

```
**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 05:57:09 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 列出标记接口

根据给到的ListOption,列出标记列表

**请求地址:**

/api/db/listmarkers


**请求正文:**

| 字段 | 类型       | 说明     |
| ---- | ---------- | -------- |
|      | ListOption | 过滤选项 |

**返回结果：**

| 字段       | 类型     | 说明                     |
| ---------- | -------- | ------------------------ |
|            | []object | 标记列表                 |
| --.Key     | string   | 标记主键                 |
| --.Value   | string   | 标记值，对应的房间Key    |
| --.Group   | string   | 标记分组                 |
| --.Desc    | string   | 标记描述                 |
| --.Message | string   | 标记消息，一般供脚本使用 |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/listmarkers HTTP/1.1

{
  "Groups":["CityCenter"]
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:01:41 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  {
    "Key": "扬州中心",
    "Value": "0",
    "Group": "CityCenter",
    "Desc": "扬州地区中心",
    "Message": "area|yz"
  }
]
```

## 批量删除标记接口 RemoveMarkers

通过给到的主键，批量删除标记。

如果主键不存在，则跳过该标记主键继续删除。

**请求地址:**

/api/db/removemarkers

**请求正文:**

| 字段 | 类型     | 说明         |
| ---- | -------- | ------------ |
| Keys | []string | 标记主键列表 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/removeMarkers HTTP/1.1

{
    "Keys":["notexists","扬州中心"]
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:03:25 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```