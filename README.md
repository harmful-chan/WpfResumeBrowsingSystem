## <center>WpfResumeBrowsingSystem</center>
---
+ 开发平台：WIN10 64
+ 数据库：Mysql
+ 服务器：Ubunttu16.04 64

|程序集|平台|特性|详细说明|Nuget包|
|:-:|:-:|:-:|:-:|:-:|
|.|.net 4.5.2|Wpf|主启动项目||
|.DBL|.net 4.5.2|ADO .Net|底层数据库连接|MySql.Data|
|.Test|.net 4.5.2||控制台程序，调试|Newtonsoft.Json|
|.Domain|.net core 2.1|EF Core DBfirst|web应用数据库连接|MySql.Data.EntityFrameworkCore|
|.WebApi|.net core 2.1|Web Api|服务器数据提取，请求处理|

<br>

## <center>WebApi</center>
---
#### 服务器文件目录
```
wwwroot    目录暂时没用
│   css
│   images    
│   js
StaticFiles     存放静态文件
│   images    图片文件
...
│└─
```

#### 获取数据库数据
  + 例：http://<server_address>/api/data?tbname=Staffs
  + 下载表数据
    + GET:http://<server_address>/api/data
      + parameter:tbname=Stadds/Experiences

#### 上传下载图片文件
  + 下载图片
    + GET:http://<server_address>/StaticFiles/images/XXX
  + 上传图片
    + POST:http://<server_address>/api/files

