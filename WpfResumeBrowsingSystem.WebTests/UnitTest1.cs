using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WpfResumeBrowsingSystem.Domain.Models;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Http.Headers;

namespace WpfResumeBrowsingSystem.WebTests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly HttpClient _client;


        public UnitTest1()
        {
            this._client = new HttpClient();
        }

        #region 本地，服务器数据库连接测试
        [TestMethod]
        public void TestLocalHostDataControl()
        {
            Task<int> tmp1 = GetTableCount<Staffs>(@"http://localhost:5000/api/data?tbname=Staffs");
            Task<int> tmp2 = GetTableCount<Experiences>(@"http://localhost:5000/api/data?tbname=Experiences");
            Assert.IsTrue(tmp1.Result > 1);
            Assert.IsTrue(tmp2.Result > 1);
        }
        [TestMethod]
        public void TestServerDataControl()
        {
            Task<int> tmp1 = GetTableCount<Staffs>(@"http://47.94.162.230:5000/api/data?tbname=Staffs");
            Task<int> tmp2 = GetTableCount<Experiences>(@"http://47.94.162.230:5000/api/data?tbname=Experiences");
            Assert.IsTrue(tmp1.Result > 1);
            Assert.IsTrue(tmp2.Result > 1);
        }
        private async Task<int> GetTableCount<T>(string url)
        {
            string result = await this._client.GetStringAsync(url);
            List<T> list = JsonConvert.DeserializeObject<List<T>>(result);

            return list.Count;
        }


        #endregion

        [TestMethod]
        public void TestLocalHostFilesControl()
        {
            string tmp1 = UpdateFile(@"http://localhost:56706/api/files").Result;
            Assert.IsTrue(tmp1.IndexOf("post file success") >= 0);
        }
        private async Task<string> UpdateFile(string url)
        {
            //测试图片是否存在
            string filePath = Path.Combine(
                "E:\\C#\\ResumeBrowsingSystem\\WpfResumeBrowsingSystem\\WpfResumeBrowsingSystem.WebTests", 
                "Resources", "update_test.jpg");
            if ( !File.Exists(filePath)) Assert.Fail("File ./Resources/update_test.jpg Not Found");

            //文件操作
            FileInfo fileInfo = new FileInfo(filePath);
            FileStream fileStream = File.OpenRead(filePath);

            //建立请求内容
            MultipartFormDataContent requsetContent = new MultipartFormDataContent("----WebKitFormBoundary7MA4YWxkTrZu0gW");
            requsetContent.Add(new StreamContent(fileStream, (int)fileStream.Length), fileInfo.Name.Split('.')[0], fileInfo.Name);

            //发post
            HttpResponseMessage response = await this._client.PostAsync(url, requsetContent);

            try
            {
                return await response.Content.ReadAsStringAsync();
            }
            finally
            {
                fileStream.Close();
                _client.Dispose();
            }
        }

        [TestMethod]
        public void Test()
        {
            DateTime d1 = new DateTime(2018, 11, 10, 18, 05, 00);
            DateTime d2 = new DateTime(2018, 11, 10, 18, 06, 00);

            int x = DateTime.Compare(d2, d1);
        }
    }
}
