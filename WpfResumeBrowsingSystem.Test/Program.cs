using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WpfResumeBrowsingSystem.DBL.Models;

namespace WpfResumeBrowsingSystem.Test
{
    class Program
    {



        static void Main(string[] args)
        {
            List<Staffs> s;
            s = SqlHelper.GetTable<Staffs>();

            Console.WriteLine(SqlHelper.ShowTable<Staffs>(s));
            Console.ReadLine();
        }

       
    }
}
