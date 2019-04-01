using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using WpfResumeBrowsingSystem.Domain.Models;
namespace WpfResumeBrowsingSystem.Domain
{
    class Program
    {
        static public void Main(string[] args)
        {
            using (ResumeBrowingSystemV00Context db = new ResumeBrowingSystemV00Context())
            {
                List<Staffs> s = db.Staffs.ToList();
            }
        }
    }
}
