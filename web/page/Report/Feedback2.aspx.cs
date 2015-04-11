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

public partial class page_Report_Feedback2 : _Report_Report
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
        DateTime dBegin = Function.ConverToDateTime(TxtDateBegin.Text.Trim());
        DateTime dEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
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
        if (BrandID <= 0)
        {
            Function.AlertMsg("请选择品牌");
            return;
        }


        #endregion
        DataTable dt = StatBLL.FeedBackRadio(BrandID,1, dBegin, dEnd);
        DataTable dtFinal = new DataTable();
        dtFinal.Columns.Add("问题", typeof(string));
        dtFinal.Columns.Add("平均分", typeof(decimal));
        dtFinal.Columns.Add("4-5分个数", typeof(int));
        foreach (DataRow item in dt.Rows)
        {
            DataRow rowFinal = dtFinal.NewRow();
            rowFinal["问题"] = "问题" + item[0];
            rowFinal["平均分"] =Math.Round(Function.ConverToDecimal(item["nAvg"]),2);
            rowFinal["4-5分个数"] = item["n45"];
            dtFinal.Rows.Add(rowFinal);
        }
        GridView1.DataSource = dtFinal;
        GridView1.DataBind();

        //StringBuilder sbFoot = new StringBuilder();
        //StringBuilder sbData = new StringBuilder();
        //foreach (FeedbackChooseInfo item in listchoose)
        //{
        //    sbFoot.Append("'").Append(item.Name).Append("',");
        //    sbData.Append(item.OrderNumber).Append(",");
        //}

        //LtlFootTxt.Text = sbFoot.ToString().Trim(',');
        //LtlPrecent.Text = sbData.ToString().Trim(',');
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "Draw();", true);

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
