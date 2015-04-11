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

public partial class page_Report_BrandSolvedBy : _Report_ReportH
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));
            TxtDateBegin.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            TxbDateEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");

        }
    }


    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        DateTime DateBegin = Function.ConverToDateTime(TxtDateBegin.Text.Trim());
        DateTime DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        int CustomerID = Function.ConverToInt(DdlCustomer.SelectedValue, 0);
        int BrandID = Function.ConverToInt(DdlBrand.SelectedValue, 0);
        if (DateBegin == Function.ErrorDate)
        {
            Function.AlertMsg("开始日期有误"); return;
        }
        if (DateEnd == Function.ErrorDate)
        {
            Function.AlertMsg("结束日期有误"); return;
        }
        if (DateBegin > DateEnd)
        {
            Function.AlertMsg("开始日期不能大于结束日期"); return;
        }
        if (CustomerID <= 0)
        {
            Function.AlertMsg("请选择客户"); return;
        }

        DataTable dtUnType = StatBLL.StatCount10_BrandSloveBy(DateBegin, DateEnd, CustomerID,BrandID);

        #region datatable类型转换
        DataTable dt = new DataTable();
        foreach (DataColumn item in dtUnType.Columns)
        {
            dt.Columns.Add(item.ColumnName, typeof(string));
        }
        for (int i = 0; i < dtUnType.Rows.Count; i++)
        {
            DataRow drTemp = dt.NewRow();
            dt.Rows.Add(drTemp);
            for (int j = 0; j < dtUnType.Columns.Count; j++)
            {
                dt.Rows[i][j] = dtUnType.Rows[i][j];
            }
        }
        #endregion

        #region 总量列
        dt.Columns.Add("总量");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            int SumRow = 0;
            for (int j = 1; j < dt.Columns.Count - 1; j++)
            {
                SumRow += Function.ConverToInt(dt.Rows[i][j], 0);
            }
            dt.Rows[i]["总量"] = SumRow;
        }
        #endregion

        #region 总量行
        DataRow dr = dt.NewRow();
        dr[0] = "累计";
        for (int j = 1; j < dt.Columns.Count; j++)
        {
            int SumColumn = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SumColumn += Function.ConverToInt(dt.Rows[i][j], 0);
            }
            dr[j] = SumColumn;
        }
        dt.Rows.Add(dr);
        #endregion

        #region 比例
        int SumTotal = Function.ConverToInt(dt.Rows[dt.Rows.Count - 1][dt.Columns.Count - 1], 0);
        dr = dt.NewRow();
        dr[0] = "比例";
        for (int i = 1; i < dt.Columns.Count; i++)
        {
            if (0 == SumTotal)
            {
                dr[i] = "0";
            }
            else
            {
                dr[i] = Math.Round(Function.ConverToDecimal(dt.Rows[dt.Rows.Count - 1][i]) / (decimal)SumTotal*100, 2)+"%";
            }
        }
        dt.Rows.Add(dr);
        
        #endregion

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
