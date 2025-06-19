using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace MLib
{
    public class IJson
    {
        /// <summary>
        /// 从指定目录和文件名读取JSON文件并反序列化为对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="directory">文件目录</param>
        /// <param name="fileName">文件名称</param>
        /// <returns>反序列化后的对象</returns>
        public static T ReadJson<T>(string directory, string fileName)
        {
            string json = "";
            string filePath = System.IO.Path.Combine(directory, fileName);
            if (!File.Exists(filePath))
                return default(T);

            json = File.ReadAllText(filePath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 将对象序列化为JSON并写入指定目录和文件名
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="directory">文件目录</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="data">要序列化的对象</param>
        public static void WriteJson<T>(string directory, string fileName, T data)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string filePath = System.IO.Path.Combine(directory, fileName);
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }

        public static object lck = new object();

        public static async void WriteJsonAsync<T>(string directory, string fileName, T data)
        {
            await Task.Run(() =>
            {
                lock (lck)
                {
                    WriteJson(directory, fileName, data);
                }

            });

        }

    }
}
