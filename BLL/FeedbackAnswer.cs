using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Reflection;

namespace CSMP.BLL
{
    public static class FeedbackAnswerBLL
    {
        private static readonly DAL.FeedbackAnswerDAL dal = new DAL.FeedbackAnswerDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackAnswerInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackAnswerInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }



        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackAnswerInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static FeedbackAnswerInfo Get(int id)
        {
            return dal.Get(id);
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public static FeedbackAnswerInfo Get(int QuestionID, int CallID)
        {
            return dal.Get(QuestionID, CallID);
        }
        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(FeedbackAnswerInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(FeedbackAnswerInfo info)
        {
            return dal.Edit(info);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        public static bool Delete(int id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// 删除Member
        /// </summary>
        /// <param name="id">Member id</param>
        public static bool DeleteByCallID(int CallID)
        {
            return dal.DeleteByCallID(CallID);
        }


        #endregion


        /// <summary>
        /// 导出数据到Excel
        /// </summary>
        /// <param name="table">数据源</param>
        /// <param name="file">导出文件名</param>
        public static void ExportExcel(List<FeedbackAnswerInfo> listAnswer,int PaperID, string file)
        {

          DataTable dt= ToDatatable(listAnswer, PaperID);
            
            Excel.Application excel = new Excel.Application();
            Excel.Workbook book = excel.Workbooks.Add(Missing.Value);
            Excel.Worksheet sheet = (Excel.Worksheet)book.ActiveSheet;

            for (int col = 0; col < dt.Columns.Count; col++)
            {
                sheet.Cells[1, col + 1] = dt.Columns[col].ColumnName;
            }

            Excel.Range range = excel.get_Range(excel.Cells[1, 1], excel.Cells[1, dt.Columns.Count]);

            range.Interior.ColorIndex = 42;
            excel.get_Range(excel.Cells[1, 1], excel.Cells[dt.Rows.Count + 1, dt.Columns.Count]).NumberFormatLocal = "@"; //设置为文本格式

            string CallNo = string.Empty;
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                int startColumns = 0;
                if (dt.Rows[row][0].ToString()==CallNo)
                {
                    startColumns = 9;
                }
                else
                {
                    CallNo = dt.Rows[row][0].ToString();
                }
                for (int col = startColumns; col < dt.Columns.Count; col++)
                {
                   
                    sheet.Cells[row + 2, col + 1] = dt.Rows[row][col].ToString();
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

        private static DataTable ToDatatable(List<FeedbackAnswerInfo> listAnswer, int PaperID)
        {
            List<FeedbackQuestionInfo> listQuestion = FeedbackQuestionBLL.GetList(PaperID);
            DataTable dt = new DataTable();
            dt.Columns.Add("系统单号");
            dt.Columns.Add("回访时间");
            dt.Columns.Add("客户");
            dt.Columns.Add("品牌");
            dt.Columns.Add("城市");
            dt.Columns.Add("店铺");
            dt.Columns.Add("回访人");
            dt.Columns.Add("被回访人");
            dt.Columns.Add("回访故障");
            dt.Columns.Add("问题");
            dt.Columns.Add("回答");

            DataRow dr = dt.NewRow();
            foreach (FeedbackAnswerInfo item in listAnswer)
            {
                CallInfo cinfo = CallBLL.Get(item.CallID);
                dr = dt.NewRow();
                dr["系统单号"] = cinfo.No;
                dr["回访时间"] = item.AddDate.ToString("yyyy-MM-dd HH:mm:ss");
                dr["客户"] = cinfo.CustomerName;
                dr["品牌"] = cinfo.BrandName;
                dr["城市"] = cinfo.CityName;
                dr["城市"] = cinfo.CityName;
                dr["店铺"] = cinfo.StoreName + "/" + cinfo.StoreNo;
                dr["回访人"] = item.RecorderName;
                dr["被回访人"] = item.FeedbackUserName;
                dr["回访故障"] = cinfo.ClassName2;
                dr["问题"] = item.QuestionName;
                dr["回答"] = item.Answer;
                dt.Rows.Add(dr);
            }
            return dt;
        }

    }
}