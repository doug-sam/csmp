using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class page_Customer_list : _BaseData_CustomerBrand
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url = "";
            string strWhere = " 1=1 ";
            string wd = Function.GetRequestSrtring("wd");
            if (!string.IsNullOrEmpty(wd))
            {
                strWhere += " and f_Name like '%"+Function.ClearText(wd)+"%' ";
                url += "&wd=" + wd;
                TxbWd.Text = wd;
            }
            strWhere += " order by id desc ";
            GridView1.DataSource = CustomersBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

        }
    }


    protected void Btn_Delete(object sender, EventArgs e)
    {
        string delList = Function.GetRequestSrtring("ckDel");
        if (string.IsNullOrEmpty(delList))
        {
            Function.AlertBack("没有选中数据");
            return;
        }
        foreach (string item in delList.Split(','))
        {
            if (item.Length > 0)
            {
                CustomersBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        //int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ID").ToString());
        //HyperLink HyperLink1 = (HyperLink)e.Row.FindControl("HyperLink1");
        //if (CallStepBLL.GetReview(ID) != null)
        //    HyperLink1.Text = "已回访";
        //HyperLink1.NavigateUrl = "javascript:tb_show('回访', '/page/call/Review.aspx?ID=" + ID + "&TB_iframe=true&height=450&width=730', false);";
        
    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(TxbWd.Text.Trim()))
        {
            Response.Redirect("list.aspx?wd="+TxbWd.Text.Trim());
        }
    }
}
