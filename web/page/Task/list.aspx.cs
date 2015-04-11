using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class page_Task_list : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url=string.Empty;
            string strWhere = " 1=1 ";
            GridView1.DataSource = TaskBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

        }
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.DataItem == null) return;
        //int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "WorkGroupID").ToString());
        //Label Control1 = (Label)e.Row.FindControl("LabWorkGroup");
        //WorkGroupInfo info = WorkGroupBLL.Get(ID);
        //if (null!=info)
        //{
        //    Control1.Text = info.Name;
        //}
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
                TaskBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }

}
