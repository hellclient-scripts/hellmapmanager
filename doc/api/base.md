# 接口请求基础

## 请求协议
HellMapManage(HMM)的请求标准如下
* 使用JSON格式作为BODY正文
* 默认使用POST方式传输数据，如无数据，则POST/GET方式都可以使用
* 成功会返回状态码200的相应，正文为JSON格式的数据
* 如果请求时带有charset: gb18030的请求头，则请求正文会以gb18030解码，返回的相应会议gb18030编码

如果发生意外，则有会返回非200状态码的数据

## 操作成功相应

无数据的成功请求，会返回一个内容为字符串success的JSON格式的相应

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Fri, 12 Dec 2025 12:44:15 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```
## 认证失败错误

如果在HMM的接口设置中设置了用户名和密码，那么需要以Http Basic Authentication的方式进行验证。

Http Basic Authentication最简单的方式就是在请求的URL中加入认证信息，如

```
http://USERNAME:USERPASS@localhost:8466/api/db/info
```

认证失败，就会返回401错误

```
HTTP/1.1 401 Unauthorized
Connection: close
Date: Fri, 12 Dec 2025 12:38:56 GMT
Server: HellMapManager
Transfer-Encoding: chunked

Unauthorized
```

## 数据解析错误

如果把错误格式或者结构的数据传输给接口，会获得一个400错误

```
HTTP/1.1 400 Bad Request
Connection: close
Date: Fri, 12 Dec 2025 12:40:51 GMT
Server: HellMapManager
Transfer-Encoding: chunked

Invalid JSON
```

## 请求接口地址错误

如果请求的地址或者请求的方式不正确(建议全使用POST请求),会得到一个404错误

```
HTTP/1.1 404 Not Found
Connection: close
Date: Fri, 12 Dec 2025 12:42:53 GMT
Server: HellMapManager
Transfer-Encoding: chunked

Not Found
```