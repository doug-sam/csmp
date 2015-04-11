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
        }
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        string ActionName=Function.GetRequestSrtring("Action");
        List<WorkGroupEmailInfo> list = new List<WorkGroupEmailInfo>();
        List<EmailGroupInfo> listg = new List<EmailGroupInfo>();
        string delList = Function.GetRequestSrtring("ckDel");
        string ckEmailGroup = Function.GetRequestSrtring("ckEmailGroup");
        foreach (string item in delList.Split(','))
        {
           WorkGroupEmailInfo info= WorkGroupEmailBLL.Get(Function.ConverToInt(item));
           if (null!=info&&info.ID>0)
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
}
