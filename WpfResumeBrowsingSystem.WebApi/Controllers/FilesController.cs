using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace WpfResumeBrowsingSystem.WebApi.Controllers
{
    /// <summary>
    /// 文件上传下载控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 构造函数初始化属性IHostingEnvironment
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public FilesController(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }


        /// <summary>
        /// POST请求 上传文件接口 保存文件名为:filename+当前时间单位ms+.jpg 路径/wwwroot/images
        /// </summary>
        /// <param name="form">请求表单</param>
        /// <returns>更新文件表单
        ///     文件：状态
        /// </returns>
        //POST api/files
        [HttpPost]
        public IActionResult Post(IFormCollection form)
        {
            //string topDirectory = this._hostingEnvironment.ContentRootPath;    //顶级目录
            string wwwDirectory = this._hostingEnvironment.WebRootPath;    //wwwroot目录
            List<string> updateFileName = new List<string>();
            
            FormFileCollection formFiles = form.Files as FormFileCollection;

            //处理表单文件
            foreach (FormFile file in formFiles)
            {
                try
                {
                    //添加当时间进文件名,保存文件
                    string currentFileName = Path.Combine(wwwDirectory, "images",
                        file.Name + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + file.FileName.Split('.').Last());
                    
                    using (FileStream fs = System.IO.File.Create(currentFileName))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    updateFileName.Add(currentFileName);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //写入文件判断
            List<KeyValuePair<string, bool>> fileStat = updateFileName.ConvertAll<KeyValuePair<string, bool> >(s => 
            {
                return new KeyValuePair<string, bool>(s, System.IO.File.Exists(s));
            });

            return Ok(fileStat);
            
        }

        /// <summary>
        /// GET请求，显示文件列表
        /// </summary>
        /// <param name="id">主键
        ///     0:含有文件名的所有文件
        ///     1:含有文件名.扩展名的所有文件
        /// </param>
        /// <param name="fileName">文件名.扩展名</param>
        /// <returns>文件表单</returns>
        //GET api/files?filename
        [HttpGet]
        public IActionResult Get(int id, string fileName = null)
        {
            if (fileName == null) return NotFound("Error:FileName Is Null");

            List<FileInfo> fileInfos = FindFile(
                new DirectoryInfo(Path.Combine(this._hostingEnvironment.WebRootPath, "images")), fileName.Split('.')[0]);
            if (fileInfos.Count <= 0) return NotFound("Error:File Not Found");

            IEnumerable<FileInfo> fileExtenSames = fileInfos.Where(f=>f.Name.Contains(Path.GetExtension(fileName)));    //匹配扩展名相通的文件
            return Ok(fileExtenSames.ToList().ConvertAll<string>(f=>f.Name));    //返回所有匹配的文件名
        }

        /// <summary>
        /// GET请求，下载最新文件
        /// </summary>
        /// <param name="fileName">文件名(可不包含系统添加的时间信息).扩展名</param>
        /// <returns>文件相应</returns>
        [HttpGet("download")]
        public IActionResult Download(string fileName = null)
        {
            try
            {
                if (fileName == null) throw new ArgumentNullException();

                string basePath = Path.Combine(this._hostingEnvironment.WebRootPath, "images");
                FileInfo targetFile = null;
                
                if (!System.IO.File.Exists(Path.Combine(basePath, fileName)))   //不是直接下载文件，通过文件名索引下载最新文件
                {
                    List<FileInfo> fileInfos = FindFile(new DirectoryInfo(basePath), fileName.Split('.')[0]);    
                    if (fileInfos.Count <= 0) throw new ArgumentException();

                    //匹配扩展名相通的文件
                    List<FileInfo> fileExtenSames = fileInfos.Where(f => f.Name.Contains(Path.GetExtension(fileName))).ToList();

                    //从文件名获取文件更新时间
                    List<DateTime> fileUpdateDates = GetFileUpdateDateList(fileExtenSames);

                    //索引最新文件位置
                    Func<List<DateTime>, int> IndexOfNewest = x =>
                    {
                        int maxIndex = -1;
                        for (int i = 0; i < x.Count - 1; i++)
                            for (int j = i; j < x.Count - 1; j++)
                                maxIndex = (DateTime.Compare(x[i], x[i + 1]) > 0) ? i : i + 1;
                        return maxIndex;
                    };
                    int index = IndexOfNewest(fileUpdateDates);
                    targetFile = fileExtenSames[index];
                }
                else    //直接下载文件
                {
                    targetFile = new FileInfo(Path.Combine(basePath, fileName));
                }

                //响应
                if (System.IO.File.Exists(targetFile.FullName))
                {
                      
                    FileStream fs = System.IO.File.OpenRead(targetFile.FullName);
                    var fileExt = Path.GetExtension(targetFile.Name);
                    var memi = new FileExtensionContentTypeProvider().Mappings[fileExt];
                    return File(fs, memi, Path.GetFileName(targetFile.Name));
                }
                else throw new FileNotFoundException(targetFile.FullName);
                
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// 指定目录查找文件名含有字符片段的文件
        /// </summary>
        /// <param name="dirs">指定目录</param>
        /// <param name="name">字符片段</param>
        /// <returns>文件信息列表</returns>
        private List<FileInfo> FindFile(DirectoryInfo dirs, string name)
        {
            List<FileInfo> result = new List<FileInfo>();
            foreach (DirectoryInfo dir in dirs.GetDirectories())
            {
                result.AddRange(FindFile(dir, name));
            }
            foreach (FileInfo file in dirs.GetFiles())
            {
                if (file.Name.IndexOf(name) >= 0) result.Add(file);
            }
            return result;
        }
        
        /// <summary>
        /// 从文件名获取文件更新时间
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>时间</returns>
        private DateTime GetFileUpdateDate(FileInfo fileInfo)
        {
            string[] tmp = fileInfo.Name.Split('.', '_');
            return DateTime.ParseExact(tmp[tmp.Length - 2], "yyyyMMddhhmmss", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// GetFileUpdateDate进化版
        /// </summary>
        /// <param name="fileInfos"></param>
        /// <returns></returns>
        private List<DateTime> GetFileUpdateDateList(List<FileInfo> fileInfos)
        {
            return fileInfos.ConvertAll<DateTime>(f => GetFileUpdateDate(f));
        }
    }
}