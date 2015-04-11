using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.IO;
using System.Data;
using System.Text;

public partial class page_Stat_MonthCount : _Report_ReportJ
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));
            TxtDateBegin.Text = DateTime.Now.Year + "-" + "01";
            TxbDateEnd.Text = DateTime.Now.ToString("yyyy-MM");
        }

    }

    protected void BtnSch_Click(object sender, EventArgs e)
    {

        
        DateTime DateBegin = Function.ConverToDateTime(TxtDateBegin.Text.Trim());
        DateTime DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        DateEnd = DateTimeHelper.getMonthEnd(DateEnd.Year, DateEnd.Month);
        DateBegin = DateTimeHelper.getMonthFirst(DateBegin.Year, DateBegin.Month);
        if (DateEnd <= DateBegin)
        {
            Function.AlertBack("开始月份必需小于结束月份");
            return;
        }
        int CustomerID = Function.ConverToInt(DdlCustomer.SelectedValue, 0);
        int BrandID = Function.ConverToInt(DdlBrand.SelectedValue, 0);

        DataTable dt = StatBLL.StatCount7_MonthCount(DateBegin, DateEnd, CustomerID, BrandID);

        StringBuilder sbCityName = new StringBuilder();
        StringBuilder sbPrecent = new StringBuilder();


        foreach (DataRow item in dt.Rows)
        {
            sbCityName.Append("'").Append(item["s_Month"]).Append("',");

            sbPrecent.Append(item["s_Count"]).Append(",");
        }
        LtlFootTxt.Text = sbCityName.ToString().Trim(',');
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

}
