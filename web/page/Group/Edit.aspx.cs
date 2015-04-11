using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;


public partial class page_Group_Edit : _User_PowerGroup
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CblRule.DataSource = SysEnum.ToDictionary(typeof(SysEnum.Rule));
            CblRule.DataBind();
            GroupInfo info = GetInfo();
            if (info != null)
            {
                TxbTitle.Text = info.Name;
                foreach (string Rule in info.Rule)
                {
                    foreach (ListItem CblItem in CblRule.Items)
                    {
                        if (CblItem.Value==Rule)
                        {
                            CblItem.Selected = true;
                        }
                    }
                }
            }
            else
            {
                LtlAction.Text = "添加";
            }
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
        if (!DataValidator.IsLen(TxbTitle.Text.Trim(),UpdatePanel1,this.GetType(),"标题",2,50))
        {
            return;
        }
        GroupInfo info = GetInfo();
        if (info==null)
        {
            info = new GroupInfo();
        }
        info.Name = TxbTitle.Text.Trim();
        info.Rule =new List<string>();
        foreach (ListItem item in CblRule.Items)
        {
            if (item.Selected)
            {
                info.Rule.Add(item.Value);
            }
        }
        if (info.Rule==null||info.Rule.Count==0)
        {
            Function.AlertMsg("必需选一个或以上的角色");
            return;
        }
        bool result = false;
        if (ViewState["INFO"] == null)
        {
            info.CityID = 0;
            info.CityName = "";
            info.Enable = true;
            info.LeaderID = 0;
            info.LeaderName = "";
            info.ItemList = "";
            info.ItemList2 = "";
            info.PowerList = "";
            
            result = (GroupBLL.Add(info) > 0);
        }
        else
        {
            result = GroupBLL.Edit(info);
        }

        if (result)
        {   
            Function.AlertRefresh(LtlAction.Text + "成功","main");   
        }
        else
        {
            Function.AlertMsg(LtlAction.Text + "失败");
        }
    }
}
