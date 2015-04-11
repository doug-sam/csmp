using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_Group_ItemCall2 : _User_PowerGroup
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GroupInfo info = GetInfo();
            if (null==info)
            {
                return;
            }
            LabName.Text = info.Name;
            LabRule.Text =string.Join(",",info.Rule.ToArray());

            List<string> ItemList = info.ItemList2.Split(',').ToList();
            string js = "";
            foreach (string item in ItemList)
            {
                js += "CheckItem('" + item + "');";
            }
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "", js, true);
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

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        string delList = Function.GetRequestSrtring("CbItem");
        GroupInfo info = GetInfo();
        info.ItemList2 = delList;
        if (GroupBLL.Edit(info))
        {
            Application["item2" + info.ID.ToString()] = info.ItemList2;
            Function.AlertMsg("提交成功");   
        }
        else
        {
            Function.AlertMsg("提交失败");
        }
    }


}
