using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MySql.Data.MySqlClient;


namespace WpfResumeBrowsingSystem.DBL
{
    public class SqlHelper
    {
        private static string connStr = "server = 47.94.162.230; user id = root; password = 123456; database = ResumeBrowingSystemV00; Character Set = utf8;";

        public List<T> GetTable<T>() where T:new()
        {
            List<T> tmpTable = new List<T>();    //表

            Type entityType = typeof(T);    //泛型类型

            List<string> propertyList = new List<string>();    //属性列表
            var entityTypeProperties = entityType.GetProperties();
            foreach(PropertyInfo item in entityTypeProperties)
            {
                propertyList.Add(item.Name.ToString());
            }

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
                                for (int i = 0; i < propertyList.Count; i++)    //遍历属性列表
                                {
                                    string currProperty = propertyList[i];
                                    col.GetType().GetProperty(currProperty).SetValue(col, data[currProperty].ToString());    //泛型属性设置
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
    }
}
