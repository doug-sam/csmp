using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class page_Class1_list : _BaseData_Class
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限","0"));

            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url = "";
            string strWhere = " 1=1 ";
            string wd = Function.GetRequestSrtring("wd");
            if (!string.IsNullOrEmpty(wd))
            {
                strWhere += " and f_Name like '%" + Function.ClearText(wd) + "%' ";
                url += "&wd=" + wd;
                TxbWd.Text = wd;
            }
            int CustomerID = Function.GetRequestInt("CustomerID");
            if (CustomerID > 0)
            {
                strWhere += " and f_CustomerID=" + CustomerID;
                url += "&CustomerID=" + CustomerID;
                DdlCustomer.SelectedValue = CustomerID.ToString();
            }
            //if (!IsAdmin)
            //{
            //    strWhere += " and f_CustomerID in(select f_MID from sys_WorkGroupBrand where f_WorkGroupID=" + CurrentUser.WorkGroupID + ") ";
            //}


            strWhere += " order by id desc ";
            GridView1.DataSource = Class1BLL.GetList(PageSize, PageIndex, strWhere, out count);
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
                Class1BLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "CustomerID").ToString());
        CustomersInfo cinfo = CustomersBLL.Get(ID);
        if (null != cinfo)
        {
            Literal HyperLink1 = (Literal)e.Row.FindControl("LtlCustomer");
            if (null != HyperLink1)
            {
                HyperLink1.Text = cinfo.Name;
            }
        }
    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?wd="+Server.UrlEncode(TxbWd.Text.Trim());
        Url+="&CustomerID="+DdlCustomer.SelectedValue;
        Response.Redirect(Url);
    }
}
