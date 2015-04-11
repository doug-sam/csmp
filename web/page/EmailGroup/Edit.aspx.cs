using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_EmailGroup_Edit : _BaseData_WorkGroupEmail
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlWorkGroup.DataSource = WorkGroupBLL.GetList();
            DdlWorkGroup.DataBind();
            DdlWorkGroup.Items.Insert(0, new ListItem("请选择", "0"));

            EmailGroupInfo info = GetInfo();
            if (info != null)
            {
                TxbName.Text = info.Name;
                DdlWorkGroup.SelectedValue = info.WorkGroupID.ToString();
                CbEnable.Checked = info.Enable;
            }
            else
            {
                LtlAction.Text = "添加";
            }
        }
    }

    private EmailGroupInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (EmailGroupInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        EmailGroupInfo info = EmailGroupBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (!DataValidator.IsLen(TxbName.Text.Trim(), UpdatePanel1, this.GetType(), "组名", 1, 100))
        {
            return;
        }
        if (DdlWorkGroup.SelectedValue=="0")
        {
            Function.AlertMsg("请选择工作组");
            return;
        }


        EmailGroupInfo info = GetInfo();
        if (info==null)
        {
            info = new EmailGroupInfo();
        }
        info.Name = TxbName.Text.Trim();
        info.WorkGroupID=Function.ConverToInt(DdlWorkGroup.SelectedValue);
        info.Enable=CbEnable.Checked;
        
        bool result = false;
        if (ViewState["INFO"] == null)
        {
            info.AddDate = DateTime.Now;
            result = (EmailGroupBLL.Add(info) > 0);
        }
        else
        {
            result = EmailGroupBLL.Edit(info);
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
