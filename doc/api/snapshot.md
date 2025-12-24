# 快照相关接口

快照指对房间的某些环境进行存档。一般用在会变化的地方，比如环境描述，房间npc,小地图等等。

快照搜索能在相关快照中找出有用的信息，并按房间分组用来判断。

## 抓快照接口 TakeSnapshot

抓快照接口用于把当前的房间制定内容的快照加入系统中。

快照如果重复，不会创建新快照，会把快照的Count加1,然后更新快照时间。

**请求地址:**

/api/db/takesnapshot

**请求正文:**

| 字段  | 类型   | 说明     |
| ----- | ------ | -------- |
| Key   | string | 房间主键 |
| Type  | string | 快照类型 |
| Value | string | 快照值   |
| Group | string | 快照分组 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/takesnapshot HTTP/1.1

{
    "Key" :"0",
    "Type" :"desc",
    "Value":"center",
    "Group":"扬州"
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sun, 14 Dec 2025 14:47:28 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 搜索快照 SearchSnapshots

根据给到的限制搜索快照

**请求地址:**

/api/db/searchsnapshot

**请求正文:**

| 字段         | 类型      | 说明                     |
| ------------ | --------- | ------------------------ |
| Type         | string?   | 限制快照类型，留空不限制 |
| Group        | string?   | 限制快照分组，留空不限制 |
| Keywords     | []string? | 搜索关键字，留空不限制   |
| PartialMatch | bool      | 部分匹配，默认为true     |
| Any          | bool      | 任意满足，默认为false    |

* Type 限制只搜索特定类型的快照
* Group 限制只搜索特定分组的快照
* Keywords, 关键字列表，具体效果和下面两个参数相关,为空返回与Any相反的结果。
* PartialMatch 部分匹配，默认表现为只要关键字出现在快照内容内就匹配。为false则必须完整匹配
* Any 任意满足一般用在完整匹配时，任何一个关键字匹配则快照匹配

**返回结果：**

| 字段               | 类型     | 说明               |
| ------------------ | -------- | ------------------ |
| .                  | []object | 搜索结果列表       |
| --.Key             | string   | 房间Key            |
| --.Sum             | int      | 房间快照总计       |
| --.Count           | int      | 匹配的快照总计     |
| --.Items           | []object | 房间的匹配快照列表 |
| --.Items.Key       | string   | 快照的房间主键     |
| --.Items.Timestamp | int      | 快照更新时间戳     |
| --.Items.Group     | string   | 快照的房间分组     |
| --.Items.Type      | string   | 快照的房间类型     |
| --.Items.Count     | int      | 快照的重复次数     |
| --.Items.Value     | string   | 快照值             |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/searchsnapshots HTTP/1.1

{
    "Type":"desc",
    "Keywords":["ce","er"],
    "PartialMatch":true,
    "Any":false
}
```

**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sun, 14 Dec 2025 15:12:13 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  {
    "Key": "0",
    "Sum": 2,
    "Count": 2,
    "Items": [
      {
        "Key": "0",
        "Timestamp": 1765723648,
        "Group": "扬州",
        "Type": "desc",
        "Count": 2,
        "Value": "center"
      }
    ]
  }
]
```

## 清除快照 ClearSnapshots

批量清除快照

**请求地址:**

/api/db/clearsnapshots

**请求正文:**

| 字段     | 类型    | 说明        |
| -------- | ------- | ----------- |
| Key      | string? | 房间主键    |
| Type     | string? | 快照类型    |
| Group    | string? | 快照分组    |
| MaxCount | number  | 最大Count数 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/clearsnapshots HTTP/1.1

{
    "Type":"desc"
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sun, 14 Dec 2025 15:18:48 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```