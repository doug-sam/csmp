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
            info = CallBLL.Get(CallID);
            if (null == info)
            {
                Function.AlertBack("数据有误 null in db");
                return;
            }
            callbackStepInfo = null;
            List<CallStepInfo> stepInfoList = CallStepBLL.GetListJoin(info);
            foreach (CallStepInfo stepInfo in stepInfoList)
            {
                //if (stepInfo.StepIndex == 1)
                //{
                //    callbackStepInfo = stepInfo;
                //    break;
                //}
                if (stepInfo.StepIndex == 1 && stepInfo.Details.Contains("A$B$C") && stepInfo.Details.Contains("D$E$F"))
                {
                    if (RecordID.ToString() == GetRecordIDFromDetails(stepInfo.Details))
                    {
                        callbackStepInfo = stepInfo;
                        break;
                    }
                    
                }
                if (stepInfo.StepIndex == 2 && stepInfo.Details.Contains("A$B$C") && stepInfo.Details.Contains("D$E$F"))
                {
                    if (RecordID.ToString() == GetRecordIDFromDetails(stepInfo.Details))
                    {
                        callbackStepInfo = stepInfo;
                        break;
                    }
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
    protected string GetRecordIDFromDetails(string Details)
    {
        string recordid = "";
        int POS1 = Details.IndexOf("A$B$C");
        int POS2 = Details.IndexOf("D$E$F");
        if (POS1 != -1 && POS2 != -1 && POS2 > POS1)
            recordid = Details.Substring(POS1 + 5, POS2 - POS1 - 5);
        return recordid;
    }
}
