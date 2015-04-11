using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_WorkGroupEmail_list : _BaseData_WorkGroupEmail
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            DdlWorkGroup.DataSource = WorkGroupBLL.GetList();
            DdlWorkGroup.DataBind();
            DdlWorkGroup.Items.Insert(0, new ListItem("不限", "0"));
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

        int WorkgroupID = Function.GetRequestInt("WorkgroupID");
        if (WorkgroupID > 0)
        {
            strWhere += " and f_GroupID=" + WorkgroupID;
            url += "&WorkgroupID=" + WorkgroupID;
            DdlWorkGroup.SelectedValue = WorkgroupID.ToString();
            DdlWorkGroup_SelectedIndexChanged(null, null);

            int EmailGroupID = Function.GetRequestInt("EmailGroupID");
            if (EmailGroupID > 0)
            {
                strWhere += " and f_EmailGroupID=" + EmailGroupID;
                url += "&EmailGroupID=" + EmailGroupID;
                DdlEmailGroup.SelectedValue = EmailGroupID.ToString();
            }
        }
        string wd = Function.ClearText(Function.GetRequestSrtring("wd"));
        if (!string.IsNullOrEmpty(wd))
        {
            strWhere += string.Format(" and (f_Name like '%{0}%' or f_Email like '%{0}%' )", wd);
            url += "&wd="+wd;
            TxbName.Text = wd;
        }


        GridView1.DataSource = WorkGroupEmailBLL.GetList(PageSize, PageIndex, strWhere, out count);
        GridView1.DataBind();
        this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int GroupID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "GroupID").ToString());
        int EmailGroupID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "EmailGroupID").ToString());
        Label LabWorkGroup = (Label)e.Row.FindControl("LabWorkGroup");
        Label LtlEmailGroup = (Label)e.Row.FindControl("LabEmailGroup");
        WorkGroupInfo info = WorkGroupBLL.Get(GroupID);
        EmailGroupInfo eginfo = EmailGroupBLL.Get(EmailGroupID);
        if (null != info)
        {
            LabWorkGroup.Text = info.Name;
        }
        if (null != eginfo)
        {
            LtlEmailGroup.Text = eginfo.Name;
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
                if (WorkGroupEmailBLL.Delete(ID))
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
        Url += "?WorkGroupID=" + DdlWorkGroup.SelectedValue;
        Url += "&EmailGroupID=" + DdlEmailGroup.SelectedValue;
        Url += "&wd=" + TxbName.Text.Trim();
        Response.Redirect(Url);
    }
    protected void DdlWorkGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DdlWorkGroup.SelectedValue=="0")
        {
            DdlEmailGroup.DataSource = new List<EmailGroupInfo>();
        }
        else
        {
            DdlEmailGroup.DataSource = EmailGroupBLL.GetList(Function.ConverToInt(DdlWorkGroup.SelectedValue));
        }
        DdlEmailGroup.DataBind();
        DdlEmailGroup.Items.Insert(0, new ListItem("不限", "0"));
    }
}
