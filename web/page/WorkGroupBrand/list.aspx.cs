using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_WorkGroupBrand_list : _User_WorkWorkGroupBrand
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlBrand.DataSource = BrandBLL.GetList();
            DdlBrand.DataBind();
            DdlBrand.Items.Insert(0, new ListItem("不限", "0"));

            DdlWorkGroup.DataSource = WorkGroupBLL.GetList();
            DdlWorkGroup.DataBind();
            DdlWorkGroup.Items.Insert(0, new ListItem("不限", "0"));

            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url = "";
            string strWhere = " 1=1   ";
            int BrandID = Function.GetRequestInt("BrandID");
            int WorkGroupID = Function.GetRequestInt("WorkGroupID");
            if (BrandID > 0)
            {
                DdlBrand.SelectedValue = BrandID.ToString();
                strWhere += " AND f_MID=" + BrandID;
                url += "&BrandID=" + BrandID;
            }
            if (WorkGroupID > 0)
            {
                DdlWorkGroup.SelectedValue = WorkGroupID.ToString();
                strWhere += " AND f_WorkGroupID=" + WorkGroupID;
                url += "&WorkGroupID=" + WorkGroupID;
            }

            GridView1.DataSource = WorkGroupBrandBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);


        }
    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?BrandID=" + DdlBrand.SelectedValue;
        Url += "&WorkGroupID=" + DdlWorkGroup.SelectedValue;
        Response.Redirect(Url);
    }
    protected void Btn_Delete(object sender, EventArgs e)
    {
        string delList = Function.GetRequestSrtring("ckDel");
        if (string.IsNullOrEmpty(delList))
        {
            Function.AlertMsg("没有选中数据"); return;
        }
        foreach (string item in delList.Split(','))
        {
            if (item.Length > 0)
            {
                WorkGroupBrandBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }
}
