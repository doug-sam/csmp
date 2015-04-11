using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_CallCategory_Edit : _BaseData_CallCategory
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            for (int i = 0; i < 20; i++)
            {
                DdlOrderID.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            CallCategoryInfo info = GetInfo();
            if (info != null)
            {
                TxbName.Text = info.Name;
                DdlOrderID.SelectedValue = info.OrderID.ToString();
                CbEnable.Checked = info.Enable;
            }
            else
            {
                LtlAction.Text = "添加";
            }
        }
    }

    private CallCategoryInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (CallCategoryInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        CallCategoryInfo info = CallCategoryBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (!DataValidator.IsLen(TxbName.Text.Trim(), UpdatePanel1, this.GetType(), "标题", 1, 20))
        {
            return;
        }
        CallCategoryInfo info = GetInfo();
        if (info==null)
        {
            info = new CallCategoryInfo();
        }
        info.Name = TxbName.Text.Trim();
        info.OrderID = Function.ConverToInt(DdlOrderID.SelectedValue);
        info.Enable = CbEnable.Checked;
        bool result = false;

        List<CallCategoryInfo> list = CallCategoryBLL.GetList();
        if (null!=list && list.Find(p=>p.Name==info.Name)!=null)
        {
            Function.AlertMsg("当前名称数据已存在！");
            return;
        }

        if (ViewState["INFO"] == null)
        {
            result = (CallCategoryBLL.Add(info) > 0);
        }
        else
        {
            result = CallCategoryBLL.Edit(info);
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
