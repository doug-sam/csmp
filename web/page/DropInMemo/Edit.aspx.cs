using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_DropInMemo_Edit : _Call_Step
{
    int[] RightState = new int[] {
        (int)SysEnum.StepType.到达门店处理,
        (int)SysEnum.StepType.上门安排
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        CallStepInfo sinfo = GetStepInfo();
        if (null==sinfo)
        {
            Function.AlertBack("参数有误,操作步骤为空");
        }
        if (!RightState.Contains(sinfo.StepType))
        {
            Function.AlertBack("参数有误,步骤状态不对");
        }
        DropInMemoInfo info= GetInfo();
        if (null!=info)
        {
            TxbDetails.Text = info.Details;
            TxbMemoDate.Text = info.MemoDate.ToString("yyyy-MM-dd HH:mm");
        }
        else
        {
            TxbMemoDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

    }

    private CallStepInfo GetStepInfo()
    {
        CallStepInfo info;
        if (ViewState["sinfo"] != null)
        {
            info = (CallStepInfo)ViewState["sinfo"];
        }
        int ID = Function.GetRequestInt("StepID");
        if (ID > 0)
        {
            info = CallStepBLL.Get(ID);
            if (null != info)
            {
                ViewState["sinfo"] = info;
            }
            return info;
        }
        return null;
    }
    private DropInMemoInfo GetInfo()
    {
        DropInMemoInfo info;
        if (ViewState["info"] != null)
        {
            info = (DropInMemoInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = DropInMemoBLL.Get(ID);
            if (null != info)
            {
                ViewState["info"] = info;
            }
            return info;
        }
        return null;
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        DropInMemoInfo info = GetInfo();
        if (null == info)
        {
            info = new DropInMemoInfo();
        }
        if (string.IsNullOrEmpty(TxbDetails.Text.Trim())||TxbDetails.Text.Trim().Length>500)
        {
            Function.AlertMsg("备注不能为空。"); return;
        }
        if (Convert.ToDateTime(TxbMemoDate.Text) == Function.ErrorDate)
        {
            Function.AlertMsg("日期有错。"); return;
        }
        info.AddDate = DateTime.Now;
        info.Details = TxbDetails.Text.Trim();
        info.Enable = true;
        info.OrderNumber = 0;
        info.StepID = GetStepInfo().ID;
        info.UserID = CurrentUser.ID;
        info.UserName = CurrentUser.Name;
        info.MemoDate = Convert.ToDateTime(TxbMemoDate.Text.Trim());
        info.TypeName = GetStepInfo().StepName;
        if (GetInfo() == null)
        {
            if (DropInMemoBLL.Add(info) > 0)
            {
                Function.AlertRefresh("添加成功","main");
            }
            else
            {
                Function.AlertMsg("添加失败");
            }
        }
        else
        {
            if (DropInMemoBLL.Edit(info))
            {
                Function.AlertRefresh("修改成功", "main");
            }
            else
            {
                Function.AlertMsg("修改失败");
            }
        }

    }


}
