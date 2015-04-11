using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Collections;


public partial class page_call_slnDropIn : _Call_Step
{
    int[] RightState = new int[] {
        (int)SysEnum.CallStateDetails.电话支持,
        (int)SysEnum.CallStateDetails.二线离场确认,
        (int)SysEnum.CallStateDetails.开始处理,
        (int)SysEnum.CallStateDetails.上门取消,
        (int)SysEnum.CallStateDetails.升级到客户,
         (int)SysEnum.CallStateDetails.第三方预约取消,
        (int)SysEnum.CallStateDetails.第三方处理离场
   };

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxbDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            CallInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("数据有误。");
            }
            CallState1.CallID = info.ID;
            if (info.StateMain == (int)SysEnum.CallStateMain.已完成)
            {
                Function.AlertBack("数据有误，无法处理已完成的call");
                return;
            }
            #region 状态跳转
            if (!RightState.Contains(info.StateDetail))
            {
                Function.AlertBack("数据状态有误");
                return;
            }
            #endregion


            #region 用户检查
            NotMyCallCheck(info.ID);
            #endregion

        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        ViewState_Clean();
        CallInfo cinfo = GetInfo();
        if (!RightState.Contains(cinfo.StateDetail))
        {
            LogInfo linfo = new LogInfo();
            linfo.AddDate = DateTime.Now;
            linfo.Category = "临时监测数据";
            linfo.Content = string.Format("出现了不在状态的数据info.ID={0}; info.StateDetail={1}", cinfo.ID, cinfo.StateDetail);
            linfo.ErrorDate = DateTime.Now;
            linfo.SendEmail = false;
            linfo.Serious = 1;
            linfo.UserName = "小林测试";
            LogBLL.Add(linfo);
            Function.AlertMsg("数据失效，请刷新，管理员已经发现并得到这个问题");
            return;
            
        }

        

        CallStepInfo sinfo = new CallStepInfo();
        sinfo.StepType = (int)SysEnum.StepType.上门准备;
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName = CurrentUserName;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = "";
        sinfo.UserID = CurrentUserID;
        sinfo.UserName = CurrentUserName;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.DateEnd = sinfo.DateBegin = Function.ConverToDateTime(TxbDate.Text.Trim());
        sinfo.Details =Function.RemoveHTML(TxbHardWare.Text.Trim());

        if (sinfo.DateBegin == Tool.Function.ErrorDate)
        {
            Function.AlertMsg("请认真填写预约上门时间");
            return;
        }

        sinfo.IsSolved = false;
        if (CbHardWare.Checked)
        {
            sinfo.StepName = SysEnum.CallStateDetails.等待安排上门.ToString();
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.等待安排上门;
            cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
        }
        else
        {
            sinfo.StepName = SysEnum.CallStateDetails.等待备件.ToString();
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.等待备件;
            cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
        }
        if (sinfo.DateBegin == Tool.Function.ErrorDate)
        {
            Function.AlertMsg("请认真填写上门时间");
            return;
        }

        UpdateData(cinfo, sinfo);
        BtnSubmit.Visible = false;

    }

    private void ViewState_Clean()
    {
        ViewState["info"] = null;
    }

    private CallInfo GetInfo()
    {
        if (ViewState["info"] != null)
        {
            return (CallInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("id");
        if (ID <= 0)
        {
            return null;
        }
        CallInfo info = CallBLL.Get(ID);
        if (null == info)
        {
            return null;
        }
        ViewState["info"] = info;
        return info;
    }
    private void UpdateData(CallInfo cinfo, CallStepInfo sinfo)
    {
        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {
           // ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", "alert('成功记录');location.href='list.aspx?state=2';", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", "alert('成功记录');location.href='sln.aspx?id="+cinfo.ID+"';", true);
            return;
        }
        else
        {
            Function.AlertMsg("处理出错。请联系管理员");
            return;
        }
    }

    protected void CbHardWare_CheckedChanged(object sender, EventArgs e)
    {
        //TR_Hardware.Visible = !CbHardWare.Checked;
    }
}
