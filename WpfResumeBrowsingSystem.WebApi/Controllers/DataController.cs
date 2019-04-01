using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Newtonsoft.Json;
using WpfResumeBrowsingSystem.Domain.Models;
namespace WpfResumeBrowsingSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string tbName)
        {
            //检查表名是否存在
            Assembly ass = Assembly.Load("WpfResumeBrowsingSystem.Domain");
            Type tbObj = ass.GetType("WpfResumeBrowsingSystem.Domain.Models."+tbName);
            if (null == tbObj) return NotFound();

            //返表 Json数据
            using (ResumeBrowingSystemV00Context db = new ResumeBrowingSystemV00Context())
            {
                if (tbName == "Staffs") return Ok(db.Staffs.ToList());
                else if (tbName == "Experiences") return Ok(db.Experiences.ToList());
                else return NotFound();
            }
        }
    }
}