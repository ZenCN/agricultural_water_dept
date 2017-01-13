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

        public string Index(int id, string table, string type)
        {
            string name = null, guid = null, extension, pdf_file;

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
                case "dt04":
                    DT04 dt04 = db.DT04.SingleOrDefault(t => t.D01 == id);
                    switch (type)
                    {
                        case "acept_report":
                            name = dt04.D04;
                            guid = dt04.D07;
                            break;
                        case "acept_data":
                            name = dt04.D05;
                            guid = dt04.D08;
                            break;
                        case "acept_card":
                            name = dt04.D05;
                            guid = dt04.D08;
                            break;
                    }
                    break;
            }

            extension = name.Substring(name.LastIndexOf("."));

            //生成的pdf文件必须是以guid为文件名，否则遇到同名但文件类型不同的文件预览时会产生不一致现象
            pdf_file = AppDomain.CurrentDomain.BaseDirectory + "PDF Files\\" + guid + ".pdf";

            if (System.IO.File.Exists(pdf_file)) //存在pdf，直接返回
            {
                return guid + ".pdf";
            }
            else
            {
                if (extension != ".pdf")
                {
                    if (System.IO.File.Exists(base_path + guid))
                    {
                        switch (extension)
                        {
                            case ".doc":
                            case ".docx":
                                return Word(guid);
                            case ".xls":
                            case ".xlsx":
                                return Excel(guid);
                            case ".ppt":
                            case ".pptx":
                                return PowerPoint(guid);
                            default:
                                return "文件：" + name + " 不是Office文档";
                        }
                    }
                    else
                    {
                        return "文件：" + base_path + guid + " 不存在！";
                    }
                }
                else
                {
                    System.IO.File.Copy(base_path + guid, pdf_file);

                    return guid + ".pdf";
                }
            }
        }

        public string Word(string guid)
        {
            string msg = null;
            Microsoft.Office.Interop.Word.Application word_app = null;
            string out_path = AppDomain.CurrentDomain.BaseDirectory + "PDF Files\\" + guid + ".pdf";

            try
            {
                word_app = new Microsoft.Office.Interop.Word.Application();
                var doc = word_app.Documents.Open(base_path + guid);

                if (doc != null)
                {
                    doc.SaveAs(out_path, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                    msg = guid + ".pdf";
                }
                else
                {
                    msg = "Office Word 打开文件：" + base_path + guid + " 失败！";
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

        public string Excel(string guid)
        {
            string msg = null;
            Microsoft.Office.Interop.Excel.Application excel_app = null;
            string out_path = AppDomain.CurrentDomain.BaseDirectory + "PDF Files\\" + guid + ".pdf";

            try
            {
                excel_app = new Microsoft.Office.Interop.Excel.Application();
                //多个Sheet有问题
                Microsoft.Office.Interop.Excel.Workbook book = excel_app.Workbooks.Open(base_path + guid);

                if (book != null)
                {
                    Microsoft.Office.Interop.Excel.XlFileFormat xlFormatPDF = (Microsoft.Office.Interop.Excel.XlFileFormat)57;
                    book.SaveAs(out_path, xlFormatPDF);
                    msg = guid + ".pdf";
                }
                else
                {
                    msg = "Office Excel 打开文件：" + base_path + guid + " 失败！";
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

        public string PowerPoint(string guid)
        {
            string msg = null;
            Microsoft.Office.Interop.PowerPoint.Application powerpoint_app = null;
            string out_path = AppDomain.CurrentDomain.BaseDirectory + "PDF Files\\" + guid + ".pdf";

            try
            {
                powerpoint_app = new Microsoft.Office.Interop.PowerPoint.Application();

                Microsoft.Office.Interop.PowerPoint.Presentation presentation =
                    powerpoint_app.Presentations.Open(base_path + guid,
                        Microsoft.Office.Core.MsoTriState.msoFalse,
                        Microsoft.Office.Core.MsoTriState.msoFalse,
                        Microsoft.Office.Core.MsoTriState.msoFalse);

                if (presentation != null)
                {
                    presentation.SaveAs(out_path, Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsPDF);
                    msg = guid + ".pdf";
                }
                else
                {
                    msg = "Office PowerPoint 打开文件：" + base_path + guid + " 失败！";
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
