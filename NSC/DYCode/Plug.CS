using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElegantWM.Tools;
using System.IO;
using System.Text;
using ZipApplication;
using Dynashety.DYAttribute;

namespace Dynashety.Plug
{
    public class FileOper
    {
        private I_FileUpLoad I;

        public FileOper()
        {
        }

        public FileOper(I_FileUpLoad I)
        {
            this.I = I;
        }

        public object UpLoad(List<UFileInfo> list, List<HttpPostedFileBase> files)
        {
            try
            {
                return I.Handle(list, files);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public object UpLoad(string md5, HttpPostedFileBase file)
        {
            try
            {
                return I.Handle(md5, file);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DownLoad(List<DFileInfo> files, HttpContextBase c)
        {
            switch (files.Count)
            {
                case 0:
                    break;
                case 1:
                    {
                        var file = files.SingleOrDefault();
                        c.Response.AppendHeader("Content-disposition", "attachment;filename=" + file.fileName);
                        c.Response.WriteFile(AppDomain.CurrentDomain.BaseDirectory + file.fileUrl);
                    }
                    break;
                default:
                    {
                        var Name = string.Empty;
                        var temp = AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["dirPath"] + Path.DirectorySeparatorChar + Guid.NewGuid().ToString("N");
                        if (!Directory.Exists(temp)) Directory.CreateDirectory(temp);
                        files.ToList().ForEach(t =>
                        {
                            Name += t.fileName + "&";
                            File.Copy(AppDomain.CurrentDomain.BaseDirectory + t.fileUrl, temp + Path.DirectorySeparatorChar + t.fileName, true);
                        });
                        var outName = Path.GetDirectoryName(temp) + Path.DirectorySeparatorChar + "[" + Name.TrimEnd('&') + "]等" + files.Count() + "个文件.ZIP";
                        new ZipFloClass().ZipFile(temp, outName);
                        using (MemoryStream m = new MemoryStream())
                        {
                            using (FileStream f = new FileStream(outName, FileMode.Open)) f.CopyTo(m);
                            c.Response.AppendHeader("Content-disposition", "attachment;filename=" + Path.GetFileName(outName));
                            c.Response.Charset = "utf8";
                            c.Response.ContentEncoding = Encoding.UTF8;
                            c.Response.BinaryWrite(m.GetBuffer());
                        }
                        File.Delete(outName);
                        Directory.Delete(temp, true);
                    }
                    break;
            }
        }
    }

    public class RetFileInfo
    {
        public string fileUrl;
        public string fileName;
        public string contentType;
        public int contentLength;
    }

    public class UFileInfo
    {
        public string fileName;
        public string md5;
    }

    public class DFileInfo
    {
        public string fileName;
        public string fileUrl;
    }

    public interface I_FileUpLoad
    {
        List<RetFileInfo> Handle(List<UFileInfo> f, List<HttpPostedFileBase> t);
        string Handle(string md5, HttpPostedFileBase file);
    }
}