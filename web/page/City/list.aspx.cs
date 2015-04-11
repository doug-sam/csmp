using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_City_list : _BaseData_ProvinceCity
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("不限", "0"));
            Sch();
            
        }
    }

    private void Sch()
    {
        int PageSize = 20;  //每页记录数
        int PageIndex = Function.GetRequestInt("page");  //当前页码
        int page = 10;      //分页显示数
        int count = 0;     //记录总数
        string url = "";
        string strWhere = " 1=1 ";

        int ProvinceID = Function.GetRequestInt("ProvinceID");
        if (ProvinceID>0)
        {
            strWhere += " and f_ProvinceID="+ProvinceID;
            url += "&ProvinceID="+ProvinceID;
            DdlProvince.SelectedValue = ProvinceID.ToString();
        }
        string wd = Function.ClearText(Function.GetRequestSrtring("wd"));
        if (!string.IsNullOrEmpty(wd))
        {
            strWhere +=string.Format(" and f_Name like '%{0}%'",wd);
            url += "&wd="+wd;
            TxbName.Text = wd;
        }


        GridView1.DataSource = CityBLL.GetList(PageSize, PageIndex, strWhere, out count);
        GridView1.DataBind();
        this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int ProvinceID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ProvinceID").ToString());
        Literal Literal1 = (Literal)e.Row.FindControl("LtlProvince");
        ProvincesInfo info = ProvincesBLL.Get(ProvinceID);
        if (null != info)
        {
            Literal1.Text = info.Name;
        }

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
                if (CityBLL.Delete(ID))
                {
                    Flag++;
                }
            }
        }
        Function.AlertRefresh(Flag + "条数据删除成功，\n如果数据未能删除，是由于有店铺属于该城市，\n请先删除在该城市下的店铺");
    }


    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?ProvinceID="+DdlProvince.SelectedValue;
        Url += "&wd="+TxbName.Text.Trim();
        Response.Redirect(Url);
    }
}
