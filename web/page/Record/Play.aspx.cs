using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_Attachment_View : _Sys_Attachment
{
    public CallInfo info;
    public CallStepInfo callbackStepInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int CallID = Function.GetRequestInt("ID");
            int RecordID = Function.GetRequestInt("RecID");
            if (CallID <= 0)
            {
                Function.AlertBack("参数有误");
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
                if (stepInfo.StepIndex == 1)
                {
                    callbackStepInfo = stepInfo;
                    break;
                }
            }
            RecordPlay1.UseOldRecordDB = false;
            RecordPlay1.IsIncommingRecord = false;
            RecordPlay1.info = info;
            if (null != callbackStepInfo && !string.IsNullOrEmpty(callbackStepInfo.Details))
                RecordPlay1.stepinfo = callbackStepInfo;
            else                
                RecordPlay1.stepinfo = null;
        }
    }
}
