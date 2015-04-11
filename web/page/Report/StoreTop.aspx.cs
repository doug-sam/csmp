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

public partial class page_Stat_StoreTop : _Report_ReportK
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));
            TxtDateBegin.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            TxbDateEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

    }

    protected void BtnSch_Click(object sender, EventArgs e)
    {


        DateTime DateBegin = Function.ConverToDateTime(TxtDateBegin.Text.Trim());
        DateTime DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        if (DateEnd <= DateBegin)
        {
            Function.AlertBack("开始月份必需小于结束月份");
            return;
        }
        int CustomerID = Function.ConverToInt(DdlCustomer.SelectedValue, 0);
        int BrandID = Function.ConverToInt(DdlBrand.SelectedValue, 0);
        if (CustomerID<=0)
        {
            Function.AlertBack("请选择客户");
        }

        decimal Sum = 0;
        DataTable dt = StatBLL.StatCount8_StoreTop(DateBegin, DateEnd, CustomerID, BrandID);

        StringBuilder sbColumn = new StringBuilder();
        StringBuilder sbPrecent = new StringBuilder();
        //StringBuilder sbCount = new StringBuilder();
        foreach (DataRow item in dt.Rows)
        {
            sbColumn.Append("'").Append(item["StatColumn"]).Append("',");

            sbPrecent.Append(item["StatCount"]).Append(",");
            Sum += Function.ConverToDecimal(item["StatCount"]);
        }
        LtlFootTxt.Text = sbColumn.ToString().Trim(',');
        LtlPrecent.Text = sbPrecent.ToString().Trim(',');
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "Draw();", true);

        StringBuilder strSQL = new StringBuilder();
        strSQL.Append(" 1=1 ");
        strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");
        strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");
        if (CustomerID > 0)
        {
            strSQL.Append(" and  sys_Calls.f_CustomerID=").Append(CustomerID);
        }
        if (BrandID > 0)
        {
            strSQL.Append(" and  sys_Calls.f_BrandID=").Append(BrandID);
        }

        int TotalSum = 0;
        CallBLL.GetList(1, 1, strSQL.ToString(), out TotalSum);
        LtlCount.Text = string.Format(" 当前十个城市的总报修数为：{0}个；总共有{1}个", Sum, TotalSum);
        DataTable dtBind = new DataTable();
        dtBind.Columns.Add("StatColumn");
        dtBind.Columns.Add("Count");
        dtBind.Columns.Add("PercentCurrent");
        dtBind.Columns.Add("PercentTotal");
        foreach (DataRow item in dt.Rows)
        {
            DataRow dr = dtBind.NewRow();
            dr["StatColumn"] = item["StatColumn"];
            dr["Count"] = item["StatCount"];
            dr["PercentCurrent"] = Sum == 0 ? 0 : Math.Round(Function.ConverToDecimal(item["StatCount"]) / Sum * 100, 2);
            dr["PercentTotal"] = TotalSum == 0 ? 0 : Math.Round(Function.ConverToDecimal(item["StatCount"]) / (decimal)TotalSum * 100, 2);
            dtBind.Rows.Add(dr);
        }
        GridView1.DataSource = dtBind;
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
