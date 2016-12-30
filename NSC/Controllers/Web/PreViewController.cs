using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.IO;
using Microsoft.Office.Interop.Word;
using NSC;
namespace Suya.Web.Apps.Areas.PMP.Controllers
{
    /// <summary>
    /// 在线预览Office文件
    /// </summary>
    public class OfficeViewController : Controller
    {
        #region Index页面
        /// <summary>
        /// Index页面
        /// </summary>
        /// <param name="url">例：/uploads/......XXX.xls</param>
        public ActionResult Index(int index)
        {
            var obj = new NSCEntities().DT01.SingleOrDefault(t => t.D01 == index && t.D99 == 0);
            string htmlUrl = string.Empty;
            string extension = Path.GetExtension(obj.D02);
            string physicalPath = AppDomain.CurrentDomain.BaseDirectory + obj.D04;
            switch (extension.ToLower())
            {
                case ".xls":
                case ".xlsx":
                    htmlUrl = PreviewExcel(physicalPath);
                    break;
                case ".doc":
                case ".docx":
                    htmlUrl = PreviewWord(physicalPath);
                    break;
                case ".txt":
                    htmlUrl = PreviewTxt(physicalPath);
                    break;
                case ".pdf":
                    htmlUrl = PreviewPdf(physicalPath);
                    break;
            }
            return Redirect("../temp/" + htmlUrl);
        }
        #endregion
        #region 预览Excel
        /// <summary>
        /// 预览Excel
        /// </summary>
        public string PreviewExcel(string physicalPath)
        {
            Microsoft.Office.Interop.Excel._Application application = null;
            Microsoft.Office.Interop.Excel._Workbook workbook = null;
            application = new Microsoft.Office.Interop.Excel.Application();
            object missing = Type.Missing;
            object trueObject = true;
            application.Visible = false;
            application.DisplayAlerts = false;
            workbook = application.Workbooks.Open(physicalPath, missing, trueObject, missing, missing, missing,
              missing, missing, missing, missing, missing, missing, missing, missing, missing);
            object format = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
            string htmlName = Path.GetFileNameWithoutExtension(physicalPath) + ".html";
            String outputFile = AppDomain.CurrentDomain.BaseDirectory + "temp" + Path.DirectorySeparatorChar + htmlName;
            workbook.SaveAs(outputFile, format, missing, missing, missing,
                     missing, XlSaveAsAccessMode.xlNoChange, missing,
                     missing, missing, missing, missing);
            workbook.Close();
            application.Quit();
            return htmlName;
        }
        #endregion
        #region 预览Word
        /// <summary>
        /// 预览Word
        /// </summary>
        public string PreviewWord(string physicalPath)
        {
            Microsoft.Office.Interop.Word._Application application = null;
            Microsoft.Office.Interop.Word._Document document = null;
            application = new Microsoft.Office.Interop.Word.Application();
            object missing = Type.Missing;
            object trueObject = true;
            application.Visible = false;
            application.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            document = application.Documents.Open(physicalPath, missing, trueObject, missing, missing, missing,
              missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML;
            string htmlName = Path.GetFileNameWithoutExtension(physicalPath) + ".html";
            String outputFile = AppDomain.CurrentDomain.BaseDirectory + "temp" + Path.DirectorySeparatorChar + htmlName;
            document.SaveAs(outputFile, format, missing, missing, missing,
                     missing, XlSaveAsAccessMode.xlNoChange, missing,
                     missing, missing, missing, missing);
            document.Close();
            application.Quit();
            return htmlName;
        }
        #endregion
        #region 预览Txt
        /// <summary>
        /// 预览Txt
        /// </summary>
        public string PreviewTxt(string physicalPath)
        {
            var htmlName = Path.GetFileNameWithoutExtension(physicalPath) + ".html";
            System.IO.File.Copy(physicalPath, AppDomain.CurrentDomain.BaseDirectory + "temp" + Path.DirectorySeparatorChar + htmlName, true);
            return htmlName;
        }
        #endregion
        #region 预览Pdf
        /// <summary>
        /// 预览Pdf
        /// </summary>
        public string PreviewPdf(string physicalPath)
        {
            var htmlName = Path.GetFileNameWithoutExtension(physicalPath) + ".pdf";
            System.IO.File.Copy(physicalPath, AppDomain.CurrentDomain.BaseDirectory + "temp" + Path.DirectorySeparatorChar + htmlName, true);
            return htmlName;
        }
        #endregion
    }
}
