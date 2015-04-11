using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_WorkGroupEmail_Edit : _BaseData_WorkGroupEmail
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlWorkGroup.DataSource = WorkGroupBLL.GetList();
            DdlWorkGroup.DataBind();
            DdlWorkGroup.Items.Insert(0, new ListItem("请选择", "0"));


            WorkGroupEmailInfo info = GetInfo();
            if (info != null)
            {
                TxbName.Text = info.Name;
                TxbEmail.Text = info.Email;
                TxbOrderNo.Text = info.OrderNo.ToString();
                DdlWorkGroup.SelectedValue = info.GroupID.ToString();
                DdlWorkGroup_SelectedIndexChanged(null, null);
                DdlEmailGroup.SelectedValue = info.EmailGroupID.ToString();
                CbEnable.Checked = info.Enable;
            }
            else
            {
                LtlAction.Text = "添加";
            }
        }
    }

    private WorkGroupEmailInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (WorkGroupEmailInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        WorkGroupEmailInfo info = WorkGroupEmailBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (!DataValidator.IsEmail(TxbEmail.Text.Trim(), UpdatePanel1, this.GetType(), "邮箱", 2, 50))
        {
            return;
        }
        if (!DataValidator.IsLen(TxbName.Text.Trim(), UpdatePanel1, this.GetType(), "用户名", 1, 100))
        {
            return;
        }
        if (DdlWorkGroup.SelectedValue == "0")
        {
            Function.AlertMsg("请选择工作组");
            return;
        }
        if (Function.ConverToInt(TxbOrderNo.Text, Function.ErrorNumber) == Function.ErrorNumber)
        {
            Function.AlertMsg("排序号请输入一个整数");
            return;
        }


        WorkGroupEmailInfo info = GetInfo();
        if (info == null)
        {
            info = new WorkGroupEmailInfo();
        }
        info.Name = TxbName.Text.Trim();
        info.Email = TxbEmail.Text.Trim();
        info.GroupID = Function.ConverToInt(DdlWorkGroup.SelectedValue);
        info.EmailGroupID = Function.ConverToInt(DdlEmailGroup.SelectedValue);
        info.BrandID = 0;
        info.OrderNo = Function.ConverToInt(TxbOrderNo.Text.Trim());
        info.Enable = CbEnable.Checked;
        bool result = false;
        if (ViewState["INFO"] == null)
        {
            result = (WorkGroupEmailBLL.Add(info) > 0);
        }
        else
        {
            result = WorkGroupEmailBLL.Edit(info);
        }

        if (result)
        {

            Function.AlertRefresh(LtlAction.Text + "成功", "main");
        }
        else
        {
            Function.AlertMsg(LtlAction.Text + "失败");
        }
    }
    protected void DdlWorkGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlWorkGroup.SelectedValue);
        List<EmailGroupInfo> list = new List<EmailGroupInfo>();
        if (ID > 0)
        {
            list = EmailGroupBLL.GetList(ID);
        }
        DdlEmailGroup.DataSource = list;
        DdlEmailGroup.DataBind();

        DdlEmailGroup.Items.Insert(0, new ListItem("不在组里，独立", "0"));
    }
}
