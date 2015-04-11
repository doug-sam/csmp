using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Web.UI.WebControls;
using System.Reflection;

namespace Tool
{
    public class DocHelper
    {
        public const string DefalutPath = "~/file/download/";

        /// <summary>
        /// 检查文件,如果文件不存在则创建 
        /// </summary>
        /// <param name="FilePath"></param>
        private static void ExistsFile(string FilePath)
        {
            FilePath = FilePath.Replace("/", "\\");
            //以上写法会报错,详细解释请看下文.........
            if (!File.Exists(FilePath))
            {
                string Dir = FilePath.Substring(0, FilePath.LastIndexOf("\\") + 1);
                if (!Directory.Exists(Dir))
                {
                    Directory.CreateDirectory(Dir);
                }
                // File.Create(FilePath);
                FileStream fs = File.Create(FilePath);
                fs.Close();
            }
        }
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="FilePath">相对系统根目录的路径</param>
        /// <param name="utf"></param>
        /// <returns></returns>
        public static string Read(string FilePath, bool utf)
        {
            FilePath = HttpContext.Current.Server.MapPath(FilePath);
            string input = "";
            ExistsFile(FilePath);//检查文件是否存在 
            //读取文件 
            try
            {
                StreamReader sr = new StreamReader(FilePath, utf ? System.Text.Encoding.UTF8 : System.Text.Encoding.Default);
                input = sr.ReadToEnd();
                sr.Close(); //有的平台只有\n表示换行　如 mac,linux之流，windows平台换行使用\r\n 
                //所以就连.net框架都有一个&nbsp; System.Environment.NewLine;　以实现不同平台的换行．</DIV>
                //input = input.Replace("\r\n", "").Replace("\n", ""); //注:\r\n在winform中是换行，在html的文档内换行,显示出来的页面是不会换行的.
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return input;
        }
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="FilePath">相对系统根目录的路径</param>
        public static string Read(string FilePath)
        {
            FilePath = HttpContext.Current.Server.MapPath(FilePath);
            return Read(FilePath, false);
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="FilePath">相对站点根目录的路径</param>
        /// <param name="NewString"></param>
        /// <returns></returns>
        public static bool Write(string FilePath, string NewString)
        {
            FilePath = HttpContext.Current.Server.MapPath(FilePath);
            return Write(FilePath, NewString, false);
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="FilePath">相对站点根目录的路径</param>
        public static bool Write(string FilePath, string NewString, bool utf)
        {
            FilePath = HttpContext.Current.Server.MapPath(FilePath);
            ExistsFile(FilePath);//检查文件是否存在 
            StreamWriter sr = new StreamWriter(FilePath, false, utf ? System.Text.Encoding.UTF8 : System.Text.Encoding.Default);
            try
            {
                sr.Write(NewString);
                sr.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public string WavToMP3(string sFileName_wav, string sFileName_mp3, bool bWait)
        {
            FileInfo fi;

            // Path for lame.exe and WAV file.
            fi = new FileInfo(sFileName_wav);
            string sPath_wav = fi.DirectoryName;

            // Path for MP3 file.
            fi = new FileInfo(sFileName_mp3);
            string sPath_mp3 = fi.DirectoryName;

            if (!File.Exists(sFileName_wav))
            {
                return "WAV file not found:" + "\r\n" + sFileName_wav;
            }

            if (!Directory.Exists(sPath_mp3))
            {
                return "Directory for MP3 file not found:" + "\r\n" + sPath_mp3;
            }

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;      // No Command Prompt window.
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.FileName = "\"" + sPath_wav + @"\lame.exe" + "\"";
                psi.Arguments = "-h " +  //-b 128 --resample 22.05 -m j
                                "\"" + sFileName_wav + "\"" + " " +
                                "\"" + sFileName_mp3 + "\"";
                // psi.Arguments = "-h" + "\"" + "\"" + sFileName_wav + "\"" + " " + "\"" + sFileName_mp3 + "\"";
                Process p = Process.Start(psi);
                if (bWait)
                {
                    p.WaitForExit();
                }
                p.Close();
                p.Dispose();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// 导出数据到Excel
        /// </summary>
        /// <param name="table">数据源</param>
        /// <param name="file">导出文件名</param>
        public static void ExportExcel(DataTable table, string file)
        {
            Excel.Application excel = new Excel.Application();
            Excel.Workbook book = excel.Workbooks.Add(Missing.Value);
            Excel.Worksheet sheet = (Excel.Worksheet)book.ActiveSheet;

            for (int col = 0; col < table.Columns.Count; col++)
            {
                sheet.Cells[1, col + 1] = table.Columns[col].ColumnName;
            }

            Excel.Range range = excel.get_Range(excel.Cells[1, 1], excel.Cells[1, table.Columns.Count]);

            range.Interior.ColorIndex = 42;
            excel.get_Range(excel.Cells[1, 1], excel.Cells[table.Rows.Count+1, table.Columns.Count]).NumberFormatLocal = "@"; //设置为文本格式
            for (int row = 0; row < table.Rows.Count; row++)
            {
                for (int col = 0; col < table.Columns.Count; col++)
                {

                    sheet.Cells[row + 2, col + 1] = table.Rows[row][col].ToString();
                }
            }
            excel.Cells.Select();//全表自动列宽
            excel.Cells.Columns.AutoFit();//全表自动列宽
            //excel.Save(file);
            book.SaveAs(file, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            book.Close(true, file, Missing.Value);
            excel.Quit();
            GC.Collect();

        }


        /// <summary>
        /// 导出数据到Excel
        /// </summary>
        /// <param name="table">数据源</param>
        /// <param name="file">导出文件名</param>
        public static void ExportExcel(DataTable table, GridView dg)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = Encoding.Default;

            dg.Visible = true;
            dg.EnableViewState = false;
            dg.AllowPaging = false;
            dg.DataSource = table;
            dg.DataBind();
            StringWriter oStringWriter = new StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            dg.RenderControl(oHtmlTextWriter);
            HttpContext.Current.Response.Write(oStringWriter.ToString());

            HttpContext.Current.Response.End();
        }

    }

}