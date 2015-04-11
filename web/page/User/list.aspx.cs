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
using System.IO;
using System.Data;

public partial class page_User_list : _User_User
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            DdlGroup.DataSource = GroupBLL.GetList();
            DdlGroup.DataBind();
            DdlGroup.Items.Insert(0, new ListItem("不限", "0"));

            DdlWorkGroup.DataSource = WorkGroupBLL.GetList();
            DdlWorkGroup.DataBind();
            DdlWorkGroup.Items.Insert(0, new ListItem("不限", "0"));

            DdlRule.DataSource = SysEnum.ToDictionary(typeof(SysEnum.Rule));
            DdlRule.DataBind();
            DdlRule.Items.Insert(0, new ListItem("不限", "0"));

            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url = "";
            string strWhere = " 1=1   ";
            if (!IsAdmin)
            {
                //strWhere += " and f_Code<>'admin' ";
            }

            SchWhere(ref url, ref strWhere);
            GridView1.DataSource = UserBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);
            List<string> HideHead = new List<string>();
            if (!GroupBLL.PowerCheck((int)PowerInfo.P4_User.用户管理))
            {
                HideHead.Add("操作");
                BtnDelete.Visible = false;
            }
        }
    }

    private void SchWhere(ref string url, ref string strWhere)
    {
        int GroupID = Function.GetRequestInt("GroupID");
        if (GroupID > 0)
        {
            strWhere += " AND f_PowerGroupID=" + GroupID;
            url += "&GroupID=" + GroupID;
            DdlGroup.SelectedValue = GroupID.ToString();
        }
        int WorkGroupID = Function.GetRequestInt("WorkGroupID");
        if (WorkGroupID > 0)
        {
            strWhere += " and f_WorkGroupID=" + WorkGroupID;
            url += "&WorkGroupID=" + WorkGroupID;
            DdlWorkGroup.SelectedValue = WorkGroupID.ToString();
        }
        int Rule = Function.GetRequestInt("Rule");
        string RuleName = Enum.GetName(typeof(SysEnum.Rule), Rule);
        if (!string.IsNullOrEmpty(RuleName))
        {
            strWhere += string.Format(" and f_Rule like '%{0}%' ", RuleName);
            url += "&Rule=" + Rule;
            DdlRule.SelectedValue = Rule.ToString();
        }

        int Enable = Function.GetRequestInt("Enable");
        if (Enable >= 0)
        {
            if (Enable > 1)
            {
                Enable = 1;
            }
            strWhere += " AND f_Enable=" + Enable;
            url += "&Enable=" + Enable;
            DdlEnable.SelectedValue = Enable.ToString();
        }

        string wd = Function.ClearText(Function.GetRequestSrtring("wd"));
        if (!string.IsNullOrEmpty(wd))
        {
            strWhere += string.Format(" and f_Name like '%{0}%' or f_Tel like '%{0}%' ", wd);
            url += "&wd=" + wd;
            TxbWd.Text = wd;
        }

        strWhere += " order by f_Enable desc,ID desc ";
    }
    private void GridViewHide(List<string> header)
    {
        foreach (DataControlField column in GridView1.Columns)
        {
            if (header.Contains(column.HeaderText.Trim()))
            { column.Visible = false; }
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int WorkGroupID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "WorkGroupID").ToString());
        Label mycontrol = (Label)e.Row.FindControl("LabWorkGroup");
       WorkGroupInfo info= WorkGroupBLL.Get(WorkGroupID);
        if (null != info)
        {
            mycontrol.Text = info.Name;
        }

    }

    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?GroupID=" + DdlGroup.SelectedValue;
        Url += "&WorkGroupID=" + DdlWorkGroup.SelectedValue;
        Url += "&Rule=" + DdlRule.SelectedValue;
        Url += "&wd=" + TxbWd.Text.Trim();
        Url += "&Enable=" + DdlEnable.SelectedValue;
        Response.Redirect(Url);
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        //string delList = Function.GetRequestSrtring("ckDel");
        //if (string.IsNullOrEmpty(delList))
        //{
        //    Function.AlertBack("没有选中数据");
        //    return;
        //}
        //foreach (string item in delList.Split(','))
        //{
        //    int ID = Function.ConverToInt(item);
        //    if (ID > 0)
        //    {
        //        if (UserBLL.Delete(ID))
        //        {
        //            Flag++;
        //        }
        //    }
        //}
        //Function.AlertRefresh(Flag + "条数据删除成功，\n如果数据未能删除，是由于有店铺属于该城市，\n请先删除在该城市下的店铺");

    }
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        string url = "";
        string strWhere = "";
        SchWhere(ref url, ref strWhere);
        List<UserInfo> list = UserBLL.GetList(strWhere);
        DataTable dt = ListToTable(list);

        string InnerPath = "~/file/download/" + DateTime.Now.ToString("yyyy-MM") + "/";
        string InnerName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        string FilePath = Server.MapPath(InnerPath);
        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }
        string FileAll = FilePath + InnerName;

        Tool.DocHelper.ExportExcel(dt, FileAll);
        Response.Redirect(InnerPath + InnerName);

    }

    private DataTable ListToTable(List<UserInfo> list)
    {

        DataTable dt = new DataTable();
        dt.Columns.Add("登录名");
        dt.Columns.Add("联系电话");
        dt.Columns.Add("电子邮件");
        dt.Columns.Add("最后登录时间");
        dt.Columns.Add("创建时间");
        dt.Columns.Add("角色");
        dt.Columns.Add("工作组");
        dt.Columns.Add("是否启用");


        WorkGroupInfo winfo = null;

        DataRow dr = dt.NewRow();

        foreach (UserInfo item in list)
        {
            winfo = WorkGroupBLL.Get(item.WorkGroupID);
            if (null==winfo)
            {
                winfo = new WorkGroupInfo();
            }
            dr = dt.NewRow();
            dr["登录名"] = item.Name;
            dr["联系电话"] = item.Tel;
            dr["电子邮件"] = item.Email;
            dr["最后登录时间"] = item.LastDate;
            dr["创建时间"] = item.CreateDate;
            dr["角色"] =string.Join(",",item.Rule.ToArray());
            dr["工作组"] = winfo.Name;
            dr["是否启用"] = item.Enable;
            dt.Rows.Add(dr);
        }

        return dt;
    }

}
