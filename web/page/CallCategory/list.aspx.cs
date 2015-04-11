using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_CallCategory_list : _BaseData_CallCategory
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
            string strWhere = " 1=1 order by f_OrderID desc,id asc";


            GridView1.DataSource = CallCategoryBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        //int ProvinceID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ProvinceID").ToString());
        //Literal Literal1 = (Literal)e.Row.FindControl("LtlProvince");
        //ProvincesInfo info = ProvincesBLL.Get(ProvinceID);
        //if (null != info)
        //{
        //    Literal1.Text = info.Name;
        //}

    }

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        string delList = Function.GetRequestSrtring("ckDel");
        if (string.IsNullOrEmpty(delList))
        {
            Function.AlertBack("没有选中数据");
            return;
        }
        int Flag = 0;
        foreach (string item in delList.Split(','))
        {
            int ID = Function.ConverToInt(item);
            if (ID > 0)
            {
                if (CallCategoryBLL.Delete(ID))
                {
                    Flag++;
                }
            }
        }
        Function.AlertRefresh(Flag + "条数据删除成功");
    }


}
