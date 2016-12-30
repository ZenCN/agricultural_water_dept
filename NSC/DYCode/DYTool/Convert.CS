using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.IO;

namespace ElegantWM.WebUI.Areas.WeiXin
{
    public class WXConvert
    {
        public static string XMLDocumentToString(XmlDocument doc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            doc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return xmlString;
        }
    }

    public class XmlEntity<T> where T : new()
    {
        /// <summary>
        /// 将XML转换为对象
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T ConvertXml2Entity(string xml)
        {
            XmlDocument doc = new XmlDocument();
            PropertyInfo[] propinfos = null;
            doc.LoadXml(xml);
            XmlNodeList nodelist = doc.SelectNodes("/xml");
            T entity = new T();
            foreach (XmlNode node in nodelist)
            {
                //初始化propertyinfo
                if (propinfos == null)
                {
                    Type objtype = entity.GetType();
                    propinfos = objtype.GetProperties();
                }
                //填充entity类的属性
                foreach (PropertyInfo pi in propinfos)
                {
                    XmlNode cnode = node.SelectSingleNode(pi.Name);
                    pi.SetValue(entity, Convert.ChangeType(cnode.InnerText, pi.PropertyType), null);
                }
            }
            return entity;
        }

        /// <summary>
        /// 将XML转换为对象
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T ConvertXml2Entity(XmlReader xml)
        {
            XmlDocument doc = new XmlDocument();
            PropertyInfo[] propinfos = null;
            doc.Load(xml);
            XmlNodeList nodelist = doc.SelectNodes("/xml");
            T entity = new T();
            foreach (XmlNode node in nodelist)
            {
                //初始化propertyinfo
                if (propinfos == null)
                {
                    Type objtype = entity.GetType();
                    propinfos = objtype.GetProperties();
                }
                //填充entity类的属性
                foreach (PropertyInfo pi in propinfos)
                {
                    XmlNode cnode = node.SelectSingleNode(pi.Name);
                    pi.SetValue(entity, Convert.ChangeType(cnode.InnerText, pi.PropertyType), null);
                }
            }
            return entity;
        }

        /// <summary>
        /// 将XML转换为对象
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T ConvertXml2Entity(XmlDocument xml)
        {
            PropertyInfo[] propinfos = null;
            XmlNodeList nodelist = xml.SelectNodes("/xml");
            T entity = new T();
            foreach (XmlNode node in nodelist)
            {
                //初始化propertyinfo
                if (propinfos == null)
                {
                    Type objtype = entity.GetType();
                    propinfos = objtype.GetProperties();
                }
                //填充entity类的属性
                foreach (PropertyInfo pi in propinfos)
                {
                    XmlNode cnode = node.SelectSingleNode(pi.Name);
                    pi.SetValue(entity, Convert.ChangeType(cnode.InnerText, pi.PropertyType), null);
                }
            }
            return entity;
        }

        /// <summary>
        /// 构造微信消息
        /// </summary>
        /// <param name="t">对象实体</param>
        /// <returns>返回微信消息xml格式</returns>
        public static string ConvertEntity2Xml(T t)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<xml>");
            Type objtype = t.GetType();
            //填充entity类的属性
            foreach (PropertyInfo pi in objtype.GetProperties())
            {
                object obj = pi.GetValue(t, null);
                string value = obj == null ? "" : obj.ToString();
                if (pi.PropertyType.Name.ToLower() != "string")
                    builder.Append("<" + pi.Name + ">" + value + "</" + pi.Name + ">");
                else
                    builder.Append("<" + pi.Name + "><![CDATA[" + value + "]]></" + pi.Name + ">");
            }
            builder.Append("</xml>");
            return builder.ToString();
        }
    }
}