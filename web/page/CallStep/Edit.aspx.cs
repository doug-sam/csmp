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


public partial class page_callStep_Edit : _CallStep_Edit
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CallStepInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("数据有误。");
            }
            CallInfo cinfo=CallBLL.Get(info.CallID);
            if (!CallStepEnableEdit(cinfo))
            {
                GroupBLL.EnterCheck(false); return;
            }
            if (info.StepType!=(int)SysEnum.StepType.上门安排)
            {
                Function.AlertBack("只允许编辑上门安排记录"); return;
            }
            LabStepType.Text = Enum.GetName(typeof(SysEnum.StepType), info.StepType);
            LabAddDate.Text = info.AddDate.ToString("yyyy-MM-dd HH:mm");
            TxbDetail.Text = info.Details;

        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TxbDetail.Text.Trim()))
        {
            Function.AlertMsg(LabDetail.Text+"不能为空"); return;
        }
        if (TxbDetail.Text.Trim().Length>500)
        {
            Function.AlertMsg(LabDetail.Text+"不能超过500字"); return;
        }

        CallStepInfo info = GetInfo();
        info.Details = TxbDetail.Text.Trim();
        if (CallStepBLL.Edit(info))
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "", "alert('编辑成功！');parent.tb.remove();", true);
        }
        else
        {
            Function.AlertBack("编辑失败");
        }
    }

    private CallStepInfo GetInfo()
    {
        if (ViewState["info"] != null)
        {
            return (CallStepInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("id");
        if (ID <= 0)
        {
            return null;
        }
        CallStepInfo info = CallStepBLL.Get(ID);
        if (null == info)
        {
            return null;
        }
        ViewState["info"] = info;
        return info;
    }
}
