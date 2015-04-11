using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_EmailRecord_list : _Report_EmailRecord
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlUser.DataSource = UserBLL.GetList(CurrentUser.WorkGroupID, SysEnum.Rule.二线.ToString());
            DdlUser.DataBind();
            DdlUser.Items.Insert(0, new ListItem("不限", "0"));
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

        int UserID = Function.GetRequestInt("UserID");
        DateTime DateBegin = Function.GetRequestDateTime("DateBegin");
        DateTime DateEnd = Function.GetRequestDateTime("DateEnd");
        if (UserID > 0)
        {
            strWhere += " and f_UserID=" + UserID;
            url += "&UserID=" + UserID;
            DdlUser.SelectedValue = UserID.ToString();
        }
        if (DateBegin != Function.ErrorDate)
        {
            strWhere += " and DATEDIFF(day,[f_DateAdd],'" + DateBegin + "')<=0 ";
            url += "&DateBegin=" + DateBegin;
            TxbDateBegin.Text = DateBegin.ToString("yyyy-MM-dd");
        }
        if (DateEnd != Function.ErrorDate)
        {
            strWhere += " and DATEDIFF(day,[f_DateAdd],'" + DateEnd + "')>=0 ";
            url += "&DateEnd=" + DateEnd;
            TxbDateEnd.Text = DateEnd.ToString("yyyy-MM-dd");
        }

        strWhere += " order by id desc ";

        GridView1.DataSource = EmailRecordBLL.GetList(PageSize, PageIndex, strWhere, out count);
        GridView1.DataBind();
        this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

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
                if (EmailRecordBLL.Delete(ID))
                {
                    Flag++;
                }
            }
        }
        Function.AlertRefresh(Flag + "条数据删除成功");
    }


    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?UserID=" + DdlUser.SelectedValue;
        Url += "&DateBegin=" + TxbDateBegin.Text;
        Url += "&DateEnd=" + TxbDateEnd.Text;
        Response.Redirect(Url);
    }
    protected void BtnDeleteCurrent_Click(object sender, EventArgs e)
    {
        int UserID = Function.ConverToInt(DdlUser.SelectedValue,0);
        DateTime DateStart = Function.ConverToDateTime(TxbDateBegin.Text);
        DateTime DateEnd = Function.ConverToDateTime(TxbDateEnd.Text);

        if (DateStart==Function.ErrorDate||DateEnd==Function.ErrorDate)
        {
            Function.AlertMsg("日期范围是必填的删除条件！");
            return;
        }
        bool Result = EmailRecordBLL.Delete(UserID, 0, DateStart, DateEnd);
        if (Result)
        {
            Function.AlertRefresh("删除成功啦!");
            return;
        }
        else
        {
            Function.AlertMsg("删除失败了，请告知管理员");
        }


    }

}
