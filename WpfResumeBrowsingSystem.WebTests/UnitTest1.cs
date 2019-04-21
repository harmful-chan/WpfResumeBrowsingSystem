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
        /// ���캯��
        /// </summary>
        public UnitTest1()
        {
            this._client = new HttpClient();
        }

        #region    ���Կ��������أ������� DataControl

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
        /// ���Է���ȫ������
        /// </summary>
        /// <param name="url">����url</param>
        private void TestDataControlTableName(string url)
        {
            //����ȫ����
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
        /// ���Է���һ�����ȫ������
        /// </summary>
        /// <param name="url">����url</param>
        private void TestDataControlTableDataAll(string url)
        {
            //����ȫ������sql
            string tableString = this._client.GetStringAsync(url).Result;
            List<Staffs> table = JsonConvert.DeserializeObject<List<Staffs>>(tableString);
            Assert.IsTrue(table.Count > 0);
        }
        /// <summary>
        /// ������sql����ѯ��
        /// </summary>
        /// <param name="url">����url</param>
        private void TestDataControlTableDataSql(string url)
        {
            //������sql
            string tableStringSql = this._client.GetStringAsync(url).Result;
            List<Staffs> tableSql = JsonConvert.DeserializeObject<List<Staffs>>(tableStringSql);
            Assert.IsTrue(tableSql.Count > 0);
        }

        #endregion

        #region    ���Կ��������أ�������FilesControl

        [TestMethod]
        public void TestProgarmFilesControl()
        {
            string filePath = Path.Combine("E:\\C#\\ResumeBrowsingSystem\\WpfResumeBrowsingSystem\\WpfResumeBrowsingSystem.WebTests",
                "Resources", "update_test.jpg");
            //PSOT /api/Files �ϴ�ͼƬ
            string companyKey = TestUpdateFile(filePath, "http://localhost:56706/api/Files");
            //GET /api/Files/0?filename=update_test ��ѯ�ļ����޺�׺
            TestSelectFile(companyKey, "http://localhost:56706/api/Files/0?filename=update_test");
            //GET /api/Files/1?filename=update_test.jpg ��ѯ�ļ����к�׺
            TestSelectFile(companyKey, "http://localhost:56706/api/Files/1?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test.jpg ����ͼƬ������ʱ����Ϣ
            TestDownloadFile(filePath, "http://localhost:56706/api/Files/download?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test_XXX.jpg ֱ������ͼƬ��ʱ����Ϣ
            TestDownloadFile(filePath, $"http://localhost:56706/api/Files/download?filename={companyKey}");

        }
        [TestMethod]
        public void TestLocalhostFilesControl()
        {
            string filePath = Path.Combine("E:\\C#\\ResumeBrowsingSystem\\WpfResumeBrowsingSystem\\WpfResumeBrowsingSystem.WebTests",
                "Resources", "update_test.jpg");
            //PSOT /api/Files �ϴ�ͼƬ
            string companyKey = TestUpdateFile(filePath, "http://192.168.137.222:5000/api/Files");
            //GET /api/Files/0?filename=update_test ��ѯ�ļ����޺�׺
            TestSelectFile(companyKey, "http://192.168.137.222/api/Files/0?filename=update_test");
            //GET /api/Files/1?filename=update_test.jpg ��ѯ�ļ����к�׺
            TestSelectFile(companyKey, "http://192.168.137.222/api/Files/1?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test.jpg ����ͼƬ������ʱ����Ϣ
            TestDownloadFile(filePath, "http://192.168.137.222/api/Files/download?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test_XXX.jpg ֱ������ͼƬ��ʱ����Ϣ
            TestDownloadFile(filePath, $"http://192.168.137.222/api/Files/download?filename={companyKey}");

        }
        [TestMethod]
        public void TestServerFilesControl()
        {
            string filePath = Path.Combine("E:\\C#\\ResumeBrowsingSystem\\WpfResumeBrowsingSystem\\WpfResumeBrowsingSystem.WebTests",
                "Resources", "update_test.jpg");
            //PSOT /api/Files �ϴ�ͼƬ
            string companyKey = TestUpdateFile(filePath, "http://47.94.162.230:5000/api/Files");
            //GET /api/Files/0?filename=update_test ��ѯ�ļ����޺�׺
            TestSelectFile(companyKey, "http://47.94.162.230:5000/api/Files/0?filename=update_test");
            //GET /api/Files/1?filename=update_test.jpg ��ѯ�ļ����к�׺
            TestSelectFile(companyKey, "http://47.94.162.230:5000/api/Files/1?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test.jpg ����ͼƬ������ʱ����Ϣ
            TestDownloadFile(filePath, "http://47.94.162.230:5000/api/Files/download?filename=update_test.jpg");
            //GET /api/Files/download?filename=update_test_XXX.jpg ֱ������ͼƬ��ʱ����Ϣ
            TestDownloadFile(filePath, $"http://47.94.162.230:5000/api/Files/download?filename={companyKey}");

        }

        /// <summary>
        /// PSOT /api/Files �ϴ�ͼƬ
        /// </summary>
        /// <param name="url">����url</param>
        /// <returns>�����ļ���</returns>
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
        /// GET /api/Files/id ��ѯ�ļ�
        /// </summary>
        /// <param name="companyKey">�����ļ��������ڱȽ��ļ�</param>
        /// <param name="url">����url</param>
        private void TestSelectFile(string companyKey, string url)
        {
            string responseAll = this._client.GetStringAsync(url).Result;
            List<string> responseFileall = JsonConvert.DeserializeObject<List<string>>(responseAll);
            Assert.IsTrue(responseFileall.Contains(companyKey));
        }
        /// <summary>
        /// GET /api/Files/download �����ļ�
        /// </summary>
        /// <param name="companyFilePath">�Ƚ��ļ�·��</param>
        /// <param name="url">����url</param>
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
        /// ���ͱ����ļ�
        /// </summary>
        /// <param name="url">POST ����URL</param>
        /// <returns>��Ӧ������</returns>
        private async Task<string> UpdateFile(string filePath, string url)
        {
            //����ͼƬ�Ƿ����
            if ( !File.Exists(filePath)) Assert.Fail("File ./Resources/update_test.jpg Not Found");

            //�򿪱���ͼƬ�����������壬����һ��ͼƬ�ļ�
            FileInfo fileInfo = new FileInfo(filePath);
            FileStream fileStream = File.OpenRead(filePath);
            MultipartFormDataContent requsetContent = new MultipartFormDataContent("----WebKitFormBoundary7MA4YWxkTrZu0gW");
            requsetContent.Add(new StreamContent(fileStream, (int)fileStream.Length), fileInfo.Name.Split('.')[0], fileInfo.Name);           
            try
            {   
                //��post
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
