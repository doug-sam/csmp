using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class page_Brand_list : _BaseData_CustomerBrand
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList();
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0,new ListItem("请选择","0"));

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
            int CustomerID = Function.GetRequestInt("CustomerID");
            if (CustomerID>0)
            {
                strWhere += " and f_CustomerID="+CustomerID+" ";
                url += "&CustomerID="+CustomerID;
                DdlCustomer.SelectedValue = CustomerID.ToString();
            }
            strWhere += " order by id desc ";
            GridView1.DataSource = BrandBLL.GetList(PageSize, PageIndex, strWhere, out count);
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
                BrandBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        BrandInfo info = (BrandInfo)e.Row.DataItem;
        SlaModeInfo infom = SlaModeBLL.Get(info.SlaModeID);
        if (null!=infom)
        {
            Literal LtlSLA = (Literal)e.Row.FindControl("LtlSLA");
            LtlSLA.Text = infom.Name;
        }
        
        
    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
            Response.Redirect("list.aspx?CustomerID="+DdlCustomer.SelectedValue+"&wd=" + TxbWd.Text.Trim());
    }
}
