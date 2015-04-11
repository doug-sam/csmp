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


public partial class page_Trace_Trace : _Call_Trace
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CallInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("数据有误。");
            }
            if (!CallBLL.EnableTrace(info, CurrentUser))
            {
                Function.AlertBack("无权访问");
            }

            if (!CallBLL.EnableTrace(info, CurrentUser))
            {
                GroupBLL.EnterCheck(false);
            }
            TxbDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (Function.ConverToDateTime(TxbDate.Text.Trim()) == Function.ErrorDate)
        {
            Function.AlertBack("催促日期有误"); return;
        }
        if (TxbName.Text.Trim().Length>50)
        {
            Function.AlertBack("催促人名过长"); return;
        }

        CallInfo cinfo = GetInfo();
        CallStepInfo sinfo = new CallStepInfo();
        sinfo.StepType = (int)SysEnum.StepType.店铺催促;
        sinfo.StepName = SysEnum.StepType.店铺催促.ToString();
        sinfo.MajorUserID = 0;
        sinfo.MajorUserName =TxbName.Text.Trim();
        sinfo.SolutionID = 0;
        sinfo.SolutionName = "";
        sinfo.UserID = CurrentUserID;
        sinfo.UserName = CurrentUserName;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.DateEnd = sinfo.DateBegin = Function.ConverToDateTime(TxbDate.Text.Trim());
        sinfo.Details = TxbDetail.Text.Trim();
        sinfo.IsSolved = false;
        if (CallStepBLL.Add(sinfo) > 0)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", "alert('成功记录');parent.tb_remove();", true);
            return;
        }
        else
        {
            Function.AlertMsg("处理出错。请联系管理员");
            return;
        }
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
}
