using System;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace ElegantWM.Tools
{
    public static class Tool
    {
        /// <summary>
        /// 跨域访问
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Post(string url, string param, int time = 60000)
        {
            Uri address = new Uri(url);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8"; //"application/x-www-form-urlencoded";
            request.Timeout = time;
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(param == null ? string.Empty : param);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            string result = string.Empty;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();
            }
            return (result);
        }

        /// <summary>
        /// 跨域访问
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Get(string url, int time = 60000)
        {
            Uri address = new Uri(url);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json;charset=utf-8"; //"application/x-www-form-urlencoded";
            request.Timeout = time;
            string result = string.Empty;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();
            }
            return (result);
        }

        public static string getMd5(string str)
        {
            MD5 md5 = MD5.Create();
            StringBuilder sb = new StringBuilder();
            byte[] retVal = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string getMd5(Stream str)
        {
            MD5 md5 = MD5.Create();
            StringBuilder sb = new StringBuilder();
            str.Seek(0, SeekOrigin.Begin);
            byte[] retVal = md5.ComputeHash(str);
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static void Write(this string s)
        {
            StreamWriter dout = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Log.txt", true);
            dout.Write("事件：" + s + "\r\n时间：" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
            dout.Close();
        }
    }
}