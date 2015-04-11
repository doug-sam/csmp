using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;
using System.Text;

public partial class page_PunchIn_list : _Report_PunchIn
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlUser.DataSource = UserBLL.GetList(CurrentUser.WorkGroupID, SysEnum.Rule.现场工程师.ToString());
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
        StringBuilder strWhere=new StringBuilder(" 1=1 ");
        List_Page.Text = string.Empty;

        int UserID = Function.GetRequestInt("UserID");
        DateTime DateBegin = Function.GetRequestDateTime("DateBegin");
        DateTime DateEnd = Function.GetRequestDateTime("DateEnd");
        if (DateBegin == Function.ErrorDate || DateEnd == Function.ErrorDate )
        {
            //List_Page.Text = "你需要限定时间范围和选择员工";
            //return;
        }
        if ((DateEnd-DateBegin).TotalDays>100)
        {
            List_Page.Text = "时间范围不能超过一百天";
            return;
        }


        if (UserID > 0)
        {
            strWhere.Append(" and f_UserID=").Append(UserID);
            url += "&UserID=" + UserID;
            DdlUser.SelectedValue = UserID.ToString();
        }

        if (DateBegin != Function.ErrorDate)
        {
            TxbDateBegin.Text = DateBegin.ToString("yyyy-MM-dd");
            strWhere.Append(" and DATEDIFF(day,'").Append(DateBegin).Append("',f_DateRegisterAbs)>=0 ");
            url += "&DtBegin=" + TxbDateBegin.Text;
        }

        if (DateEnd != Function.ErrorDate)
        {
            TxbDateEnd.Text = DateEnd.ToString("yyyy-MM-dd");
            strWhere.Append(" and DATEDIFF(day,'").Append(DateEnd).Append("',f_DateRegisterAbs)<=0 ");
            url += "&DtBegin=" + TxbDateEnd.Text;
        }
        
        List<PunchInInfo> list = PunchInBLL.GetList(strWhere.ToString());
        List<PunchInViewInfo> listview = PunchInBLL.ToPunchInView(list);
        GridView1.DataSource = listview;
        GridView1.DataBind();

        if (list.Count<=0)
        {
            List_Page.Text = "没有符合的数据";
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        Repeater Rp1=(Repeater)e.Row.FindControl("Rp1");
        List<PunchInItemInfo> list = (List<PunchInItemInfo>)DataBinder.Eval(e.Row.DataItem, "ItemInfo");
        Rp1.DataSource = list;
        Rp1.DataBind();
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
                if (PunchInBLL.Delete(ID))
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
        Url += "&DateBegin=" + TxbDateBegin.Text.Trim();
        Url += "&DateEnd=" + TxbDateEnd.Text.Trim();
        Response.Redirect(Url);
    }
}
