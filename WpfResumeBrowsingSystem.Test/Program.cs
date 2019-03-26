using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using WpfResumeBrowsingSystem.DBL;
namespace WpfResumeBrowsingSystem.Test
{
    class Program
    {



        static void Main(string[] args)
        {
            Console.Write(SqlHelper.ShowTable<Staffs>(SqlHelper.GetTable<Staffs>()));
            Console.ReadLine();
        }

       
    }
}
