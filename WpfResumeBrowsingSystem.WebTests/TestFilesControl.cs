using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.IO;

namespace WpfResumeBrowsingSystem.WebTests
{
    [TestClass]
    public class TestFilesControl
    {
        private HttpClient _client;

        public TestFilesControl()
        {
            this._client = new HttpClient();
        }

        #region    测试开发，本地，服务器FilesControl

        [TestMethod]
        public void TestProgarmFilesControl()
        {
            TestControl("http://localhost:56706/");
        }
        [TestMethod]
        public void TestLocalhostFilesControl()
        {
            TestControl("http://localhost:5000/");
        }
        [TestMethod]
        public void TestServerFilesControl()
        {
            TestControl("http://47.94.162.230:80/");
        }

        /// <summary>
        /// 执行测试程序
        /// </summary>
        /// <param name="host">发送主机url</param>
        private void TestControl(string host)
        {
            string filePath = Path.Combine("E:\\C#\\ResumeBrowsingSystem\\WpfResumeBrowsingSystem\\WpfResumeBrowsingSystem.WebTests",
                "Resources", "update_test.jpg");
            //PSOT /api/Files 上传图片
            string companyKey = TestUpdateFile(filePath, $"{host}api/Files");
            //GET /api/Files/0?filename=update_test 查询文件，无后缀
            TestSelectFile(companyKey, $"{host}api/Files/0?filename=update_test");
            //GET /api/Files/1?filename=update_test.jpg 查询文件，有后缀
            TestSelectFile(companyKey, $"{host}api/Files/1?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test.jpg 下载图片不包含时间信息
            TestDownloadFile(filePath, $"{host}api/Files/download?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test_XXX.jpg 直接下载图片含时间信息
            TestDownloadFile(filePath, $"{host}api/Files/download?filename={companyKey}");
            //GET /api/Files?filename=update_test_XXX.jpg 删除服务器文件
            TestDeleteFile(companyKey, $"{host}api/Files?filename={companyKey}");

        }

        /// <summary>
        /// 发送本地文件
        /// </summary>
        /// <param name="url">POST 请求URL</param>
        /// <returns>相应体数据</returns>
        private async Task<string> UpdateFile(string filePath, string url)
        {
            //测试图片是否存在
            if (!File.Exists(filePath)) Assert.Fail("File ./Resources/update_test.jpg Not Found");

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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fileStream.Close();
            }
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
        /// DETELE /api/File 删除文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="url">请求url</param>
        private void TestDeleteFile(string fileName, string url)
        {
            HttpResponseMessage msg = this._client.DeleteAsync(url).Result;
            string s = msg.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(s.Contains(fileName) & s.Contains("Success"));
        }

        #endregion
    }
}
