using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_call_Close : _Call_Close
{
    public CallInfo info;
    protected void Page_Load(object sender, EventArgs e)
    {
        GetInfo();
        NotMyCallCheck(info.ID);
        if (!IsPostBack)
        {
            if (info.StateMain != (int)SysEnum.CallStateMain.已完成)
            {
                Function.AlertBack("该call状态不对。你是怎么进来的？！。");
            }

            TxbNo2.Text = info.No;

            StoreInfo sinfo = StoresBLL.Get(info.StoreID);
            if (null == sinfo)
            {
                Response.End();
            }
            LtlStoreNo.Text = sinfo.No;
            LtlTel.Text = sinfo.Tel;
            LtlAddress.Text = sinfo.Address;

            Class3Info c3info = Class3BLL.Get(info.Class3);
            if (null != c3info)
            {
                LabSLA.Text = c3info.SLA.ToString() + "Hour";
            }

            CallStepInfo stepBill = CallStepBLL.GetLast(info.ID, SysEnum.StepType.回收服务单);
            if (stepBill != null )
            {
                TxbNo2.Visible = false;
                CbClose.Checked = true;
                CbClose.Enabled = false;
            }

        }

    }

    private void GetInfo()
    {
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            Response.End();
        }
        info = CallBLL.Get(ID);
        if (null == info)
        {
            Response.End();
        }
    }

    private string Call_ReciveBill(CallInfo info, string CallNo2)
    {
        CallNo2 = CallNo2.Trim();
        if (null == info)
        {
            return "无法获取数据";
        }
        if (CallNo2.Length > 100)
        {
            return "服务单号不应超过100个字";
        }


        //info.StateMain = (int)SysEnum.CallStateMain.已关闭;
        info.StateDetail = (int)SysEnum.CallStateDetails.已回收服务单;
        info.CallNo2 = CallNo2;


        CallStepInfo sinfo = new CallStepInfo();
        sinfo.CallID = info.ID;
        sinfo.AddDate = DateTime.Now;
        sinfo.DateBegin = sinfo.DateEnd = DateTime.Now;
        sinfo.Details = string.Format("由{0}回收服务单号:{1}", CurrentUserName, TxbNo2.Text.Trim());
        sinfo.IsSolved = false;
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName = CurrentUserName;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = string.Empty;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(info.ID) + 1;
        sinfo.StepName = SysEnum.StepType.回收服务单.ToString();
        sinfo.StepType = (int)SysEnum.StepType.回收服务单;
        sinfo.UserID = CurrentUserID;
        sinfo.UserName = CurrentUserName;
        if (CallStepBLL.AddCallStep_UpdateCall(info, sinfo))
        {
            return string.Empty;
        }
        return "系统已接收服务单，但无法更新状态。这可能由网络原因引起的，请稍候再试。";

    }

    private bool Call_Close(CallInfo info)
    {
        info.StateMain = (int)SysEnum.CallStateMain.已关闭;

        CallStepInfo sinfo = new CallStepInfo();
        sinfo.CallID = info.ID;
        sinfo.AddDate = DateTime.Now;
        sinfo.DateBegin = sinfo.DateEnd = DateTime.Now;
        sinfo.Details = string.Format("由{0}关call", CurrentUserName);
        sinfo.IsSolved = false;
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName = CurrentUserName;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = string.Empty;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(info.ID) + 1;
        sinfo.StepName = SysEnum.StepType.关单.ToString();
        sinfo.StepType = (int)SysEnum.StepType.关单;
        sinfo.UserID = CurrentUserID;
        sinfo.UserName = CurrentUserName;
        bool Result = CallStepBLL.AddCallStep_UpdateCall(info, sinfo);
        return Result;

    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (null == info)
        {
            GetInfo();
        }
        if (CbClose.Checked)
        {
            if (info.ReplacementStatus == (int)SysEnum.ReplacementStatus.备件跟进中)
            {
                Function.AlertMsg("备件状态有误");
                return;
            }
        }
        string Result = string.Empty;
        if (TxbNo2.Visible)
        {
            Result = Call_ReciveBill(info, TxbNo2.Text);
            if (!string.IsNullOrEmpty(Result))
            {
                Function.AlertMsg(Result);
            }
        }

        if (CbClose.Checked)
        {
            if (!Call_Close(info))
            {
                Function.AlertMsg("报修关闭失败了，请重试");
                return;
            }
        }

        Function.AlertRefresh("操作成功", "main");
        return;
    }
}
