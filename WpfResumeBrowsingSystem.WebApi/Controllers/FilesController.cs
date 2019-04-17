using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
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
    [Consumes("multipart/form-data")]//此处为新增
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public FilesController(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        //POST api/files
        /// <summary>
        /// 上传文件接口，获取POST请求的内容文件，filename+当前时间+.jpg保存进images文件夹中
        /// </summary>
        /// <param name="form">请求体</param>
        /// <returns>最后更新文件名</returns>
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
                        file.Name + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file.FileName.Split('.').Last());
                    updateFileName.Add(currentFileName);
                    using (FileStream fs = System.IO.File.Create(currentFileName))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                }
                finally
                {

                }
            }

            //写入文件判断
            List<KeyValuePair<string, bool>> fileStat = updateFileName.ConvertAll<KeyValuePair<string, bool> >(s => 
            {
                return new KeyValuePair<string, bool>(s, System.IO.File.Exists(s));
            });

            return Ok(fileStat);
            
        }

        //GET api/files?filename
        [HttpGet("{id}")]
        public IActionResult Get(int id, string fileName)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(this._hostingEnvironment.WebRootPath, "images"));
            List<FileInfo> result = FindFile(dir, fileName);

            if (0 == id) return Ok(result.ConvertAll<string>(x => x.Name));
            else if (1 == id)
            {
                List<DateTime> fileUpdateDate =    //更新时间
                    result.ConvertAll<string>(x => x.Name)
                    .ConvertAll<DateTime>(s => {
                        string[] tmp = s.Split('.', '_');
                        return DateTime.ParseExact(
                            tmp[tmp.Length - 2],
                            "yyyyMMddhhmmss",
                            System.Globalization.CultureInfo.CurrentCulture);
                    });
                Func<List<DateTime>, int> FindMaxDateIndex = x => {
                    int maxIndex = -1;
                    for (int i = 0; i < x.Count - 1; i++)
                        for (int j = i; j < x.Count - 1; j++)
                            maxIndex = (DateTime.Compare(x[i], x[i + 1]) > 0) ? i : i + 1;
                    return maxIndex;
                };
                int index = FindMaxDateIndex(fileUpdateDate);
                using (FileStream fs = System.IO.File.OpenRead(result[index].FullName))
                {
                    string fileExt = Path.GetExtension(result[index].FullName);
                    var provider = new FileExtensionContentTypeProvider();
                    var memi = provider.Mappings[fileExt];
                    return File(fs, memi, Path.GetFileName(result[index].FullName));
                }
            }
            else
            {
                return NotFound();
            }
        }

        //查找文件
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
    }
}