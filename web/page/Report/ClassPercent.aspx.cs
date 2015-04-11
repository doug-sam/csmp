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

public partial class page_Stat_ClassPercent : _Report_ReportK
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));

            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("不限", "0"));


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
        int ProvinceID = Function.ConverToInt(DdlProvince.SelectedValue, 0);
        int CityID = Function.ConverToInt(DdlCity.SelectedValue, 0);
        int Class1ID = Function.ConverToInt(DdlClass1.SelectedValue, 0);
        int Class2ID = Function.ConverToInt(DdlClass2.SelectedValue, 0);
        if (CustomerID <= 0)
        {
            Function.AlertBack("请选择客户");
            return;
        }


        DataTable dt = StatBLL.StatK(CustomerID, BrandID, ProvinceID, CityID, Class1ID,Class2ID, DateBegin, DateEnd);
        decimal SumTotal = 0;
        StringBuilder sbColumn = new StringBuilder();
        StringBuilder sbPrecent = new StringBuilder();
        //StringBuilder sbCount = new StringBuilder();

        DataTable DtBind = new DataTable();
        DtBind.Columns.Add("StatClass");
        DtBind.Columns.Add("StatCount");
        DtBind.Columns.Add("StatPercent");
        foreach (DataRow item in dt.Rows)
        {
            int classid = Function.ConverToInt(item["StatClass"]);
            string className = Class1ID <= 0 ? Class1BLL.Get(classid).Name : Class2ID <= 0 ? Class2BLL.Get(classid).Name : Class3BLL.Get(classid).Name;
           sbColumn.Append("'").Append(className).Append("',");

            sbPrecent.Append(item["StatCount"]).Append(",");

            SumTotal += Function.ConverToDecimal(item["StatCount"]);
  
            DataRow dr = DtBind.NewRow();
            dr["StatClass"] = className;
            dr["StatCount"] = item["StatCount"];
            dr["StatPercent"] = 0;
            DtBind.Rows.Add(dr);
        }
        LtlFootTxt.Text = sbColumn.ToString().Trim(',');
        LtlPrecent.Text = sbPrecent.ToString().Trim(',');
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "Draw();", true);
        LtlCount.Text = string.Format(" 当前条件下报修总数为:{0}个", SumTotal);


        foreach (DataRow item in DtBind.Rows)
        {
            item["StatPercent"] =SumTotal==0?0:Math.Round(Function.ConverToDecimal(item["StatCount"])/SumTotal*100,2);
        }
        GridView1.DataSource = DtBind;
        GridView1.DataBind();

    }
    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID > 0)
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID);
            DdlBrand.DataBind();
            DdlClass1.DataSource = Class1BLL.GetList(ID);
            DdlClass1.DataBind();
        }
        else
        {
            DdlBrand.DataSource = new List<BrandInfo>();
            DdlBrand.DataBind();
            DdlClass1.DataSource = new List<Class1Info>();
            DdlClass1.DataBind();
        }
        DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
        DdlClass1.Items.Insert(0, new ListItem("不限", "0"));
    }

    protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(DdlProvince.SelectedValue);
        DdlCity.Enabled = ID > 0;
        if (ID <= 0)
        {
            DdlCity.DataSource = null;
            DdlCity.DataBind();
        }
        else
        {
            DdlCity.DataSource = CityBLL.GetList(ID);
            DdlCity.DataBind();
            DdlCity.Items.Insert(0, new ListItem("不限", "0"));
        }
    }
    protected void DdlClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlClass1.SelectedValue, 0);
        if (ID>0)
        {
            DdlClass2.DataSource = Class2BLL.GetList(ID);
        }
        else
        {
            DdlClass2.DataSource = new List<Class2Info>();
        }
        DdlClass2.DataBind();
        DdlClass2.Items.Insert(0, new ListItem("不限", "0"));
    }
}
