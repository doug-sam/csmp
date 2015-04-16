using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_call_view : _Call_list
{
    public CallInfo info;
    public CallStepInfo callbackStepInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            Function.AlertBack("数据有误 no id");
            return;
        }
        info = CallBLL.Get(ID);
        if (null == info)
        {
            Function.AlertBack("数据有误 null in db");
            return;
        }
        callbackStepInfo = null;
        List<CallStepInfo> stepInfoList = CallStepBLL.GetListJoin(info);
        foreach (CallStepInfo stepInfo in stepInfoList)
        {
            if (stepInfo.StepName.TrimEnd() == "回访")
            {
                callbackStepInfo = stepInfo;
                break;
            }
        }
        View1.BindData(info);
        if (!IsPostBack)
        {

            tr_Trace.Visible = CallBLL.EnableTrace(info,CurrentUser);
            HlHistory.NavigateUrl = string.Format("/page/call/sch.aspx?StoreNo={0}",info.StoreName);

            string UrlDead = ProfileBLL.GetValue(ProfileInfo.UserKey.电话服务器录音根地址);

            DateTime useNewCCDate = new DateTime(2015, 4, 8);
            if (info.CreateDate >= useNewCCDate)
                RecordPlay1.UseOldRecordDB = false;
            else
                RecordPlay1.UseOldRecordDB = true;
            RecordPlay2.UseOldRecordDB = false;
            if (!string.IsNullOrEmpty(info.VideoID))
            {
                RecordPlay1.info = info;
                RecordPlay1.IsIncommingRecord = true;
            }
            else
            {
                RecordPlay1.info = null;
                RecordPlay1.stepinfo = null;
            }
            if (null != callbackStepInfo && !string.IsNullOrEmpty(callbackStepInfo.Details))
            {
                RecordPlay2.info = info;
                RecordPlay2.stepinfo = callbackStepInfo;
                RecordPlay2.IsIncommingRecord = false;
            }
            else
            {
                RecordPlay2.info = info;
                RecordPlay2.stepinfo = null;
            }
        }

    }
}
