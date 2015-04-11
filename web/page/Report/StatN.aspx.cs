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

public partial class page_Report_StatN : _Report_ReportA
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

        }
    }


    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        #region Validator
        DateTime dBegin = Function.ConverToDateTime(TxtDateBegin.Text.Trim());
        DateTime dEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
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
        int CustomerID=Function.ConverToInt(DdlCustomer.SelectedValue);
        int BrandID=Function.ConverToInt(DdlBrand.SelectedValue);
        if (CustomerID<=0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('必需选择一个客户');", true);
            return;
        }

        DataTable dt = StatBLL.StatN(CustomerID, BrandID, CbIsOverLSA.Checked, dBegin, dEnd);
        if (dt == null || dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('无法找到相关统数据');", true);
            return;
        }
        #endregion

        StringBuilder sbColumn = new StringBuilder();
        StringBuilder sbPrecent = new StringBuilder();

        foreach (DataRow item in dt.Rows)
        {
            sbColumn.Append("'").Append(item["StatClass"]).Append("',");
            sbPrecent.Append(item["StatCount"]).Append(",");
        }
        LtlFootTxt.Text = sbColumn.ToString().Trim(',');
        LtlPrecent.Text = sbPrecent.ToString().Trim(',');
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "Draw();", true);

        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID > 0)
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID, CurrentUser.WorkGroupID);
            DdlBrand.DataBind();
        }
        else
        {
            DdlBrand.DataSource = null;
            DdlBrand.DataBind();
        }
        DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ID").ToString());
        HyperLink HyperLink1 = (HyperLink)e.Row.FindControl("HyperLink1");
        string URL="/page/Report/StatList2.aspx";
        URL+="?State=-12";
        URL+="&DtBegin="+TxtDateBegin.Text;
        URL+="&DtEnd="+TxbDateEnd.Text;
        URL+="&CustomerID="+DdlCustomer.SelectedValue;
        URL+="&BrandID="+DdlBrand.SelectedValue;
        URL += "&SLA=2";
        URL += "&Class1ID=" + ID;
        HyperLink1.NavigateUrl = URL;
    }

}
