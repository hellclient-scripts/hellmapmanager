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

## 列出房间接口 ListRooms

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

## 列出标记接口 ListMarkers

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

## 插入路线接口 InsertRoutes

批量插入路线。如果路线已经存在，会进行替换。

**请求地址:**

/api/db/insertroutes

**请求正文:**

| 字段       | 类型     | 说明                  |
| ---------- | -------- | --------------------- |
| Routes     | []object | 路线列表              |
| --.Key     | string   | 路线主键              |
| --.Desc    | string   | 路线描述              |
| --.Group   | string   | 路线分组              |
| --.Message | string   | 路线消息              |
| --.Rooms   | []string | 路线包含的房间Key列表 |

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/insertroutes HTTP/1.1

{
  "Routes":[
    {
      "Key":"route1",
      "Desc":"示例路线",
      "Group":"默认",
      "Message":"msg|example",
      "Rooms":["0","1","2"]
    }
  ]
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:14:41 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 列出路线接口 ListRoutes

根据给到的 `ListOption`，列出路线列表。

**请求地址:**

/api/db/listroutes

**请求正文:**

| 字段 | 类型       | 说明     |
| ---- | ---------- | -------- |
|      | ListOption | 过滤选项 |

**返回结果：**

| 字段       | 类型     | 说明              |
| ---------- | -------- | ----------------- |
|            | []object | 路线列表          |
| --.Key     | string   | 路线主键          |
| --.Desc    | string   | 路线描述          |
| --.Group   | string   | 路线分组          |
| --.Message | string   | 路线消息          |
| --.Rooms   | []string | 路线包含的房间Key |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/listroutes HTTP/1.1

{
  "Groups":["默认"]
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:14:52 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  {
    "Key": "route1",
    "Desc": "示例路线",
    "Group": "默认",
    "Message": "msg|example",
    "Rooms": [
      "0",
      "1",
      "2"
    ]
  }
]
```

## 批量删除路线接口 RemoveRoutes

通过给到的主键，批量删除路线。如果主键不存在，则跳过该路线主键继续删除。

**请求地址:**

/api/db/removeroutes

**请求正文:**

| 字段 | 类型     | 说明         |
| ---- | -------- | ------------ |
| Keys | []string | 路线主键列表 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/removeroutes HTTP/1.1

{
    "Keys":["notexists","route1"]
}
```

**示例结果:**

```
HTTP/1.1 200 OK

## 插入足迹接口 InsertTraces

批量插入足迹(Trace)。如果足迹已经存在，会进行替换。

**请求地址:**

/api/db/inserttraces


**请求正文:**

| 字段               | 类型     | 说明              |
| ------------------ | -------- | ----------------- |
| Traces             | []object | 足迹列表          |
| --Traces.Key       | string   | 足迹主键          |
| --Traces.Group     | string   | 足迹分组          |
| --Traces.Desc      | string   | 足迹描述          |
| --Traces.Message   | string   | 足迹消息          |
| --Traces.Locations | []string | 足迹包含的房间Key |

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/inserttraces HTTP/1.1

{
  "Traces":[
    {
      "Key":"mytrace",
      "Group":"默认",
      "Desc":"示例足迹",
      "Message":"tag|trace",
      "Locations":["0","1","2"]
    }
  ]
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:20:42 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 列出足迹接口 ListTraces

根据给到的 `ListOption`，列出足迹列表。

**请求地址:**

/api/db/listtraces


**请求正文:**

| 字段 | 类型       | 说明     |
| ---- | ---------- | -------- |
|      | ListOption | 过滤选项 |

**返回结果：**

| 字段         | 类型     | 说明              |
| ------------ | -------- | ----------------- |
|              | []object | 足迹列表          |
| --.Key       | string   | 足迹主键          |
| --.Group     | string   | 足迹分组          |
| --.Desc      | string   | 足迹描述          |
| --.Message   | string   | 足迹消息          |
| --.Locations | []string | 足迹包含的房间Key |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/listtraces HTTP/1.1

{
  "Groups":["默认"]
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:20:59 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  {
    "Key": "mytrace",
    "Group": "默认",
    "Desc": "示例足迹",
    "Message": "tag|trace",
    "Locations": [
      "0",
      "1",
      "2"
    ]
  }
]
```

## 批量删除足迹接口 RemoveTraces

通过给到的主键，批量删除足迹。如果主键不存在，则跳过该主键继续删除。

**请求地址:**

/api/db/removetraces


**请求正文:**

| 字段 | 类型     | 说明         |
| ---- | -------- | ------------ |
| Keys | []string | 足迹主键列表 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/removetraces HTTP/1.1

{
    "Keys":["notexists","mytrace"]
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:21:12 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 插入地区接口 InsertRegions

批量插入地区(Region)。如果地区已经存在，会进行替换。

**请求地址:**

/api/db/insertregions


**请求正文:**

| 字段              | 类型     | 说明                           |
| ----------------- | -------- | ------------------------------ |
| Regions           | []object | 地区列表                       |
| --Regions.Key     | string   | 地区主键                       |
| --Regions.Group   | string   | 地区分组                       |
| --Regions.Desc    | string   | 地区描述                       |
| --Regions.Message | string   | 地区消息（供脚本/标签使用）    |
| --Regions.Items   | []object | 地区元素列表                   |
| ----Items.Not     | bool     | 是否为排除项（true 表示排除）  |
| ----Items.Type    | string   | 元素类型："Room" 或 "Zone"     |
| ----Items.Value   | string   | 元素值（房间 Key 或 区域标识） |

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/insertregions HTTP/1.1

{
  "Regions":[
    {
      "Key":"myregion",
      "Group":"默认",
      "Desc":"示例地区",
      "Message":"tag|region",
      "Items":[
        { "Not":false, "Type":"Room", "Value":"0" },
        { "Not":true,  "Type":"Room", "Value":"2220" },
        { "Not":false, "Type":"Zone", "Value":"aaaa" }
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
Date: Mon, 15 Dec 2025 06:25:47 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 列出地区接口 ListRegions

根据给到的 `ListOption`，列出地区列表。

**请求地址:**

/api/db/listregions


**请求正文:**

| 字段 | 类型       | 说明     |
| ---- | ---------- | -------- |
|      | ListOption | 过滤选项 |

**返回结果：**

| 字段             | 类型     | 说明                |
| ---------------- | -------- | ------------------- |
|                  | []object | 地区列表            |
| --.Key           | string   | 地区主键            |
| --.Group         | string   | 地区分组            |
| --.Desc          | string   | 地区描述            |
| --.Message       | string   | 地区消息            |
| --.Items         | []object | 地区元素列表        |
| ----.Items.Not   | bool     | 是否为排除项        |
| ----.Items.Type  | string   | 元素类型(Room/Zone) |
| ----.Items.Value | string   | 元素值              |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/listregions HTTP/1.1

{
  "Groups":["默认"]
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:30:12 GMT
Server: HellMapManager
Transfer-Encoding: chunked

HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:26:14 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  {
    "Key": "myregion",
    "Group": "默认",
    "Desc": "示例地区",
    "Message": "tag|region",
    "Items": [
      {
        "Not": false,
        "Type": "Room",
        "Value": "0"
      },
      {
        "Not": true,
        "Type": "Room",
        "Value": "2220"
      },
      {
        "Not": false,
        "Type": "Zone",
        "Value": "aaaa"
      }
    ]
  }
]
```

## 批量删除地区接口 RemoveRegions

通过给到的主键，批量删除地区。如果主键不存在，则跳过该主键继续删除。

**请求地址:**

/api/db/removeregions


**请求正文:**

| 字段 | 类型     | 说明         |
| ---- | -------- | ------------ |
| Keys | []string | 地区主键列表 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/removeregions HTTP/1.1

{
    "Keys":["notexists","myregion"]
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:26:36 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[]
```

## 插入定位接口 InsertLandmarks

批量插入定位。如果定位已经存在，会进行替换。

**请求地址:**

/api/db/insertlandmarks

**请求正文:**

| 字段              | 类型     | 说明                         |
| ----------------- | -------- | ---------------------------- |
| Landmarks         | []object | 定位列表                     |
| --Landmarks.Key   | string   | 定位主键，一般是房间主键     |
| --Landmarks.Type  | string   | 定位类型（用于区分同名定位） |
| --Landmarks.Value | string   | 定位值，用于定位的数据       |
| --Landmarks.Group | string   | 定位分组                     |
| --Landmarks.Desc  | string   | 定位描述                     |

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/insertlandmarks HTTP/1.1

{
  "Landmarks":[
    {
      "Key":"TreeTop",
      "Type":"regexp",
      "Value":"^象一蓬蓬巨伞般伸向天空",
      "Group":"Scenery",
      "Desc":"树冠的描述，供匹配与显示使用"
    }
  ]
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:33:54 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 列出定位接口 ListLandmarks

根据给到的 `ListOption`，列出定位列表。

**请求地址:**

/api/db/listlandmarks

**请求正文:**

| 字段 | 类型       | 说明     |
| ---- | ---------- | -------- |
|      | ListOption | 过滤选项 |

**返回结果：**

| 字段     | 类型     | 说明     |
| -------- | -------- | -------- |
|          | []object | 定位列表 |
| --.Key   | string   | 定位主键 |
| --.Type  | string   | 定位类型 |
| --.Value | string   | 定位值   |
| --.Group | string   | 定位分组 |
| --.Desc  | string   | 定位描述 |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/listlandmarks HTTP/1.1

{
  "Groups":["Scenery"]
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:34:02 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  {
    "Key": "TreeTop",
    "Type": "regexp",
    "Value": "^象一蓬蓬巨伞般伸向天空",
    "Group": "Scenery",
    "Desc": "树冠的描述，供匹配与显示使用"
  }
]
```

## 批量删除定位接口 RemoveLandmarks

通过给到的主键列表，批量删除定位。主键由 `Key` 与 `Type` 两部分组成。

**请求地址:**

/api/db/removelandmarks

**请求正文:**

| 字段                | 类型     | 说明              |
| ------------------- | -------- | ----------------- |
| LandmarkKeys        | []object | 定位主键列表      |
| --LandmarkKeys.Key  | string   | 定位主键的 `Key`  |
| --LandmarkKeys.Type | string   | 定位主键的 `Type` |

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/removelandmarks HTTP/1.1

{
  "LandmarkKeys": [
    { "Key": "TreeTop", "Type": "regexp" }
  ]
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:34:14 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```


## 插入快捷出口接口 InsertShortcuts

批量插入快捷出口。如果已存在同主键的快捷出口，会进行替换。

**请求地址:**

/api/db/insertshortcuts

**请求正文:**

| 字段                               | 类型     | 说明         |
| ---------------------------------- | -------- | ------------ |
| Shortcuts                          | []object | 快捷出口列表 |
| --Shortcuts.Key                    | string   | 快捷出口主键 |
| --Shortcuts.Command                | string   | 出口命令     |
| --Shortcuts.To                     | string   | 目标房间Key  |
| --Shortcuts.RoomConditions         | []object | 房间条件列表 |
| ----Shortcuts.RoomConditions.Key   | string   | 条件主键     |
| ----Shortcuts.RoomConditions.Not   | bool     | 条件取否     |
| ----Shortcuts.RoomConditions.Value | int      | 条件值       |
| --Shortcuts.Conditions             | []object | 出口条件列表 |
| ----Shortcuts.Conditions.Key       | string   | 条件主键     |
| ----Shortcuts.Conditions.Not       | bool     | 条件取否     |
| ----Shortcuts.Conditions.Value     | int      | 条件值       |
| --Shortcuts.Cost                   | int      | 出口消耗     |
| --Shortcuts.Group                  | string   | 分组         |
| --Shortcuts.Desc                   | string   | 描述         |

ValueCondition 结构：

| 字段  | 类型   | 说明     |
| ----- | ------ | -------- |
| Key   | string | 条件主键 |
| Not   | bool   | 是否取非 |
| Value | int    | 条件值   |

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/insertshortcuts HTTP/1.1

{
  "Shortcuts": [
    {
      "Key": "FastPass",
      "Command": "shortcut",
      "To": "100",
      "RoomConditions": [ { "Key": "is_safe", "Not": false, "Value": 1 } ],
      "Conditions": [],
      "Cost": 1,
      "Group": "Default",
      "Desc": "当房间安全时可用的快捷出口"
    }
  ]
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:41:52 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 列出快捷出口接口 ListShortcuts

根据给到的 `ListOption`，列出快捷出口列表。

**请求地址:**

/api/db/listshortcuts

**请求正文:**

| 字段 | 类型       | 说明     |
| ---- | ---------- | -------- |
|      | ListOption | 过滤选项 |

**返回结果：**

| 字段                      | 类型     | 说明         |
| ------------------------- | -------- | ------------ |
|                           | []object | 快捷出口列表 |
| --.Key                    | string   | 快捷出口主键 |
| --.Command                | string   | 出口命令     |
| --.To                     | string   | 目标房间Key  |
| --.RoomConditions         | []object | 房间条件列表 |
| ----.RoomConditions.Key   | string   | 条件主键     |
| ----.RoomConditions.Not   | bool     | 条件取否     |
| ----.RoomConditions.Value | int      | 条件值       |
| --.Conditions             | []object | 出口条件列表 |
| ----.Conditions.Key       | string   | 条件主键     |
| ----.Conditions.Not       | bool     | 条件取否     |
| ----.Conditions.Value     | int      | 条件值       |
| --.Cost                   | int      | 出口消耗     |
| --.Group                  | string   | 分组         |
| --.Desc                   | string   | 描述         |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/listshortcuts HTTP/1.1

{
  "Groups": ["Default"]
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:42:00 GMT
Server: HellMapManager
Transfer-Encoding: chunked

HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:42:19 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  {
    "Key": "FastPass",
    "Command": "shortcut",
    "To": "100",
    "RoomConditions": [
      {
        "Key": "is_safe",
        "Not": false,
        "Value": 1
      }
    ],
    "Conditions": [],
    "Cost": 1,
    "Group": "Default",
    "Desc": "当房间安全时可用的快捷出口"
  }
]
```

## 批量删除快捷出口接口 RemoveShortcuts

通过给到的主键列表，批量删除快捷出口。主键为 `Key`。

**请求地址:**

/api/db/removeshortcuts

**请求正文:**

| 字段 | 类型     | 说明             |
| ---- | -------- | ---------------- |
| Keys | []string | 快捷出口主键列表 |

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/removeshortcuts HTTP/1.1

{
  "Keys": ["FastPass"]
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:45:00 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 插入变量接口 InsertVariables

批量插入变量。如果变量已经存在，会进行替换。

**请求地址:**

/api/db/insertvariables


**请求正文:**

| 字段              | 类型     | 说明     |
| ----------------- | -------- | -------- |
| Variables         | []object | 变量列表 |
| --Variables.Key   | string   | 变量主键 |
| --Variables.Value | string   | 变量值   |
| --Variables.Group | string   | 变量分组 |
| --Variables.Desc  | string   | 变量描述 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/insertvariables HTTP/1.1

{
  "Variables":[
    {
      "Key":"greeting",
      "Value":"hello",
      "Group":"common",
      "Desc":"问候语"
    }
  ]
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:53:20 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 列出变量接口 ListVariables

根据给到的 `ListOption`，列出变量列表。

**请求地址:**

/api/db/listvariables


**请求正文:**

| 字段 | 类型       | 说明     |
| ---- | ---------- | -------- |
|      | ListOption | 过滤选项 |

**返回结果：**

| 字段     | 类型     | 说明     |
| -------- | -------- | -------- |
|          | []object | 变量列表 |
| --.Key   | string   | 变量主键 |
| --.Value | string   | 变量值   |
| --.Group | string   | 变量分组 |
| --.Desc  | string   | 变量描述 |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/listvariables HTTP/1.1

{
  "Groups":["common"]
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:52:55 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  {
    "Key": "greeting",
    "Value": "hello",
    "Group": "common",
    "Desc": "问候语"
  }
]
```

## 批量删除变量接口 RemoveVariables

通过给到的主键，批量删除变量。如果主键不存在，则跳过该主键继续删除。

**请求地址:**

/api/db/removevariables


**请求正文:**

| 字段 | 类型     | 说明         |
| ---- | -------- | ------------ |
| Keys | []string | 变量主键列表 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/removevariables HTTP/1.1

{
  "Keys": ["greeting"]
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:53:32 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```


## 插入快照接口 InsertSnapshots

批量插入快照。如果快照已经存在（Key+Type+Value），则会替换。

**请求地址:**

/api/db/insertsnapshots

**请求正文:**

| 字段         | 类型     | 说明        |
| ------------ | -------- | ----------- |
| Snapshots    | []object | 快照列表    |
| --.Key       | string   | 快照主键    |
| --.Timestamp | int      | Unix 时间戳 |
| --.Group     | string   | 分组        |
| --.Type      | string   | 类型        |
| --.Count     | int      | 计数        |
| --.Value     | string   | 值          |

**返回结果：**

"success"

**示例请求:**

```http
POST http://127.0.0.1:8466/api/db/insertsnapshots HTTP/1.1

{
  "Snapshots":[
    {
      "Key":"health",
      "Timestamp":1700000000,
      "Group":"metrics",
      "Type":"status",
      "Count":1,
      "Value":"ok"
    }
  ]
}
```
**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 06:53:32 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 列出快照接口 ListSnapshots

根据给到的 `ListOption` 过滤并列出快照。

**请求地址:**

/api/db/listsnapshots

**请求正文:**

| 字段 | 类型       | 说明     |
| ---- | ---------- | -------- |
|      | ListOption | 过滤选项 |

**返回结果：**

| 字段         | 类型     | 说明        |
| ------------ | -------- | ----------- |
|              | []object | 快照列表    |
| --.Key       | string   | 快照主键    |
| --.Timestamp | int      | Unix 时间戳 |
| --.Group     | string   | 分组        |
| --.Type      | string   | 类型        |
| --.Count     | int      | 计数        |
| --.Value     | string   | 值          |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/listsnapshots HTTP/1.1

{
  "Groups":["metrics"]
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 07:12:25 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  {
    "Key": "health",
    "Timestamp": 1700000000,
    "Group": "metrics",
    "Type": "status",
    "Count": 1,
    "Value": "ok"
  }
]
```

## 批量删除快照接口 RemoveSnapshots

通过 `Key+Type+Value` 列表批量删除快照。不存在的键将被跳过。

**请求地址:**

/api/db/removesnapshots

**请求正文:**

| 字段     | 类型     | 说明                           |
| -------- | -------- | ------------------------------ |
| Keys     | []object | 快照主键列表（Key/Type/Value） |
| --.Key   | string   | 快照主键                       |
| --.Type  | string   | 类型                           |
| --.Value | string   | 值                             |

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/removesnapshots HTTP/1.1

{
  "Keys":[
    { "Key":"health", "Type":"status", "Value":"ok" }
  ]
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Dec 2025 07:13:04 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```