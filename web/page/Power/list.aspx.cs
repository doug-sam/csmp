using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class page_Power_list : _User_PowerGroup
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GroupInfo info = GetInfo();
            LabGroup.Text = info.Name;
            LabRule.Text = string.Join(",", info.Rule.ToArray()); 
            rpManages.DataSource = PowerInfo.ToDictionary(typeof(PowerInfo.PMain));
            rpManages.DataBind();
        }
    }

    private GroupInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (GroupInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        GroupInfo info = GroupBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }
    protected void rpManages_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item == null) return;
        int id = Function.ConverToInt(((KeyValuePair<string, int>)e.Item.DataItem).Value);
        Repeater rpSecond = ((Repeater)e.Item.FindControl("rpSecond"));
        rpSecond.DataSource = PowerInfo.GetSecond(id);
        rpSecond.DataBind();

        GroupInfo ginfo = (GroupInfo)ViewState["INFO"];
        if (!string.IsNullOrEmpty(ginfo.PowerList.Trim()) && ginfo.PowerList.Split(',').Contains(id.ToString()))
        {
            ((Literal)e.Item.FindControl("ltlChecked")).Text = "checked";
        }
    }
    protected void rpSecond_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        int id = Function.ConverToInt(((KeyValuePair<string, int>)e.Item.DataItem).Value);
        GroupInfo ginfo = (GroupInfo)ViewState["INFO"];
        if (!string.IsNullOrEmpty(ginfo.PowerList.Trim()) && ginfo.PowerList.Split(',').Contains(id.ToString()))
        {
            ((Literal)e.Item.FindControl("ltlChecked")).Text = "checked";
        }
    }
    protected void btnSub_Click(object sender, EventArgs e)
    {
        GroupInfo ginfo = GetInfo();
        ginfo.PowerList = Function.GetRequestSrtring("manages");
        if (GroupBLL.Edit(ginfo))
        {
            Function.AlertRedirect("修改成功","/page/group/list.aspx");
        }
        else
        {
            Function.AlertBack("修改失败");
        }

    }
}
