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
#### 数据库
  + 例：http://localhost:56706/api/data?tbname=Staffs

	|请求|Url|功能|
	|-|-|
	|GET|http://localhost:56706/api/data|获取数据库数据|

	|可带参数|值|说明|
	|-|-|-|
	|tbname|Stadds/Experiences|表名，大小写敏感|

#### 服务器文件
  + 例
  
	|请求|Url|功能|
	|-|-|
	|POST|http://localhost:56706/api/file|下载服务器文件数据|
	|PUT|http://localhost:56706/api/file|更新文件|

	|可带参数|值|说明|
	|-|-|-|
	|file_url|/~/pic/pic.jpg|文件地址|


