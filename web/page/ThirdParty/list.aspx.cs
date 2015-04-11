using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class page_ThirdParty_list : _BaseData_ThirdParty
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
            string url = "";
            string strWhere = " 1=1 ";

            int WorkGroupID = Function.GetRequestInt("WorkGroupID");
            if (WorkGroupID > 0)
            {
                DdlWorkGroup.SelectedValue = WorkGroupID.ToString();
                strWhere += string.Format(" and f_WorkGroupID={0} ", WorkGroupID);
                url += "&WorkGroupID=" + WorkGroupID;
            }

            strWhere += " order by ID desc ";
            GridView1.DataSource = ThirdPartyBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

        }
    }


    protected void Btn_Delete(object sender, EventArgs e)
    {
        Function.AlertBack("功能未可能");
        return;
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
                // ManufacturerBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int WorkGroupID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "WorkGroupID").ToString());
        Literal Control1 = (Literal)e.Row.FindControl("LtlWorkGroup");
        WorkGroupInfo pinfo = WorkGroupBLL.Get(WorkGroupID);
        if (null != pinfo) Control1.Text = pinfo.Name;

    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?WorkGroupID=" + DdlWorkGroup.SelectedValue;
        Response.Redirect(Url);
    }
}
