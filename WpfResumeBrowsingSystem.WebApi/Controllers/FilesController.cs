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
        [HttpPost]
        public IActionResult Post(IFormCollection form)
        {
            //string topDirectory = this._hostingEnvironment.ContentRootPath;    //顶级目录
            string wwwDirectory = this._hostingEnvironment.WebRootPath;    //wwwroot目录
            string fileCurrentName = null;
            List<FileInfo> targetFiles = null;
            FormFileCollection formFiles = form.Files as FormFileCollection;

            //处理表单文件
            foreach (FormFile file in formFiles)
            {
                //搜索相同file.name 的文件信息
                targetFiles = new List<FileInfo>();
                {
                    DirectoryInfo dir = new DirectoryInfo(Path.Combine(wwwDirectory, "images"));
                    targetFiles = FindFile(dir, "update_test");
                }

                //添加当时间进文件名,保存文件
                fileCurrentName = Path.Combine(wwwDirectory, "images",
                    file.Name + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file.FileName.Split('.').Last());
                using (FileStream fs = System.IO.File.Create(fileCurrentName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }

            //结果处理
            List<string> result = new List<string>();
            {
                if (formFiles.Count > 0) result.Add("post file success, current save fileInfo list:");
                else result.Add("post file null");

                if (null != targetFiles)
                    foreach (FileInfo f in targetFiles)
                        result.Add(f.Name);
                else result.Add("current save file null");

                if (null != fileCurrentName) result.Add(fileCurrentName);
                else result.Add("current save file null");
            }
            return Ok(result);
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