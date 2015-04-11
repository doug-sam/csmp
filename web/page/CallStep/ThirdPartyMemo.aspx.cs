using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_ThirdPartyMemo_Edit : _Call_Step
{
    int[] RightState = new int[] {
        (int)SysEnum.CallStateDetails.第三方预约上门
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxbMemoDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            CallInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("参数有误,操作步骤为空");
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

    public CallInfo GetInfo()
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


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        CallInfo info = GetInfo();
        CallStepInfo sinfo = new CallStepInfo();
        sinfo.StepType = (int)SysEnum.StepType.第三方上门备注;
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName = CurrentUserName;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = info.ID;
        sinfo.DateBegin = Function.ConverToDateTime(TxbMemoDate.Text.Trim());
        sinfo.DateEnd = sinfo.DateBegin;
        sinfo.Details = TxbDetails.Text.Trim();
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(info.ID) + 1;

        sinfo.IsSolved = false;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = "";

        sinfo.StepName = SysEnum.StepType.第三方上门备注.ToString();
        sinfo.UserID = CurrentUser.ID;
        sinfo.UserName = CurrentUser.Name;

        if (CallStepBLL.Add(sinfo) > 0)
        {
            Function.AlertRedirect("添加成功", "/page/call/sln.aspx?ID=" + info.ID, "main");
        }
        else
        {
            Function.AlertMsg("添加失败，请重试或联系管理员");
        }
    }


}
