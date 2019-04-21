using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using WpfResumeBrowsingSystem.Domain.Models;

namespace WpfResumeBrowsingSystem.WebTests
{
    [TestClass]
    public class TestDataControl
    {
        private HttpClient _client;

        public TestDataControl()
        {
            this._client = new HttpClient();
        }

        #region    测试开发，本地，服务器 DataControl
        [TestMethod]
        public void TestProgarmDataControl()
        {
            TestDataControlTableName("http://localhost:56706/api/Data/0");
            TestDataControlTableDataAll("http://localhost:56706/api/Data/1?tbname=Staffs");
            TestDataControlTableDataSql("http://localhost:56706/api/Data/1?tbName=Staffs&sql=select%20*%20from%20Staffs%3B");
        }
        [TestMethod]
        public void TestLocalHostDataControl()
        {
            TestDataControlTableName("http://localhost:5000/api/Data/0");
            TestDataControlTableDataAll("http://localhost:5000/api/Data/1?tbname=Staffs");
            TestDataControlTableDataSql("http://localhost:5000/api/Data/1?tbName=Staffs&sql=select%20*%20from%20Staffs%3B");
        }
        [TestMethod]
        public void TestServerDataControl()
        {
            TestDataControlTableName("http://47.94.162.230:80/api/Data/0");
            TestDataControlTableDataAll("http://47.94.162.230:80/api/Data/1?tbname=Staffs");
            TestDataControlTableDataSql("http://47.94.162.230:80/api/Data/1?tbName=Staffs&sql=select%20*%20from%20Staffs%3B");
        }

        /// <summary>
        /// 测试返回全部表名
        /// </summary>
        /// <param name="url">请求url</param>
        private void TestDataControlTableName(string url)
        {
            //返回全表名
            string tableNameAll = this._client.GetStringAsync(url).Result;
            List<string> tableNames = JsonConvert.DeserializeObject<List<string>>(tableNameAll);
            List<string> propertyNames = new List<PropertyInfo>(typeof(ResumeBrowingSystemV00Context)
                .GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly))
                .ConvertAll<string>(p => p.Name);
            Func<IList<string>, IList<string>, bool> EqualAll = (srcList, dstList) =>
            {
                int i;
                for (i = 0; i < dstList.Count; i++)
                    if (!srcList.Contains(dstList[i])) break;
                return i == dstList.Count;
            };
            Assert.IsTrue(EqualAll(tableNames, propertyNames));
        }

        /// <summary>
        /// 测试返回一个表的全部数据
        /// </summary>
        /// <param name="url">请求url</param>
        private void TestDataControlTableDataAll(string url)
        {
            //返回全表，不加sql
            string tableString = this._client.GetStringAsync(url).Result;
            List<Staffs> table = JsonConvert.DeserializeObject<List<Staffs>>(tableString);
            Assert.IsTrue(table.Count > 0);
        }

        /// <summary>
        /// 测试用sql语句查询表
        /// </summary>
        /// <param name="url">请求url</param>
        private void TestDataControlTableDataSql(string url)
        {
            //返表，加sql
            string tableStringSql = this._client.GetStringAsync(url).Result;
            List<Staffs> tableSql = JsonConvert.DeserializeObject<List<Staffs>>(tableStringSql);
            Assert.IsTrue(tableSql.Count > 0);
        }

        #endregion
    }
}
