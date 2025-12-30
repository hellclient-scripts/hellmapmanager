# 无头浏览器控制接口

无头浏览器控制接口智能在命令行使用了 -c 参数,并制定hmm文件后使用，用于在没有图形界面的场景下时对程序进行简单的控制。

由于Windows系统的特殊性，Windows下需要使用带Console的可执行程序，才能出现终端窗口进行文字反馈。

## 保存HMM接口 Save

将当前的数据保存到打开的hmm文件位置


**请求地址:**

/api/headless/save

**请求正文:**

任意

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/headless/save HTTP/1.1

{}
```
**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Tue, 30 Dec 2025 05:05:12 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 加载HMM接口 Save

放弃当前数据，重新加载hmm文件


**请求地址:**

/api/headless/load

**请求正文:**

任意

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/headless/load HTTP/1.1

{}
```
**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Tue, 30 Dec 2025 05:05:54 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```

## 推出程序 Quit

结束任务，退出程序


**请求地址:**

/api/headless/quit

**请求正文:**

任意

**返回结果：**

"success"

**示例请求**

```http
POST http://127.0.0.1:8466/api/headless/quit HTTP/1.1

{}
```
**示例结果:**
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Tue, 30 Dec 2025 05:06:47 GMT
Server: HellMapManager
Transfer-Encoding: chunked

"success"
```