# 信息接口

信息接口是HellMapManager(HMM)服务的基础信息

## 版本信息

返回HMM接口的版本信息。部分新接口可能在指定的版本之后才能使用

**请求地址:**

/api/version

**请求正文:**

无

**返回结果：**

Int 数字版本号

**示例请求**

```http
POST http://127.0.0.1:8466/api/version HTTP/1.1
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 12:58:08 GMT
Server: HellMapManager
Transfer-Encoding: chunked

1000
```

## 地图信息接口

返回当前打开地图的名称和描述。如果没有打开地图，则返回空。可用于判断HMM服务是否已经打开地图

**请求地址:**

/api/db/info

**请求正文:**

无

**返回结果：**


| 字段 | 类型   | 说明     |
| ---- | ------ | -------- |
| Name | string | 地图名称 |
| Desc | string | 地图描述 |

**示例请求**

```http
POST http://127.0.0.1:8466/api/db/info HTTP/1.1
```

**示例结果:**

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 13:01:25 GMT
Server: HellMapManager
Transfer-Encoding: chunked

{
  "Name": "mapname",
  "Desc": "mapdesc"
}
```

