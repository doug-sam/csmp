using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_FeedbackChoose_Edit : _Feedback
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            FeedbackChooseInfo info = GetInfo();
            if (info == null)
            {

                Function.AlertBack("参数有误"); return;
            }
            TxbName.Text = info.Name;
            
            TxbOrderNumber.Text = info.OrderNumber.ToString();
            CbEnable.Checked = info.Enable;
        }
    }

    private FeedbackChooseInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (FeedbackChooseInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        FeedbackChooseInfo info = FeedbackChooseBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {


        if (!DataValidator.IsLen(TxbName.Text.Trim(), UpdatePanel1, this.GetType(), "选项", 2, 50))
        {
            return;
        }
        if (!DataValidator.IsNumber(TxbOrderNumber.Text.Trim(), UpdatePanel1, GetType(), "OrderNumber", 0, 999))
        {
            return;
        }
        FeedbackChooseInfo info = GetInfo();
        if (info == null)
        {
            info = new FeedbackChooseInfo();
        }
        info.Name = TxbName.Text.Trim();
        info.OrderNumber = Function.ConverToInt(TxbOrderNumber.Text.Trim(), 0);
        info.Enable = CbEnable.Checked;


        if (FeedbackChooseBLL.Edit(info))
        {

            Function.AlertRefresh(LtlAction.Text + "成功", "main");
        }
        else
        {
            Function.AlertMsg(LtlAction.Text + "失败");
        }
    }
}
