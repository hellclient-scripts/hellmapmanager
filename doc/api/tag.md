# 标记接口

标记接口是一系列特殊操作接口。

他们的作用实在机器运行时，根据房间的特殊情况对房间或相关元素打上标记。

特点是，可以重复执行，无返回结果。

## 房间分组接口 GroupRoom

设置制定房间的分组

**请求地址:**

/api/db/grouproom

**请求正文:**

| 字段  | 类型   | 说明     |
| ----- | ------ | -------- |
| Key   | string | 房间主键 |
| Group | string | 房间分组 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/grouproom HTTP/1.1

{
    "Key":"0",
    "Group":"扬州"
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 16:42:18 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 设置房间标签接口 TagRoom

设置制定房间的分组

**请求地址:**

/api/db/tagroom

**请求正文:**

| 字段  | 类型   | 说明                 |
| ----- | ------ | -------------------- |
| Room  | string | 房间主键             |
| Tag   | string | 标签主键             |
| Value | int    | 标签值,为0会删除标签 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/tagroom HTTP/1.1

{
    "Room":"0",
    "Tag":"outdoor",
    "Value":1
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 16:45:05 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 设置房间数据接口 SetRoomData

设置制定房间的数据。

可以用来记录一些不会变化的数据，比如带色彩的房间名之类。

理论上，房间的非核心数据更适合使用快照的方式处理。

**请求地址:**

/api/db/setroomdata

**请求正文:**

| 字段  | 类型   | 说明                  |
| ----- | ------ | --------------------- |
| Room  | string | 房间主键              |
| Key   | string | 数据主键              |
| Value | string | 数据值,为空会删除数据 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/setroomdata HTTP/1.1

{
    "Room":"0",
    "Key":"Title",
    "Value":"$HIW中央广场"
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 16:45:05 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 设置追踪 TraceLocation

将房间加入足迹中。

一般用于在特定对象出现后对足迹进行维护。

**请求地址:**

/api/db/tracelocation

**请求正文:**

| 字段     | 类型   | 说明     |
| -------- | ------ | -------- |
| Key      | string | 足迹主键 |
| Location | string | 房间主键 |


**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/tracelocation HTTP/1.1

{
    "Key":"mytrace",
    "Location":"0"
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 18:22:51 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```
