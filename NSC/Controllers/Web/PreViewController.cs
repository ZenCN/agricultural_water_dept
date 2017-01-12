using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;
using System.IO;
using NSC;

namespace Suya.Web.Apps.Areas.PMP.Controllers
{
    /// <summary>
    /// 在线预览Office文件
    /// </summary>
    public class PreviewController : Controller
    {
        private string base_path = AppDomain.CurrentDomain.BaseDirectory +
                                   System.Configuration.ConfigurationManager.AppSettings["dirPath"] + "\\";

        private NSCEntities db = new NSCEntities();

        public string Index(int id, string table)
        {
            string name = null;
            string guid = null;
            string extension = null;

            switch (table)
            {
                case "dt01":
                    DT01 dt01 = db.DT01.SingleOrDefault(t => t.D01 == id);
                    name = dt01.D02;
                    guid = dt01.D04.Replace("Zizo\\", "");
                    break;
                case "dt02":
                    DT02 dt02 = db.DT02.SingleOrDefault(t => t.D01 == id);
                    name = dt02.D02;
                    guid = dt02.D04.Replace("Zizo\\", "");;
                    break;
                case "dt03":
                    DT03 dt03 = db.DT03.SingleOrDefault(t => t.D01 == id);
                    name = dt03.D02;
                    guid = dt03.D04.Replace("Zizo\\", "");;
                    break;
            }

            extension = name.Substring(name.LastIndexOf("."));
            name = name.Substring(0, name.LastIndexOf("."));


            if (!System.IO.File.Exists(base_path + guid + extension))
            {
                System.IO.File.Copy(base_path + guid, base_path + guid + extension);
            }

            switch (extension)
            {
                case ".doc":
                case ".docx":
                    return Word(base_path + guid + extension, name);
                default:
                    return "";
            }
        }

        public string Word(string source, string name)
        {
            string msg = null;
            Microsoft.Office.Interop.Word.Application word_app = null;
            string out_path = AppDomain.CurrentDomain.BaseDirectory + "PDF Files\\" + name + ".pdf";

            try
            {
                if (!System.IO.File.Exists(out_path))
                {
                    word_app = new Microsoft.Office.Interop.Word.Application();
                    var doc = word_app.Documents.Open(source);

                    if (doc != null)
                    {
                        doc.SaveAs(out_path, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                        msg = name + ".pdf";
                    }
                    else
                    {
                        msg = "打开文件：" + source + " 失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                word_app.Visible = false;
                word_app.Quit();
            }

            return msg;
        }
    }
}
