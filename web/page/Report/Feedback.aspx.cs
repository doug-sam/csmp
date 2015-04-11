using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Text;
using System.Data;

public partial class page_Report_Feedback : _Report_Report
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxtDateBegin.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            TxbDateEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));
            DdlPaper.DataSource = FeedbackPaperBLL.GetList();
            DdlPaper.DataBind();
            DdlPaper.Items.Insert(0, new ListItem("请选择", "0"));
        }
    }


    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        DateTime dBegin = Function.ConverToDateTime(TxtDateBegin.Text.Trim());
        DateTime dEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        int PaperID = Function.ConverToInt(DdlPaper.SelectedValue);
        int QuestionID = Function.ConverToInt(DdlQuestion.SelectedValue);
        int CustomerID=Function.ConverToInt(DdlCustomer.SelectedValue);
        int BrandID=Function.ConverToInt(DdlBrand.SelectedValue);
        #region Validator

        if (dBegin == Function.ErrorDate)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('开始日期填写有误');", true);
            return;
        }
        if (dEnd == Function.ErrorDate)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('结束日期填写有误');", true);
            return;
        }

        if (dBegin >= dEnd)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('开始日期不能大于结束日期');", true);
            return;
        }
        if (dEnd > DateTime.Now.Date)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('结束日期不能大于今天');", true);
            return;
        }
        if (PaperID<=0)
        {
            Function.AlertMsg("请选择问卷"); return;
        }
        if (QuestionID<=0)
        {
            Function.AlertMsg("请选择问题"); return;
        }

        #endregion

        DataTable dt = StatBLL.FeedBackRadio(CustomerID, BrandID, dBegin, dEnd, QuestionID);
        List<FeedbackChooseInfo> listchoose = FeedbackChooseBLL.GetList(QuestionID);
        foreach (FeedbackChooseInfo item in listchoose)
        {
            item.OrderNumber = 0;
            foreach (DataRow itemRow in dt.Rows)
            {
                if (itemRow["ID"].ToString()==item.ID.ToString())
                {
                    item.OrderNumber = Function.ConverToInt(itemRow["f_Count"], 0);
                    break;
                }
            }
        }


        StringBuilder sbFoot = new StringBuilder();
        StringBuilder sbData = new StringBuilder();
        foreach (FeedbackChooseInfo item in listchoose)
        {
            sbFoot.Append("'").Append(item.Name).Append("',");
            sbData.Append(item.OrderNumber).Append(",");
        }

        LtlFootTxt.Text = sbFoot.ToString().Trim(',');
        LtlPrecent.Text = sbData.ToString().Trim(',');
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "Draw();", true);

    }

    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID > 0)
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID);
            DdlBrand.DataBind();
        }
        else
        {
            DdlBrand.DataSource = null;
            DdlBrand.DataBind();
        }
        DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
    }
    protected void DdlPaper_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlPaper.SelectedValue);
        if (ID > 0)
        {
            DdlQuestion.DataSource = FeedbackQuestionBLL.GetList(ID, SysEnum.QuestionType.Radio);
            DdlQuestion.DataBind();
        }
        else
        {
            DdlQuestion.DataSource = null;
            DdlQuestion.DataBind();
        }
        DdlQuestion.Items.Insert(0, new ListItem("请选择", "0"));
    }
}
