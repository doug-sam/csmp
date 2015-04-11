using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_FeedbackQuestion_Edit : _Feedback
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlType.DataSource = SysEnum.ToDictionary(typeof(SysEnum.QuestionType));
            DdlType.DataBind();
            DdlType.Items.Insert(0, new ListItem("请选择", "0"));
            DdlPaper.DataSource = FeedbackPaperBLL.GetList();
            DdlPaper.DataBind();
            DdlPaper.Items.Insert(0, new ListItem("请选择", "0"));

            FeedbackQuestionInfo info = GetInfo();
            if (info != null)
            {
                DdlType.Enabled = false;
                TxbName.Text = info.Name;
                DdlType.SelectedValue = info.Type.ToString();
                TxbOrderNumber.Text = info.OrderNumber.ToString();
                CbEnable.Checked = info.Enable;

                if ((info.Type == (int)SysEnum.QuestionType.Check || info.Type == (int)SysEnum.QuestionType.Radio))
                {
                    HlChoose.Visible = true;
                    HlChoose.NavigateUrl = "/page/FeedbackChoose/list.aspx?QuestionID=" + info.ID;
                    GridView1.DataSource = FeedbackChooseBLL.GetList(info.ID);
                    GridView1.DataBind();
                }
            }
            else
            {
                LtlAction.Text = "添加";
            }
        }
    }

    private FeedbackQuestionInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (FeedbackQuestionInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        FeedbackQuestionInfo info = FeedbackQuestionBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {


        if (!DataValidator.IsLen(TxbName.Text.Trim(), UpdatePanel1, this.GetType(), "名称", 2, 500))
        {
            return;
        }
        if (!DataValidator.IsNumber(TxbOrderNumber.Text.Trim(), UpdatePanel1, GetType(), "OrderNumber", 0, 999))
        {
            return;
        }
        FeedbackQuestionInfo info = GetInfo();
        if (info == null)
        {
            info = new FeedbackQuestionInfo();
        }

        info.PaperID = Function.ConverToInt(DdlPaper.SelectedValue);
        info.Name = TxbName.Text.Trim();
        info.Memo = "";
        info.Type = Function.ConverToInt(DdlType.SelectedValue);
        info.OrderNumber = Function.ConverToInt(TxbOrderNumber.Text.Trim(), 0);
        info.Enable = CbEnable.Checked;

        if (info.PaperID <= 0)
        {
            Function.AlertMsg("This Question belong to which Paper?");
            return;
        }
        if (info.Type <= 0)
        {
            Function.AlertMsg("please chose the question type");
            return;
        }

        bool result = false;
        if (ViewState["INFO"] == null)
        {
            result = (FeedbackQuestionBLL.Add(info) > 0);
        }
        else
        {
            result = FeedbackQuestionBLL.Edit(info);
        }

        if (result)
        {

            Function.AlertRefresh(LtlAction.Text + "成功", "main");
        }
        else
        {
            Function.AlertMsg(LtlAction.Text + "失败");
        }
    }
}
