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
        /// <summary>
        /// 中英文字符串等宽10字符输出
        /// </summary>
        /// <param name="srcStr">源字符串</param>
        /// <returns></returns>
        public static string Align(string srcStr)
        {
            int extraSpaces = 0;
            foreach (char c in srcStr.ToCharArray())
            {
                if(Encoding.Default.GetByteCount(c.ToString()).Equals(2))
                {
                    extraSpaces++;
                }
            }
            return srcStr.PadRight(10 - extraSpaces);
        }

        public static void ShowTable()
        {
            List<Staffs> c = new SqlHelper().GetTable<Staffs>();

            PropertyInfo[] propertyList = typeof(Staffs).GetProperties();
            foreach (var item in propertyList)
            {
                Console.Write("{0}", Align(item.Name.ToString()));
            }
            Console.WriteLine();

            foreach (var item in c)
            {
                for (int i = 0; i < propertyList.Length; i++)
                {
                    Console.Write(Align(propertyList[i].GetValue(item).ToString()));
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }


        static void Main(string[] args)
        {
            ShowTable(); 
        }

       
    }
}
