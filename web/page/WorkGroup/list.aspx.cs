using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Collections;

public partial class page_WorkGroup_list : _User_WorkGroup
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("不限", "0"));

            DdlType.DataSource = SysEnum.ToDictionary(typeof(SysEnum.WorkGroupType));
            DdlType.DataBind();
            DdlType.Items.Insert(0, new ListItem("不限", "0"));


            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url = "";
            string strWhere = " 1=1   ";
            int ProvinceID = Function.GetRequestInt("ProvinceID");
            if (ProvinceID > 0)
            {
                strWhere += " and f_ProvinceID=" + ProvinceID + " ";
                url += "&ProvinceID=" + ProvinceID;
                DdlProvince.SelectedValue = ProvinceID.ToString();
            }
            int WType = Function.GetRequestInt("WType");
            if (WType > 0)
            {
                strWhere += " and f_Type=" + WType + " ";
                url += "&WType=" + WType;
                DdlType.SelectedValue = WType.ToString();
            }

            strWhere += " order by ID desc  ";
            GridView1.DataSource = WorkGroupBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);
        }
    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?ProvinceID=" + DdlProvince.SelectedValue;
        Url += "&WType=" + DdlType.SelectedValue;
        Response.Redirect(Url);
    }
}
