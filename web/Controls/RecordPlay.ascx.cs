using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Tool;
using CSMP.Model;
using CSMP.BLL;
using System.IO;
using System.Net;


public partial class Controls_RecordPlay : System.Web.UI.UserControl
{
    private const string DeadPath = "/file/download/wav/";
    public CallInfo info;
    public CallStepInfo stepinfo;
    public bool UseOldRecordDB;
    public bool IsIncommingRecord;

    protected void Page_Load(object sender, EventArgs e)
    {
        info = GetInfo();
        if (null == info)
        {
            Panel1.Visible = false;
            Panel2.Visible = true;
            return;
        }
        ViewState["info"] = info;
        DeleteOldFile();

        //LtlSrc1.Text = LtlSrc2.Text = DownLoad(info);
        if (UseOldRecordDB)
            hdsrc.Value = DownLoad(info);
        else
        {
            string recordFileUrl = "";
            if (IsIncommingRecord)
            {
                recordFileUrl = GetRecordURL(info);
                if (String.IsNullOrEmpty(recordFileUrl))
                {
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                }
                hdsrc.Value = DownLoadEx("http:" + recordFileUrl, "01");
            }
            else if (stepinfo != null)
            {
                recordFileUrl = GetCallbackRecordURL(stepinfo);
                hdsrc.Value = DownLoadEx("http:" + recordFileUrl, "03");
            }
            if (String.IsNullOrEmpty(recordFileUrl))
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
            }
        }
        StoreInfo sinfo = StoresBLL.Get(info.StoreID);
        if (null == sinfo)
        {
            Function.AlertBack("数据有误，找不到店铺数据");
            return;
            //Response.End();
        }
    }
    private  CallInfo GetInfo()
    {
        if (null!=info)
        {
            return info;
        }
        int CallID = Function.GetRequestInt("CallID");
        info = CallBLL.Get(CallID);
        return info;
    }
    public string DownLoad(CallInfo info)
    {
        string DeadUrl = ProfileBLL.GetValue(ProfileInfo.UserKey.电话服务器录音根地址, true);
        RecordInfo recinfo = null;
        try
        {
            recinfo = RecordBLL.Get(info.VideoID);
        }
        catch (Exception ex)
        {
           // throw ex;
            WriteLog("", "", String.Format("{0}    VideoID is:{1}.", ex.Message, info.VideoID));
            recinfo = null;
        }
        if (null == recinfo)
        {
            WriteLog("", "", String.Format("Cannot find record in db.    VideoID is:{0}.", info.VideoID));
            return string.Empty;
            //Response.End();
        }
        string Url = DeadUrl.Replace("recording_file_path", recinfo.filepath).Replace("recording_file_name", recinfo.recordname);
        string SavePath = DeadPath + DateTime.Now.ToString("yyyy-MM-dd") + "/";
        string SaveMapPath = Server.MapPath("~" + SavePath);
        if (!Directory.Exists(SaveMapPath))
        {
            Directory.CreateDirectory(SaveMapPath);
        }
        string SaveName =  ".wav";
        if (ViewState["info"]!=null)
        {
            SaveName = ((CallInfo)ViewState["info"]).ID + SaveName;
        }
        else if (Function.GetRequestInt("CallID")>0)
        {
            SaveName = Function.GetRequestInt("CallID") + SaveName;
        }
        else
        {
            SaveName = Function.GetRand() + SaveName;
        }
        WebClient myWebClient = new WebClient();
        try
        {
            myWebClient.DownloadFile(Url, SaveMapPath + SaveName);
        }
        catch (Exception ex)
        {
            //Function.AlertMsg("系统目前无法找到录音文件。");
            WriteLog(Url, SaveMapPath + SaveName, String.Format("{0}    VideoID is:{1}.", ex.Message, info.VideoID));
            return string.Empty;
            // throw;
        }

        return SavePath + SaveName;
    }
    /// <summary>
    /// 与2015年5月份新的呼叫中心系统匹配。连接新的录音服务器。
    /// recfiletype。每个call将对应三个录音文件。所以下载到网站目录下不能再以一个单一的call的标识为文件名，必须再加点其他内容。
    ///             01:呼入;    02:开始处理外呼门店;    03:回访。
    /// </summary>
    public string DownLoadEx(string recfilepath, string recfiletype)
    {
        string SavePath = DeadPath + DateTime.Now.ToString("yyyy-MM-dd") + "/";
        string SaveMapPath = Server.MapPath("~" + SavePath);
        if (!Directory.Exists(SaveMapPath))
        {
            Directory.CreateDirectory(SaveMapPath);
        }
        string SaveName = ".wav";
        if (ViewState["info"] != null)
        {
            SaveName = ((CallInfo)ViewState["info"]).ID + recfiletype + SaveName;
        }
        else if (Function.GetRequestInt("CallID") > 0)
        {
            SaveName = Function.GetRequestInt("CallID") + recfiletype + SaveName;
        }
        else
        {
            SaveName = Function.GetRand() + SaveName;
        }
        WebClient myWebClient = new WebClient();
        try
        {
            myWebClient.DownloadFile(recfilepath.Replace("\\", "/"), SaveMapPath + SaveName);
        }
        catch (Exception e)
        {
            WriteLog(recfilepath, SaveMapPath + SaveName, e.Message);
            return string.Empty;
        }

        return SavePath + SaveName;
    }
    public string GetRecordURL(CallInfo info)
    {
        RecordInfo recinfo = null;
        try
        {
            recinfo = RecordBLL.GetRecordInfo(info.VideoID);
        }
        catch (Exception ex)
        {
            recinfo = null;
        }
        if (null == recinfo)
            return string.Empty;
        return recinfo.filepath;
    }
    public string GetCallbackRecordURL(CallStepInfo stepinfo)
    {
        RecordInfo recinfo = null;
        string recordid = "";
        int POS1 = stepinfo.Details.IndexOf("A$B$C");
        int POS2 = stepinfo.Details.IndexOf("D$E$F");
        if (POS1 != -1 && POS2 != -1 && POS2 > POS1)
            recordid = stepinfo.Details.Substring(POS1 + 5, POS2 - POS1 - 5);
        if (string.IsNullOrEmpty(recordid))
            return string.Empty;
        try
        {
            recinfo = RecordBLL.GetRecordInfo(recordid);
        }
        catch (Exception ex)
        {
            recinfo = null;
        }
        if (null == recinfo)
            return string.Empty;
        return recinfo.filepath;
    }
    /// <summary>
    /// 每次看录音。系统都会删除两日前的目录
    /// </summary>
    private void DeleteOldFile()
    {
        //try
        //{
        //    if (!Directory.Exists(Server.MapPath(DeadPath)))
        //    {
        //        Directory.CreateDirectory(Server.MapPath(DeadPath));
        //    }
        //    foreach (string item in Directory.GetDirectories(Server.MapPath(DeadPath)))
        //    {
        //        if (item != DateTime.Now.ToString("yyyy-MM-dd") && item != DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"))
        //        {
        //            Directory.Delete(item, true);
        //        }
        //    }
        //}
        //catch (Exception)
        //{
        //    Function.AlertMsg("系统繁忙，请重试 。");
        //}
        
    }

    protected void WriteLog(string originalurl, string localpath, string cause)
    {
        LogInfo linfo = new LogInfo();
        linfo.AddDate = DateTime.Now;
        linfo.Category = SysEnum.LogType.普通日志.ToString();
        linfo.Content = string.Format("下载录音文件失败  网络路径：{0}  本地路径：{1}  原因：{2}", originalurl, localpath, cause);
        linfo.ErrorDate = DateTime.Now;
        linfo.SendEmail = false;
        linfo.Serious = 1;
        linfo.UserName = "admin";
        LogBLL.Add(linfo);

    }


}
