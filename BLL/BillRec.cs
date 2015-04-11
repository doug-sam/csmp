using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using Tool;
using System.Web;
using System.IO;

namespace CSMP.BLL
{
    public static class BillRecBLL
    {
        private static readonly DAL.BillRecDAL dal = new DAL.BillRecDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<BillRecInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<BillRecInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<BillRecInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static BillRecInfo Get(int id)
        {
            return dal.Get(id);
        }

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static BillRecInfo Get(string CallNo, string Pwd)
        {
            return dal.Get(CallNo, Pwd);
        }


        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        private static int Add(BillRecInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(BillRecInfo info)
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


        #endregion

        private const string PrintChar = "((((({0})))))";

        /// <summary>
        /// 生成服务单
        /// </summary>
        /// <param name="cinfo">CallsInfo</param>
        /// <param name="WorkDetails"></param>
        /// <param name="StepJobCode">上门准备那一步记录</param>
        /// <param name="TemplageID">使用模板</param>
        /// <returns>返回生成的服务单地址</returns>
        private static string WriteBill(CallInfo cinfo, CallStepInfo StepJobCode, int TemplageID)
        {
            if (null == cinfo || null == StepJobCode)
            {
                return string.Empty;
            }
            TemplageID = TemplageID <= 0 ? 1 : TemplageID;

            StoreInfo sinfo = StoresBLL.Get(cinfo.StoreID);
            if (null == sinfo) return string.Empty;
            int MaxSln1Count = CallStepBLL.GetListSln1(cinfo.ID).Count;

            string doc = DocHelper.Read("~/page/sys/print" + TemplageID + ".html", true);
            if (string.IsNullOrEmpty(doc))
            {
                return string.Empty;
            }

            doc = doc.Replace(string.Format(PrintChar, "No"), cinfo.No + "_" + MaxSln1Count);
            doc = doc.Replace(string.Format(PrintChar, "ErrorDate"), cinfo.ErrorDate.ToString("yyyy-MM-dd HH:mm"));
            doc = doc.Replace(string.Format(PrintChar, "DropInDate"), StepJobCode.DateBegin.ToString("yyyy-MM-dd HH:mm"));
            doc = doc.Replace(string.Format(PrintChar, "JOBCODE"), string.Empty);//StepJobCode.StepName
            doc = doc.Replace(string.Format(PrintChar, "CustomerName"), cinfo.StoreNo + "__" + cinfo.BrandName);
            doc = doc.Replace(string.Format(PrintChar, "ErrorReportUser"), cinfo.ErrorReportUser);
            doc = doc.Replace(string.Format(PrintChar, "Tel"), sinfo.Tel);
            doc = doc.Replace(string.Format(PrintChar, "Address"), sinfo.Address);
            doc = doc.Replace(string.Format(PrintChar, "MaintaimUserName"), cinfo.MaintaimUserName);
            doc = doc.Replace(string.Format(PrintChar, "Details"), cinfo.Details);
            doc = doc.Replace(string.Format(PrintChar, "Sugesstion"), StepJobCode.Details);
            doc = doc.Replace(string.Format(PrintChar, "现场工程师"), StepJobCode.UserName);
            doc = doc.Replace(string.Format(PrintChar, "No"), cinfo.No);

            string Dir = DocHelper.DefalutPath + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
            Dir += sinfo.No + "/" + cinfo.No + "_" + MaxSln1Count + DateTime.Now.ToString("ffff") + ".html";
            if (DocHelper.Write(Dir, doc, true))
            {
                string FileUlr = Dir.Replace("~", "").Replace("\\", "/");
                return FileUlr;
            }
            return string.Empty;
        }

        /// <summary>
        /// 生成服务单
        /// </summary>
        /// <param name="cinfo">CallsInfo</param>
        /// <param name="WorkDetails"></param>
        /// <param name="StepJobCode">上门准备那一步记录</param>
        /// <param name="TemplageID">使用模板</param>
        /// <returns>返回生成的服务单地址</returns>
        private static string WriteBillWord(CallInfo cinfo, CallStepInfo StepJobCode, int TemplageID)
        {
            if (null == cinfo || null == StepJobCode)
            {
                return string.Empty;
            }
            TemplageID = TemplageID <= 0 ? 1 : TemplageID;

            StoreInfo sinfo = StoresBLL.Get(cinfo.StoreID);
            if (null == sinfo) return string.Empty;
            int MaxSln1Count = CallStepBLL.GetListSln1(cinfo.ID).Count;

            string FileName = cinfo.No + "_" + MaxSln1Count + DateTime.Now.ToString("ffff") + ".docx";
            string FileDir =AttachmentBLL.GetAttachmentPath()+ DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + sinfo.No + "/";
            string CachePath = AttachmentBLL.GetAttachmentPathTemp();
            if (!Directory.Exists(CachePath))
            {
                Directory.CreateDirectory(CachePath);
            }
            string TemplatePath=HttpContext.Current.Server.MapPath("~/page/sys/Template_" + TemplageID + ".docx");
            File.Copy(TemplatePath,CachePath+FileName);
            using (WordHelper wordApp = new WordHelper())
            {
                try
                {

                    wordApp.OpenDocument(CachePath + FileName, false,true);

                    wordApp.Replace(string.Format(PrintChar, "No"), cinfo.No + "_" + MaxSln1Count);
                    wordApp.Replace(string.Format(PrintChar, "ErrorDate"), cinfo.ErrorDate.ToString("yyyy-MM-dd HH:mm"));
                    wordApp.Replace(string.Format(PrintChar, "DropInDate"), StepJobCode.DateBegin.ToString("yyyy-MM-dd HH:mm"));
                    wordApp.Replace(string.Format(PrintChar, "JOBCODE"), string.Empty);//StepJobCode.StepName
                    wordApp.Replace(string.Format(PrintChar, "CustomerName"), cinfo.StoreNo + "__" + cinfo.BrandName);
                    wordApp.Replace(string.Format(PrintChar, "ErrorReportUser"), cinfo.ErrorReportUser);
                    wordApp.Replace(string.Format(PrintChar, "Tel"), sinfo.Tel);
                    wordApp.Replace(string.Format(PrintChar, "Address"), sinfo.Address);
                    wordApp.Replace(string.Format(PrintChar, "MaintaimUserName"), cinfo.MaintaimUserName);
                    wordApp.Replace(string.Format(PrintChar, "Details"), cinfo.Details.Length > 255 ? cinfo.Details.Substring(0, 255) : cinfo.Details);
                    wordApp.Replace(string.Format(PrintChar, "Sugesstion"), StepJobCode.Details.Length > 255 ? StepJobCode.Details.Substring(0, 255) : StepJobCode.Details);

                    //string ForReplace = string.Format(PrintChar, "Details");
                    //for (int i = 0; i < cinfo.Details.Length; i = (i + 200))
                    //{
                    //    HttpContext.Current.Response.Write("<br/>one:" + i);
                    //    HttpContext.Current.Response.Flush();
                    //    int EndLength = (i + 200) > cinfo.Details.Length ? cinfo.Details.Length : i + 200;
                    //    wordApp.Replace(ForReplace, cinfo.Details.Substring(i, EndLength) + ForReplace);
                    //}
                    //wordApp.Replace(ForReplace, "");//最后才把要替换的内容清掉

                    //ForReplace = string.Format(PrintChar, "Sugesstion");
                    //for (int i = 0; i < StepJobCode.Details.Length; i = (i + 200))
                    //{
                    //    HttpContext.Current.Response.Write("<br/>tow:" + i);
                    //    HttpContext.Current.Response.Flush();
                    //    int EndLength = (i + 200) > StepJobCode.Details.Length ? StepJobCode.Details.Length : i + 200;
                    //    wordApp.Replace(ForReplace, StepJobCode.Details.Substring(i, EndLength) + ForReplace);
                    //}
                    //wordApp.Replace(ForReplace, "");//最后才把要替换的内容清掉

                    wordApp.Replace(string.Format(PrintChar, "现场工程师"), StepJobCode.UserName);
                    wordApp.Replace(string.Format(PrintChar, "No"), cinfo.No);

                    wordApp.SaveAs(FileName, FileDir);
                    wordApp.Close(false);
                    wordApp.Dispose();

                }
                catch (Exception ex)
                {
                    wordApp.Close(false);
                    return "Error on:" + ex;
                }
                finally
                {
                    //if (File.Exists(CachePath + FileName))
                    //{
                    //    File.Delete(CachePath + FileName);
                    //}
                    wordApp.Dispose();
                    GC.Collect();
                }
            }



            string FileUlr = FileDir.Replace("~", "").Replace("\\", "/") + FileName;
            return FileUlr;
        }



        /// <summary>
        /// 生成服务单，并做记录
        /// </summary>
        /// <param name="cinfo">CallsInfo</param>
        /// <param name="WorkDetails"></param>
        /// <param name="StepJobCode">上门准备那一步记录</param>
        /// <param name="TemplageID">使用模板</param>
        /// <returns>返回生成的服务单地址</returns> 
        public static string ReduceBill(CallInfo cinfo, CallStepInfo StepJobCode, int TemplageID)
        {
            string Url = WriteBillWord(cinfo, StepJobCode, TemplageID);
            if (string.IsNullOrEmpty(Url))
            {
                return string.Empty;
            }
            BillRecInfo binfo = new BillRecInfo();
            binfo.AddDate = DateTime.Now;
            binfo.CallID = cinfo.ID;
            binfo.CallNo = cinfo.No;
            binfo.CallStepID = StepJobCode.ID;
            binfo.Confirm = false;
            binfo.CreateBy = UserBLL.GetCurrentEmployeeName();
            binfo.Flag = 0;
            binfo.Pwd = Md5Helper.Md5(DateTime.Now.ToString());
            binfo.StoreID = cinfo.StoreID;
            binfo.Url = Url;
            if (Add(binfo) > 0)
            {
                return Url;
            }
            return string.Empty;
        }
    }
}