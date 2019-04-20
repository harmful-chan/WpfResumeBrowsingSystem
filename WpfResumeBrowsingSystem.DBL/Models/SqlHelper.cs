using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Text;

namespace WpfResumeBrowsingSystem.DBL.Models
{
    public class SqlHelper
    {
        private static string connStr = "server = 47.94.162.230; user id = root; password = 123456; database = ResumeBrowingSystemV00; Character Set = utf8;";

        public static List<T> GetTable<T>() where T:new()
        {
            List<T> tmpTable = new List<T>();    //表

            Type entityType = typeof(T);

            List<PropertyInfo> propertyInfos = new List<PropertyInfo>(typeof(T).GetProperties());    //获取T类型属性信息
            List<string> propertyNames = propertyInfos.ConvertAll<string>(p => p.Name.ToString());    //属性名称转字符串

            using (MySqlConnection conn = new MySqlConnection(SqlHelper.connStr))
            {
                try
                {
                    conn.Open();

                    string sqlStr = string.Format("select * from {0};", entityType.Name.ToString());
                    using (MySqlCommand cmd = new MySqlCommand(sqlStr, conn))
                    {
                        using (MySqlDataReader data = cmd.ExecuteReader())
                        {
                            while (data.Read())
                            {
                                T col = new T();    //临时行
                                for (int i = 0; i < propertyNames.Count; i++)    //遍历属性列表
                                {
                                    string currProperty = propertyNames[i];

                                    if( !(data[currProperty] is System.DBNull) )
                                        col.GetType().GetProperty(currProperty).SetValue(col, data[currProperty]);    //泛型属性设置
                                }
                                tmpTable.Add(col);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();    
                }
                return tmpTable;
            }
        }


        /// <summary>
        /// 中英文字符串等宽10字符输出
        /// </summary>
        /// <param name="srcStr">源字符串</param>
        /// <returns></returns>
        private static string Align(string srcStr)
        {
            int extraSpaces = 0;
            foreach (char c in srcStr.ToCharArray())
            {
                if (Encoding.Default.GetByteCount(c.ToString()).Equals(2))
                {
                    extraSpaces++;
                }
            }
            return srcStr.PadRight(15 - extraSpaces);
        }

        static public string GetTable<T>(List<T> table)
        {
            List<PropertyInfo> propertyInfos = new List<PropertyInfo>(typeof(T).GetProperties());    //获取T类型属性信息
            List<string> propertyNames = propertyInfos.ConvertAll<string>( p => p.Name.ToString());    //属性名称转字符串

            string msg = "";

            //表头
            foreach (var item in propertyNames)
            {
                msg += SqlHelper.Align(item);
            }
            msg += '\n';

            foreach (var t in table)
            {
                foreach (var i in propertyNames)
                {
                    var currProperty = t.GetType().GetProperty(i).GetValue(t);
                    if (currProperty == null) continue;

                    string currPropertyName = currProperty.ToString();

                    if (currProperty is DateTime)
                        currPropertyName = currPropertyName.Replace(" 0:00:00", "");

                    msg += SqlHelper.Align(currPropertyName); 
                }
                msg += '\n';
            }

            return msg;
        }
    }
}
