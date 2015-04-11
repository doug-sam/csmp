using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_CallStep_ReciveBill : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string CallNo = Function.GetRequestSrtring("CallNo");
        string Raid = Function.GetRequestSrtring("raid");
        if (string.IsNullOrEmpty(CallNo) || string.IsNullOrEmpty(Raid))
        {
            Response.End(); return;
        }
        BillRecInfo info = BillRecBLL.Get(CallNo, Raid);
        if (null == info)
        {
            Response.End(); return;
        }
        if (info.Confirm)
        {
            Function.AlertBack("亲，\n\n这个服务单已经被确认收到过了！\n\n如果那不曾是你确认的，\n\n请联系管理员！"); return;
        }
        CallInfo cinfo = CallBLL.Get(info.CallID);
        if (null == cinfo)
        {
            Function.AlertBack("亲，\n\n这个call已经被删除了！\n\n可能不用你干了吧！"); return;
        }
        if (cinfo.StateMain!=(int)SysEnum.CallStateMain.处理中)
        {
            Function.AlertBack("亲，这个call已经不是在处理中了。"); return;
        }
        if (cinfo.StateDetail!=(int)SysEnum.CallStateDetails.等待工程师上门&&cinfo.StateDetail!=(int)SysEnum.CallStateDetails.等待第三方响应)
        {
            Function.AlertBack("亲，这个call现在不需要你确认收到服务单了\n\n。"); return;
        }

        CallStepInfo StepJobCode = CallStepBLL.Get(info.CallStepID);
        if (null == StepJobCode)
        {
            Function.AlertBack("该call上门服务已被删除。"); return;
        }
        CallStepInfo stinfo = new CallStepInfo();
        stinfo.AddDate = DateTime.Now;
        stinfo.CallID = info.CallID;
        stinfo.DateBegin = DateTime.Now;
        stinfo.DateEnd = DateTime.Now;
        stinfo.Details = "上门工程师确认收到服务单";
        stinfo.IsSolved = false;
        stinfo.MajorUserID = StepJobCode.MajorUserID;
        stinfo.MajorUserName = StepJobCode.MajorUserName;
        stinfo.SolutionID = 0;
        stinfo.SolutionName = "";
        stinfo.StepIndex = CallStepBLL.GetMaxStepIndex(info.CallID) + 1;
        stinfo.StepName = SysEnum.CallStateDetails.工程师接收服务单.ToString();
        stinfo.StepType = (int)SysEnum.StepType.工程师接收服务单;
        stinfo.UserID = stinfo.MajorUserID;
        stinfo.UserName = stinfo.MajorUserName;

        cinfo.StateDetail = (int)SysEnum.CallStateDetails.工程师接收服务单;
        if (CallStepBLL.AddCallStep_UpdateCall(cinfo,stinfo))
        {
            string Msg = "提交成功。我们得知你已收到服务单。";
            info.Confirm = true;
            if (!BillRecBLL.Edit(info))
            {
                Msg += "但你的确认信息未全部提交成功。请联系管理员";
            }
            Response.Write(Msg);
        }

    }
}
