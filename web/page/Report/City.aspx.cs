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

public partial class page_Report_City : _Report_ReportA
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
        int CustomerID = Function.ConverToInt(DdlCustomer.SelectedValue, 0);
        int BrandID = Function.ConverToInt(DdlBrand.SelectedValue, 0);
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
        DataTable dt = StatBLL.StatCity(dBegin, dEnd, CustomerID, BrandID);
        if (dt == null || dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('无法找到相关统数据');", true);
            return;
        }
        #endregion

        StringBuilder sbCityName = new StringBuilder();
        StringBuilder sbPrecent = new StringBuilder();
        //StringBuilder sbCount = new StringBuilder();
        int Sum = 0;
        foreach (DataRow item in dt.Rows)
        {
            Sum += Convert.ToInt32(item["F_COUNT"]);
        }
        if (Sum == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('数据发生严重错误，请联系管理员(分母分零)');", true);
            return;
        }
        foreach (DataRow item in dt.Rows)
        {
            sbCityName.Append("'").Append(item["CityName"]).Append("',");

            //sbPrecent.Append(Math.Round((((decimal)Convert.ToInt32(item["F_COUNT"]) / Sum) * 100), 2)).Append(",");
            sbPrecent.Append(item["F_COUNT"]).Append(",");
        }
        LtlFootTxt.Text = sbCityName.ToString().Trim(',');
        LtlPrecent.Text = sbPrecent.ToString().Trim(',');
        //LtlCount.Text = sbCount.ToString().Trim(',');
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "Draw();", true);

        StringBuilder strSQL = new StringBuilder();
        strSQL.Append(" 1=1 ");
        strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(dBegin).Append("')<=0");
        strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(dEnd).Append("')>=0");
        if (CustomerID > 0)
        {
            strSQL.Append(" and  sys_Calls.f_CustomerID=").Append(CustomerID);
        }
        if (BrandID > 0)
        {
            strSQL.Append(" and  sys_Calls.f_BrandID=").Append(BrandID);
        }
        else if (!IsAdmin)
        {
            strSQL.Append(" and  sys_Calls.f_CustomerID in(");
            strSQL.Append(" SELECT  f_MID FROM      sys_WorkGroupBrand where  f_WorkGroupID=").Append(CurrentUser.WorkGroupID);
            strSQL.Append(" ) ");
        }



        int TotalSum = 0;
        CallBLL.GetList(1, 1, strSQL.ToString(), out TotalSum);
        LtlCount.Text = string.Format(" 当前十个城市的总报修数为：{0}个；总共有{1}个", Sum, TotalSum);
        DataTable dtBind = new DataTable();
        dtBind.Columns.Add("CityName");
        dtBind.Columns.Add("Count");
        dtBind.Columns.Add("PercentCurrent");
        dtBind.Columns.Add("PercentTotal");
        foreach (DataRow item in dt.Rows)
        {
            DataRow dr = dtBind.NewRow();
            dr["CityName"] = item["CityName"];
            dr["Count"] = item["F_COUNT"];
            dr["PercentCurrent"] =Sum==0?0: Math.Round(Function.ConverToDecimal(item["F_COUNT"]) / (decimal)Sum * 100,2);
            dr["PercentTotal"] =TotalSum==0?0: Math.Round(Function.ConverToDecimal(item["F_COUNT"]) / (decimal)TotalSum * 100,2);
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
}
