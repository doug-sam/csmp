using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_ThirdParty_Edit : _BaseData_ThirdParty
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlWorkGroup.DataSource = WorkGroupBLL.GetList();
            DdlWorkGroup.DataBind();
            DdlWorkGroup.Items.Insert(0, new ListItem("请选择", "0"));


            ThirdPartyInfo info = GetInfo();
            
            if (null != info)
            {
                TxbName.Text = info.Name;
                TxbContact.Text = info.Contact;
                DdlWorkGroup.SelectedValue = info.WorkGroupID.ToString();
                CbEnable.Checked = info.Enable;
            }
        }
    }

    private ThirdPartyInfo GetInfo()
    {
        ThirdPartyInfo info;
        if (ViewState["info"] != null)
        {
            info = (ThirdPartyInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = ThirdPartyBLL.Get(ID);
            if (null != info)
            {
                ViewState["info"] = info;
            }
            return info;
        }
        return null;
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        ThirdPartyInfo info = GetInfo();
        if (null == info)
        {
            info = new ThirdPartyInfo();
        }
        


        info.Name = TxbName.Text.Trim();
        info.Contact = TxbContact.Text.Trim();
        info.WorkGroupID = Function.ConverToInt(DdlWorkGroup.SelectedValue, 0);
        info.Enable = CbEnable.Checked;
        
        if (info.Name.Length > 50)
        {
            Function.AlertMsg("名称过长"); return;
        }
        if (info.Contact.Length > 50)
        {
            Function.AlertMsg("联系信息过长"); return;
        }
        if (info.WorkGroupID <= 0)
        {
            Function.AlertMsg("请选择工作组"); return;
        }

        if (GetInfo() == null)
        {
            if (ThirdPartyBLL.Add(info) > 0)
            {
                Function.AlertRefresh("添加成功", "main");
            }
            else
            {
                Function.AlertMsg("添加失败");
            }
        }
        else
        {
            if (ThirdPartyBLL.Edit(info))
            {
                Function.AlertRefresh("修改成功", "main");
            }
            else
            {
                Function.AlertMsg("修改失败");
            }
        }

    }
}
