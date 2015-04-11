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

public partial class page_Report_Hour : _Report_ReportC
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

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        #region Validator
        DateTime dBegin = new DateTime();
        DateTime dEnd = new DateTime();
        try { dBegin = Convert.ToDateTime(TxtDateBegin.Text.Trim()); }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('开始日期填写有误');", true);
            return;
        }
        try { dEnd = Convert.ToDateTime(TxbDateEnd.Text.Trim()); }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('结束日期填写有误');", true);
            return;
        }
        #endregion

        int CustomerID = Function.ConverToInt(DdlCustomer.SelectedValue, 0);
        int BrandID = Function.ConverToInt(DdlBrand.SelectedValue, 0);
        DataTable dt = StatBLL.StatCityHour(dBegin, dEnd, CustomerID, BrandID);
        if (dt == null)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('无法找到相关统数据');", true);
            return;
        }
        DataTable Resultdt = new DataTable();
        foreach (DataColumn itemColumn in dt.Columns)
        {
            Resultdt.Columns.Add(itemColumn.ColumnName);
        }
        DataRow ResultRow = Resultdt.NewRow();
        for (int i = 1; i <= 24; i++)
        {
            object TargetRows = null;
            foreach (DataRow itemDtRow in dt.Rows)
            {
                if (itemDtRow[0].ToString() == i.ToString())
                {
                    TargetRows = itemDtRow[1];
                    break;
                }
            }
            ResultRow = Resultdt.NewRow();
            ResultRow[0] = i;
            if (null != TargetRows)
            {
                ResultRow[1] = TargetRows;

            }
            else
            {
                ResultRow[1] = 0;
            }
            Resultdt.Rows.Add(ResultRow);
        }

       
        StringBuilder sbFootTxt = new StringBuilder();
        StringBuilder sbData = new StringBuilder();
        foreach (DataRow item in Resultdt.Rows)
        {
            sbFootTxt.Append("'").Append(item[0]).Append("',");
            sbData.Append(item[1]).Append(",");
        }
        LtlFootTxt.Text = sbFootTxt.ToString().Trim(',');
        LtlPrecent.Text = sbData.ToString().Trim(',');
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "Draw();", true);

        GridView1.DataSource = Resultdt;
        GridView1.DataBind();
    }

}
