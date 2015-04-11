using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Replacement_Edit : _Call_StepReplacement
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
            List<string> listSerialNo = ReplacementBLL.GetSerialNo(info.ID);

            TxbDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            DdlUser.DataSource = UserBLL.GetList(CurrentUser.WorkGroupID, string.Empty);
            DdlUser.DataBind();
            DdlUser.Items.Insert(0, new ListItem("请选择", "0"));

            DdlUser.SelectedValue = CurrentUser.ID.ToString();

            DdlSerialNo.DataSource = listSerialNo;
            DdlSerialNo.DataBind();
            DdlSerialNo.Items.Insert(0, new ListItem("请选择","0"));

            if (listSerialNo.Count<=0)
            {
                BtnReplacement_Click(null, null);
            }

            NotMyCallCheck(info);

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



    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        CallInfo cinfo = GetInfo();
        CallInfo infoTheNew = CallBLL.Get(cinfo.ID);

        ReplacementInfo info = new ReplacementInfo();
        info.CallID = cinfo.ID;
        info.StateID = CbFinish.Checked ? (int)SysEnum.ReplacementStatus.处理完成 : (int)SysEnum.ReplacementStatus.备件跟进中;
        info.StateName = CbFinish.Checked ? SysEnum.ReplacementStatus.处理完成.ToString() : SysEnum.ReplacementStatus.备件跟进中.ToString();
        info.MaintainUserID = Function.ConverToInt(DdlUser.SelectedValue, 0);
        info.MaintainUserName = string.Empty;
        info.RecordUserID = CurrentUser.ID;
        info.RecordUserName = CurrentUser.Name;
        info.DateAction = Function.ConverToDateTime(TxbDate.Text.Trim());
        info.DateAdd = DateTime.Now;
        info.Detail = TxbDetails.Text;
        info.RpBrand = string.Empty;
        info.RpMode = string.Empty;
        info.RpSerialNo=DdlSerialNo.Visible?DdlSerialNo.SelectedValue: TxbSerialNo.Text.Trim();

        UserInfo userMaintainUser = UserBLL.Get(info.MaintainUserID);
        if (null==userMaintainUser||userMaintainUser.WorkGroupID!=CurrentUser.WorkGroupID)
        {
            Function.AlertMsg("请选择负责人"); return;
        }
        info.MaintainUserName = userMaintainUser.Name;
        if (info.DateAction<=DicInfo.DateZone)
        {
            Function.AlertMsg("事发日期不对劲啊"); return;
        }
        if (string.IsNullOrEmpty(info.RpSerialNo)||info.RpSerialNo.Length>100||info.RpSerialNo=="0")
        {
            Function.AlertMsg("备件不能为空。也不能超过100字"); return;
        }

        if (ReplacementBLL.Add(info)>0)
        {
            cinfo.ReplacementStatus =(int)(CbFinish.Checked ? SysEnum.ReplacementStatus.处理完成 : SysEnum.ReplacementStatus.备件跟进中);
            CallBLL.Edit(cinfo);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "success action", "alert('提交成功');parent.tb_remove();parent.location.reload();", true);
        }
        else
        {
            Function.AlertMsg("提交失败。请联系管理员"); return;
        }

    }
    protected void BtnReplacement_Click(object sender, EventArgs e)
    {
        if (DdlSerialNo.Visible)
        {
            DdlSerialNo.Visible = false;
            TxbSerialNo.Visible = true;
            BtnReplacement.Text = "选择已有备件进行跟进>>"; 
        }
        else
        {
            DdlSerialNo.Visible = true;
            TxbSerialNo.Visible = false;
            BtnReplacement.Text = "新备件记录>>";
        }
    }
}
