using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class page_Solution_list : _BaseData_Solution
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCuostimer.DataSource = CustomersBLL.GetList();
            DdlCuostimer.DataBind();
            DdlCuostimer.Items.Insert(0, new ListItem("不限", "0"));


            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url = "";
            string strWhere = " 1=1 ";


            int CustomerID = Function.GetRequestInt("CustomerID");
            int Class1ID = Function.GetRequestInt("Class1ID");
            int Class2ID = Function.GetRequestInt("Class2ID");
            int Class3ID = Function.GetRequestInt("Class3ID");
            int EnableFlag = Function.GetRequestInt("Enable");

            if (CustomerID > 0)
            {
                strWhere += string.Format(" and f_Class1 in(select ID from sys_Class1 where f_CustomerID={0}) ", CustomerID);
                url += "&CustomerID=" + CustomerID;
                DdlCuostimer.SelectedValue = CustomerID.ToString();
                DdlCuostimer_SelectedIndexChanged(sender, e);
            }
            if (Class1ID > 0)
            {
                strWhere += string.Format(" and f_Class1 ={0} ", Class1ID);
                url += "&Class1ID=" + Class1ID;
                DdlClass1.SelectedValue = Class1ID.ToString();
                DdlClass1_SelectedIndexChanged(sender, e);
            }
            if (Class2ID > 0)
            {
                strWhere += string.Format(" and f_Class2 ={0} ", Class2ID);
                url += "&Class2ID=" + Class2ID;
                DdlClass2.SelectedValue = Class2ID.ToString();
                DdlClass2_SelectedIndexChanged(sender, e);
            }
            if (Class3ID > 0)
            {
                strWhere += string.Format(" and f_Class3 ={0} ", Class3ID);
                url += "&Class3ID=" + Class3ID;
                DdlClass3.SelectedValue = Class3ID.ToString();
            }
            if (EnableFlag==1||EnableFlag==2)
            {
                strWhere += string.Format(" and f_EnableFlag={0} ",EnableFlag==1?1:0);
                url += "&Enable="+EnableFlag;
                DdlEnable.SelectedValue = EnableFlag.ToString();
            }


            strWhere += " order by f_Class1 desc ";
            GridView1.DataSource = SolutionBLL.GetList(PageSize, PageIndex, strWhere, out count);
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
                SolutionBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int Class1 = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "Class1").ToString());
        Literal Ct1 = (Literal)e.Row.FindControl("LCustomer");
        Class1Info c1info = Class1BLL.Get(Class1);
        if (null!=c1info)
        {
            CustomersInfo cinfo = CustomersBLL.Get(c1info.CustomerID);
            if (null!=cinfo)
            {
                Ct1.Text = cinfo.Name;
            }
        }

    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?CustomerID=" + DdlCuostimer.SelectedValue;
        Url += "&Class1ID=" +DdlClass1.SelectedValue;
        Url += "&Class2ID=" + DdlClass2.SelectedValue;
        Url += "&Class3ID=" + DdlClass3.SelectedValue;
        Url += "&Enable=" + DdlEnable.SelectedValue;
        Response.Redirect(Url);
    }
    protected void DdlCuostimer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCuostimer.SelectedValue);
        if (ID>0)
        {
            DdlClass1.DataSource = Class1BLL.GetList(ID);
        }
        else
        {
            DdlClass1.DataSource = null;
        }
        DdlClass1.DataBind();
        DdlClass1.Items.Insert(0, new ListItem("不限", "0"));
    }
    protected void DdlClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlClass1.SelectedValue);
        if (ID > 0)
        {
            DdlClass2.DataSource = Class2BLL.GetList(ID);
        }
        else
        {
            DdlClass2.DataSource = null;
        }
        DdlClass2.DataBind();
        DdlClass2.Items.Insert(0, new ListItem("不限", "0"));
    }
    protected void DdlClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlClass2.SelectedValue);
        if (ID > 0)
        {
            DdlClass3.DataSource = Class3BLL.GetList(ID);
        }
        else
        {
            DdlClass3.DataSource = null;
        }
        DdlClass3.DataBind();
        DdlClass3.Items.Insert(0, new ListItem("不限", "0"));
    }
}
