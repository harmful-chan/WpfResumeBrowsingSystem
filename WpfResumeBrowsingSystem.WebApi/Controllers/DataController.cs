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
using System.Collections;

namespace WpfResumeBrowsingSystem.WebApi.Controllers
{
    /// <summary>
    /// 数据库访问控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        /// <summary>
        /// DbContent内公共属性信息，只包含当前类
        /// </summary>
        public List<PropertyInfo> DbContentPropertyInfos { get; }

        /// <summary>
        /// DbContent内公共属性Name，只包含当前类
        /// </summary>
        public List<string> DbContentPropertyNames { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataController()
        {
            DbContentPropertyInfos = new List<PropertyInfo>(
                typeof(ResumeBrowingSystemV00Context).GetProperties(
                    BindingFlags.Instance |    //包含实例成员 
                    BindingFlags.Static |    //静态成员
                    BindingFlags.Public |    //公共成员
                    BindingFlags.DeclaredOnly    //不包含继承成员
            ));
            DbContentPropertyNames = DbContentPropertyInfos.ConvertAll<string>(t => t.Name);
        }

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

            switch (id)
            {
                case 0:
                    {
                        
                        return Ok(DbContentPropertyNames);
                    }
                    break;
                default:
                    {
                        //检查表名是否存在
                        if (!DbContentPropertyInfos.Exists(p => p.Name == tbName)) return NotFound("Error:Table Name Not Found");

                        //返表 Json数据
                        using (ResumeBrowingSystemV00Context db = new ResumeBrowingSystemV00Context())
                        {
                            IList resultList;
                                
                            if ("Staffs" == tbName) resultList = ExecuteSql<Staffs, ResumeBrowingSystemV00Context>(db, sql);
                            else if ("Experiences" == tbName) resultList = ExecuteSql<Experiences, ResumeBrowingSystemV00Context>(db, sql);
                            else resultList = null;


                            if (resultList != null) return Ok(resultList);
                            else return NotFound("Error:Sql Fail");
                        }
                    }
                    break;
            }

        }

        /// <summary>
        /// 执行Sql语句,包含sql合法判断
        /// </summary>
        /// <typeparam name="T">表类型</typeparam>
        /// <typeparam name="V">数据上下文类型</typeparam>
        /// <param name="db">数据上下文对象</param>
        /// <param name="sql">sql语句</param>
        /// <returns>结果列表，
        ///     null:sql语句错误
        ///     (other):数据结果
        /// </returns>
        public List<T> ExecuteSql<T, V>(DbContext db, string sql = null) 
            where V : DbContext
            where T : class
        {

            List<T> resultList = null;
            try
            {
                DbSet<T> resultDbSet = (DbSet<T>)(typeof(V).GetProperty(typeof(T).Name).GetValue(db));    //获取数据上下文内对饮的DbSet
                if (null == sql) resultList = resultDbSet.ToList();
                else resultList = resultDbSet.FromSql<T>(sql).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return resultList;

        }
    }
}