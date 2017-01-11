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
                                   System.Configuration.ConfigurationManager.AppSettings["dirPath"];

        public string Preview(string name, string guid)
        {
            string extension = name.Substring(name.LastIndexOf("."));
            name = name.Substring(0, name.LastIndexOf(".") - 1);


            if (!System.IO.File.Exists(base_path + guid + extension))
            {
                System.IO.File.Copy(base_path + guid, base_path + guid + extension);
            }




            if (name.IndexOf(".doc") > 0)
            {
                Word(base_path + guid + extension, name);
            }
        }

        public string Word(string source, string name)
        {
            try
            {
                string out_path = base_path + name + ".pdf";

                var WordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document doc = WordApp.Documents.Open(source);

                doc.SaveAs(out_path, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                WordApp.Visible = false;
                WordApp.Quit();

                return out_path;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
