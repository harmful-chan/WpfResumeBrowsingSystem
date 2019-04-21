using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WpfResumeBrowsingSystem.Domain.Models;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;

namespace WpfResumeBrowsingSystem.WebTests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly HttpClient _client;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UnitTest1()
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
            TestDataControlTableName("http://192.168.137.222:5000/api/Data/0");
            TestDataControlTableDataAll("http://192.168.137.222:5000/api/Data/1?abname=Staffs");
            TestDataControlTableDataSql("http://192.168.137.222:5000/api/Data/1?tbName=Experiences&sql=select%20*%20from%20Staffs%3B");
        }
        [TestMethod]
        public void TestServerDataControl()
        {
            TestDataControlTableName("http://47.94.162.230:5000/api/Data/0");
            TestDataControlTableDataAll("http://47.94.162.230:5000/api/Data/1?abname=Staffs");
            TestDataControlTableDataSql("http://47.94.162.230:5000/api/Data/1?tbName=Experiences&sql=select%20*%20from%20Staffs%3B");
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

        #region    测试开发，本地，服务器FilesControl

        [TestMethod]
        public void TestProgarmFilesControl()
        {
            string filePath = Path.Combine("E:\\C#\\ResumeBrowsingSystem\\WpfResumeBrowsingSystem\\WpfResumeBrowsingSystem.WebTests",
                "Resources", "update_test.jpg");
            //PSOT /api/Files 上传图片
            string companyKey = TestUpdateFile(filePath, "http://localhost:56706/api/Files");
            //GET /api/Files/0?filename=update_test 查询文件，无后缀
            TestSelectFile(companyKey, "http://localhost:56706/api/Files/0?filename=update_test");
            //GET /api/Files/1?filename=update_test.jpg 查询文件，有后缀
            TestSelectFile(companyKey, "http://localhost:56706/api/Files/1?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test.jpg 下载图片不包含时间信息
            TestDownloadFile(filePath, "http://localhost:56706/api/Files/download?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test_XXX.jpg 直接下载图片含时间信息
            TestDownloadFile(filePath, $"http://localhost:56706/api/Files/download?filename={companyKey}");

        }
        [TestMethod]
        public void TestLocalhostFilesControl()
        {
            string filePath = Path.Combine("E:\\C#\\ResumeBrowsingSystem\\WpfResumeBrowsingSystem\\WpfResumeBrowsingSystem.WebTests",
                "Resources", "update_test.jpg");
            //PSOT /api/Files 上传图片
            string companyKey = TestUpdateFile(filePath, "http://192.168.137.222:5000/api/Files");
            //GET /api/Files/0?filename=update_test 查询文件，无后缀
            TestSelectFile(companyKey, "http://192.168.137.222/api/Files/0?filename=update_test");
            //GET /api/Files/1?filename=update_test.jpg 查询文件，有后缀
            TestSelectFile(companyKey, "http://192.168.137.222/api/Files/1?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test.jpg 下载图片不包含时间信息
            TestDownloadFile(filePath, "http://192.168.137.222/api/Files/download?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test_XXX.jpg 直接下载图片含时间信息
            TestDownloadFile(filePath, $"http://192.168.137.222/api/Files/download?filename={companyKey}");

        }
        [TestMethod]
        public void TestServerFilesControl()
        {
            string filePath = Path.Combine("E:\\C#\\ResumeBrowsingSystem\\WpfResumeBrowsingSystem\\WpfResumeBrowsingSystem.WebTests",
                "Resources", "update_test.jpg");
            //PSOT /api/Files 上传图片
            string companyKey = TestUpdateFile(filePath, "http://47.94.162.230:5000/api/Files");
            //GET /api/Files/0?filename=update_test 查询文件，无后缀
            TestSelectFile(companyKey, "http://47.94.162.230:5000/api/Files/0?filename=update_test");
            //GET /api/Files/1?filename=update_test.jpg 查询文件，有后缀
            TestSelectFile(companyKey, "http://47.94.162.230:5000/api/Files/1?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test.jpg 下载图片不包含时间信息
            TestDownloadFile(filePath, "http://47.94.162.230:5000/api/Files/download?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test_XXX.jpg 直接下载图片含时间信息
            TestDownloadFile(filePath, $"http://47.94.162.230:5000/api/Files/download?filename={companyKey}");

        }

        /// <summary>
        /// PSOT /api/Files 上传图片
        /// </summary>
        /// <param name="url">请求url</param>
        /// <returns>最新文件名</returns>
        private string TestUpdateFile(string updateFilePath, string url)
        {

            string response = UpdateFile(updateFilePath, url).Result;
            List<KeyValuePair<string, bool>> responseFileNameAndStatu = JsonConvert.DeserializeObject<List<KeyValuePair<string, bool>>>(response);
            KeyValuePair<string, bool> responseKeyValue = responseFileNameAndStatu.Find(kv => kv.Key.Contains("update_test"));
            string responseKey = responseKeyValue.Key;

            Assert.IsFalse(string.IsNullOrEmpty(responseKey));
            Assert.IsTrue(responseKeyValue.Value);
            return responseKey;
        }
        /// <summary>
        /// GET /api/Files/id 查询文件
        /// </summary>
        /// <param name="companyKey">最新文件名，用于比较文件</param>
        /// <param name="url">请求url</param>
        private void TestSelectFile(string companyKey, string url)
        {
            string responseAll = this._client.GetStringAsync(url).Result;
            List<string> responseFileall = JsonConvert.DeserializeObject<List<string>>(responseAll);
            Assert.IsTrue(responseFileall.Contains(companyKey));
        }
        /// <summary>
        /// GET /api/Files/download 下载文件
        /// </summary>
        /// <param name="companyFilePath">比较文件路径</param>
        /// <param name="url">请求url</param>
        private void TestDownloadFile(string companyFilePath, string url)
        {
            Stream responseStream = null;
            Stream fileStream = null;
            try
            {
                HttpResponseMessage responseMessage = this._client.GetAsync(url).Result;
                responseStream = responseMessage.Content.ReadAsStreamAsync().Result;
                fileStream = File.OpenRead(companyFilePath);
                Assert.AreEqual(responseStream.Length, fileStream.Length);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (responseStream != null) responseStream.Close();
                if (fileStream != null) fileStream.Close();
            }
        }     
        /// <summary>
        /// 发送本地文件
        /// </summary>
        /// <param name="url">POST 请求URL</param>
        /// <returns>相应体数据</returns>
        private async Task<string> UpdateFile(string filePath, string url)
        {
            //测试图片是否存在
            if ( !File.Exists(filePath)) Assert.Fail("File ./Resources/update_test.jpg Not Found");

            //打开本地图片，构建请求体，发送一个图片文件
            FileInfo fileInfo = new FileInfo(filePath);
            FileStream fileStream = File.OpenRead(filePath);
            MultipartFormDataContent requsetContent = new MultipartFormDataContent("----WebKitFormBoundary7MA4YWxkTrZu0gW");
            requsetContent.Add(new StreamContent(fileStream, (int)fileStream.Length), fileInfo.Name.Split('.')[0], fileInfo.Name);           
            try
            {   
                //发post
                HttpResponseMessage response = await this._client.PostAsync(url, requsetContent);
                return await response.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                fileStream.Close();
            }
        }

        #endregion
    }
}
