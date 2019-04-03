using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
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

        // POST api/files
        [HttpPost]
        public IActionResult Post(IFormCollection form)
        {
            string topDirectory = this._hostingEnvironment.ContentRootPath;    //顶级目录
            //string wwwDirectory = this._hostingEnvironment.WebRootPath;    //wwwroot目录

            FormFileCollection formFiles = form.Files as FormFileCollection;
            foreach (FormFile file in formFiles)
            {

                string fileFullName = Path.Combine(topDirectory, "StaticFiles", "images", file.Name);    //上传文件的据对路径

                if (System.IO.File.Exists(fileFullName))   //若有则删
                {
                    System.IO.File.Delete(fileFullName);
                }

                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }

            return Ok("post file success");
        }

    }
}