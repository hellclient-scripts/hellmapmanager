# 其他接口

## 获取地图变量

获取地图指定变量

一般用于获取和地图相关的信息

**请求地址:**

/api/db/getvariable

**请求正文:**

| 字段 | 类型   | 说明                           |
| ---- | ------ | ------------------------------ |
|      | string | 变量名。未设置变量返回空字符串 |
**返回结果：**


| 字段 | 类型   | 说明   |
| ---- | ------ | ------ |
|      | string | 变量值 |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/getvariable HTTP/1.1

{
  "Key":"MyVariable"
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 18:26:48 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"mydata"
```

## 获取地区对应房间

获取地区对应的房间信息。

因为要进行依次计算，一般应该对获取的数据进行缓存。


**请求地址:**

/api/db/queryregionrooms

**请求正文:**

| 字段 | 类型   | 说明                         |
| ---- | ------ | ---------------------------- |
|      | string | 区域名。未设置区域返回空数组 |
**返回结果：**


| 字段 | 类型     | 说明     |
| ---- | -------- | -------- |
|      | []string | 房间列表 |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/queryregionrooms HTTP/1.1

{
  "Key":"myregion"
}
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sat, 13 Dec 2025 06:28:56 GMT
Server: HellMapManager
Transfer-Encoding: chunked

[
  "1",
  "2220"
]
```