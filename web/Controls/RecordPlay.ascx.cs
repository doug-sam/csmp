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
            LtlSrc2.Text = DownLoad(info);
        else
        {
            if ( IsIncommingRecord )
                LtlSrc2.Text = GetRecordURL(info);
            else
                LtlSrc2.Text = GetCallbackRecordURL(stepinfo);
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
            recinfo = null;
        }
        if (null == recinfo)
        {
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
        catch (Exception)
        {
            //Function.AlertMsg("系统目前无法找到录音文件。");
            return string.Empty;
            // throw;
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


}
