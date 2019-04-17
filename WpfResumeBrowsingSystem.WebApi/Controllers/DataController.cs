using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Newtonsoft.Json;
using WpfResumeBrowsingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace WpfResumeBrowsingSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        /// <summary>
        /// 带参数GET请求，获取数据库表数据
        /// </summary>
        /// <param name="id">主键
        ///     0:数据库全部表名(tbName当前无效)
        ///     default:其余参数有效
        /// </param>
        /// <param name="tbName">表名</param>
        /// <param name="sql">sql语句
        ///     当设置该属性时，使用该语句查找表中数据
        /// </param>
        /// <returns>结果列表json数据</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id, string tbName, string sql = null)
        {
            Assembly targerAssembly = Assembly.Load("WpfResumeBrowsingSystem.Domain");
            
            switch (id)
            {
                case 0:
                    {
                        List<Type> types = new List<Type>(targerAssembly.GetTypes());
                        List<string> names = types.ConvertAll<string>( t => t.Name );
                        return Ok(names);
                    }
                    break;
                default:
                    {
                        //检查表名是否存在
                        if (!targerAssembly.GetType("WpfResumeBrowsingSystem.Domain.Models." + tbName).IsClass)
                        {
                            return NotFound("Error:Table Name Not Found");
                        }

                        //返表 Json数据
                        using (ResumeBrowingSystemV00Context db = new ResumeBrowingSystemV00Context())
                        {
                            //db.Set<Staffs>().FromSql($"select * from {nameof(Person)} where {nameof(name)}=@{nameof(name)} ");
                            if ("Staffs" == tbName) return Ok();


                            return Ok();

                        }
                    }
                    break;
            }
            
        }
    }
}