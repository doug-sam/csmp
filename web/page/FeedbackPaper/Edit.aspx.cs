using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_FeedbackPaper_Edit : _Feedback
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            

            FeedbackPaperInfo info = GetInfo();
            if (info != null)
            {
                TxbName.Text = info.Name;
                TxbMemo.Text = info.Memo;
                TxbOrderNumber.Text = info.OrderNumber.ToString();
                CbEnable.Checked = info.Enable;

            }
            else
            {
                LtlAction.Text = "添加";
            }
        }
    }

    private FeedbackPaperInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (FeedbackPaperInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        FeedbackPaperInfo info = FeedbackPaperBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {


        if (!DataValidator.IsLen(TxbName.Text.Trim(), UpdatePanel1, this.GetType(), "名称", 2, 50))
        {
            return;
        }
        if (!DataValidator.IsNumber(TxbOrderNumber.Text.Trim(),UpdatePanel1,GetType(),"OrderNumber",0,999))
        {
            return;
        }
        FeedbackPaperInfo info = GetInfo();
        if (info==null)
        {
            info = new FeedbackPaperInfo();
        }
        info.Name = TxbName.Text.Trim();
        info.Memo = TxbMemo.Text.Trim();
        info.OrderNumber = Function.ConverToInt(TxbOrderNumber.Text.Trim(),0);
        info.Enable = CbEnable.Checked;
        bool result = false;
        if (ViewState["INFO"] == null)
        {
            result = (FeedbackPaperBLL.Add(info) > 0);
        }
        else
        {
            result = FeedbackPaperBLL.Edit(info);
        }

        if (result)
        {
            
            Function.AlertRefresh(LtlAction.Text + "成功","main");   
        }
        else
        {
            Function.AlertMsg(LtlAction.Text + "失败");
        }
    }
}
