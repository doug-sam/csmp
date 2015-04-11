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


public partial class page_Class3_list : _BaseData_Class
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList();
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));

            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url;
            string strWhere;
            SchWhere(out url, out strWhere);
            GridView1.DataSource = Class3BLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

        }
    }

    private void SchWhere(out string url, out string strWhere)
    {
        url = "";
        strWhere = " 1=1 ";
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
            url += "&CustomerID=" + CustomerID;
            DdlCustomer.SelectedValue = CustomerID.ToString();
            DdlCustomer_SelectedIndexChanged(null, null);

            int Class1ID = Function.GetRequestInt("Class1ID");
            int Class2ID = Function.GetRequestInt("Class2ID");

            if (Class1ID > 0)
            {
                DdlClass1.SelectedValue = Class1ID.ToString();
                DdlClass1_SelectedIndexChanged(null, null);
                url += "&Class1ID=" + Class1ID;
                if (Class2ID > 0)
                {
                    DdlClass2.SelectedValue = Class2ID.ToString();
                    url += "&Class2ID=" + Class2ID;
                    strWhere += string.Format(" AND f_Class2ID={0} ", Class2ID);
                }
                else
                {
                    strWhere += string.Format(" AND f_Class2ID IN (SELECT ID FROM sys_Class2 WHERE f_Class1ID={0}) ", Class1ID);
                }
            }
            else
            {
                strWhere += string.Format(" and f_Class2ID in(select ID from sys_Class2 where f_Class1ID in(select ID from sys_Class1 where f_CustomerID={0}))", CustomerID);
            }
        }

        strWhere += " order by id desc ";
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "Class2ID").ToString());
        Label Control1 = (Label)e.Row.FindControl("LabClass1");
        Label Control2 = (Label)e.Row.FindControl("LabCustomer");
        Class2Info c2info = Class2BLL.Get(ID);
        if (c2info == null) return;
        Class1Info c1info = Class1BLL.Get(c2info.Class1ID);
        if (c1info == null) return;
        Control1.Text = c1info.Name;
        CustomersInfo Cinfo = CustomersBLL.Get(c1info.CustomerID);
        if (null == Cinfo) return;
        Control2.Text = Cinfo.Name;
    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?CustomerID=" + DdlCustomer.SelectedValue;
        Url += "&Class1ID=" + DdlClass1.SelectedValue;
        Url += "&Class2ID=" + DdlClass2.SelectedValue;
        Url += "&wd=" + Server.UrlEncode(TxbWd.Text.Trim());
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
                Class3BLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }
    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID <= 0)
        {
            DdlClass1.DataSource = null;
            DdlClass1.DataBind();
        }
        else
        {
            DdlClass1.DataSource = Class1BLL.GetList(ID);
            DdlClass1.DataBind();
            DdlClass1.Items.Insert(0, new ListItem("不限", "0"));
        }
    }
    protected void DdlClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlClass1.SelectedValue);
        if (ID <= 0)
        {
            DdlClass2.DataSource = null;
            DdlClass2.DataBind();
        }
        else
        {
            DdlClass2.DataSource = Class2BLL.GetList(ID);
            DdlClass2.DataBind();
            DdlClass2.Items.Insert(0, new ListItem("不限", "0"));
        }
    }
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        string url = "";
        string strWhere = "";
        SchWhere(out url, out strWhere);
        int count = 0;
        List<Class3Info> list = Class3BLL.GetList(999999999, 1, strWhere, out count);
        DataTable dt = ListToTable(list);

        string InnerPath = "~/file/download/" + DateTime.Now.ToString("yyyy-MM") + "/";
        string InnerName ="Class"+ DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        string FilePath = Server.MapPath(InnerPath);
        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }
        string FileAll = FilePath + InnerName;

        Tool.DocHelper.ExportExcel(dt, FileAll);
        Response.Redirect(InnerPath + InnerName);

    }


    private DataTable ListToTable(List<Class3Info> list)
    {

        DataTable dt = new DataTable();
        dt.Columns.Add("客户");
        dt.Columns.Add("大类");
        dt.Columns.Add("中类");
        dt.Columns.Add("小类");
        dt.Columns.Add("SLA");
        dt.Columns.Add("优先级");
        dt.Columns.Add("是否禁用");

        DataRow dr = dt.NewRow();

        foreach (Class3Info item in list)
        {
            dr = dt.NewRow();
            
            Class2Info c2info = Class2BLL.Get(item.Class2ID);
            if (c2info == null) continue;
            Class1Info c1info = Class1BLL.Get(c2info.Class1ID);
            if (c1info == null) continue;
            CustomersInfo Cinfo = CustomersBLL.Get(c1info.CustomerID);
            if (null == Cinfo) continue;


            dr["客户"] = Cinfo.Name;
            dr["大类"] = c1info.Name;
            dr["中类"] = c2info.Name;
            dr["小类"] = item.Name;
            dr["SLA"] = item.SLA;
            dr["优先级"] = item.PriorityName;
            dr["是否禁用"] = item.IsClosed;
            dt.Rows.Add(dr);
        }

        return dt;
    }

}
