using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                case ".xls":
                case ".xlsx":
                    return Excel(base_path + guid + extension, name);
                case ".ppt":
                case ".pptx":
                    return PowerPoint(base_path + guid + extension, name);
                case ".pdf":
                    System.IO.File.Copy(base_path + guid + extension,
                        AppDomain.CurrentDomain.BaseDirectory + "PDF Files\\" + name + ".pdf");
                    return name + ".pdf";
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
                else
                {
                    msg = name + ".pdf";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                if (word_app != null)
                {
                    word_app.Visible = false;
                    word_app.Quit();
                }
            }

            return msg;
        }

        public string Excel(string source, string name)
        {
            string msg = null;
            Microsoft.Office.Interop.Excel.Application excel_app = null;
            string out_path = AppDomain.CurrentDomain.BaseDirectory + "PDF Files\\" + name + ".pdf";

            try
            {
                if (!System.IO.File.Exists(out_path))
                {
                    excel_app = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook book = excel_app.Workbooks.Open(source);
                    
                    if (book != null)
                    {
                        Microsoft.Office.Interop.Excel.XlFileFormat xlFormatPDF = (Microsoft.Office.Interop.Excel.XlFileFormat)57;
                        book.SaveAs(out_path, xlFormatPDF);
                        msg = name + ".pdf";
                    }
                    else
                    {
                        msg = "打开文件：" + source + " 失败！";
                    }
                }
                else
                {
                    msg = name + ".pdf";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                if (excel_app != null)
                {
                    excel_app.Visible = false;
                    excel_app.Quit();
                }
            }

            return msg;
        }

        public string PowerPoint(string source, string name)
        {
            string msg = null;
            Microsoft.Office.Interop.PowerPoint.Application powerpoint_app = null;
            string out_path = AppDomain.CurrentDomain.BaseDirectory + "PDF Files\\" + name + ".pdf";

            try
            {
                if (!System.IO.File.Exists(out_path))
                {
                    powerpoint_app = new Microsoft.Office.Interop.PowerPoint.Application();


                    Microsoft.Office.Interop.PowerPoint.Presentation presentation =
                        powerpoint_app.Presentations.Open(source,
                            Microsoft.Office.Core.MsoTriState.msoFalse,
                            Microsoft.Office.Core.MsoTriState.msoFalse,
                            Microsoft.Office.Core.MsoTriState.msoFalse);

                    if (presentation != null)
                    {
                        presentation.SaveAs(out_path, Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsPDF);
                        msg = name + ".pdf";
                    }
                    else
                    {
                        msg = "打开文件：" + source + " 失败！";
                    }
                }
                else
                {
                    msg = name + ".pdf";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                if (powerpoint_app != null)
                {
                    powerpoint_app.Quit();
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(powerpoint_app);
                }
            }

            return msg;
        }
    }
}
