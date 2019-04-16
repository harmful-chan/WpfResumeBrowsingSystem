using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;
using WpfResumeBrowsingSystem.Domain.Models;
using Microsoft.AspNetCore.StaticFiles;

namespace WpfResumeBrowsingSystem.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDirectoryBrowser();    //添加文件浏览功能
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseStaticFiles();    //启用wwwroot目录
            
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    //ServeUnknownFileTypes = true 
            //    ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
            //    {
            //        { ".jpg","image/jpeg"}
            //    })
            //});
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()    
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/images"
            });
        }
    }
}
