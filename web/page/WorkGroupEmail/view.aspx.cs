using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_WorkGroupEmail_view : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView1.DataSource = WorkGroupEmailBLL.GetList(CurrentUser.WorkGroupID);
            GridView1.DataBind();

            GridView2.DataSource = EmailGroupBLL.GetList(CurrentUser.WorkGroupID);
            GridView2.DataBind();

            string ActionName = GetActionName();
            GetWorkGroupEmailSelected(ActionName);
            GetEmailGroupSelected(ActionName);
        }
    }

    private List<EmailGroupInfo> GetEmailGroupSelected(string ActionName)
    {
        List<EmailGroupInfo> listg = new List<EmailGroupInfo>();
        if (null != Session["EmailGroup" + ActionName])
        {
            listg = (List<EmailGroupInfo>)Session["EmailGroup" + ActionName];
        }
        return listg;
    }

    private List<WorkGroupEmailInfo> GetWorkGroupEmailSelected(string ActionName)
    {
        List<WorkGroupEmailInfo> listEmail = new List<WorkGroupEmailInfo>();
        if (null != Session[ActionName])
        {
            listEmail = (List<WorkGroupEmailInfo>)Session[ActionName];
        }
        return listEmail;
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        WorkGroupEmailInfo info = (WorkGroupEmailInfo)e.Row.DataItem;
        List<WorkGroupEmailInfo> list = GetWorkGroupEmailSelected(GetActionName());
        if (list.Find(p => p.ID == info.ID) != null)
        {
            Literal LtlMakeItCheck = (Literal)e.Row.FindControl("LtlMakeItCheck");
            LtlMakeItCheck.Text = " checked='checked' ";
        }


    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        EmailGroupInfo info = (EmailGroupInfo)e.Row.DataItem;
        List<EmailGroupInfo> list = GetEmailGroupSelected(GetActionName());
        if (list.Find(p => p.ID == info.ID) != null)
        {
            Literal LtlMakeItCheck = (Literal)e.Row.FindControl("LtlMakeItCheck");
            LtlMakeItCheck.Text = " checked='checked' ";
        }

    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        string ActionName = GetActionName();
        List<WorkGroupEmailInfo> list = new List<WorkGroupEmailInfo>();
        List<EmailGroupInfo> listg = new List<EmailGroupInfo>();
        string delList = Function.GetRequestSrtring("ckDel");
        string ckEmailGroup = Function.GetRequestSrtring("ckEmailGroup");
        foreach (string item in delList.Split(','))
        {
            WorkGroupEmailInfo info = WorkGroupEmailBLL.Get(Function.ConverToInt(item));
            if (null != info && info.ID > 0)
            {
                list.Add(info);
            }
        }

        foreach (string item in ckEmailGroup.Split(','))
        {
            EmailGroupInfo info = EmailGroupBLL.Get(Function.ConverToInt(item));
            if (null != info && info.ID > 0)
            {
                listg.Add(info);
            }
        }

        Session[ActionName] = list;
        Session["EmailGroup" + ActionName] = listg;
        string js = "self.parent.InitData('" + ActionName + "');self.parent.tb_remove();";
        ScriptManager.RegisterStartupScript(this.Page, GetType(), "", js, true);
    }

    private string GetActionName()
    {
        string ActionName = Function.GetRequestSrtring("Action");
        return ActionName;
    }
}
