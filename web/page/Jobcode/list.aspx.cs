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


public partial class page_Jobcode_list : _BaseData_Jobcode
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlWorkGroup.DataSource = WorkGroupBLL.GetList();
            DdlWorkGroup.DataBind();
            DdlWorkGroup.Items.Insert(0, new ListItem("不限", "0"));

            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url;
            string strWhere;
            SchWhere(out url, out strWhere);
            GridView1.DataSource = JobcodeBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

        }
    }

    private void SchWhere(out string url, out string strWhere)
    {
        url = "";
        strWhere = " 1=1 ";
        int WorkGroupID = Function.GetRequestInt("WorkGroupID");
        if (WorkGroupID > 0)
        {
            strWhere += " AND f_WorkGroupID="+WorkGroupID;
            url += "&WorkGroupID=" + WorkGroupID;
            DdlWorkGroup.SelectedValue = WorkGroupID.ToString();
        }
        strWhere += " order by f_CodeNo ";
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "WorkGroupID").ToString());
        Label Control1 = (Label)e.Row.FindControl("LabWorkGroup");
        WorkGroupInfo info = WorkGroupBLL.Get(ID);
        if (null!=info)
        {
            Control1.Text = info.Name;
        }
    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?WorkGroupID=" + DdlWorkGroup.SelectedValue;
        Response.Redirect(Url);
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
                JobcodeBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }

}
