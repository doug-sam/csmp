using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_Province_Edit : _BaseData_ProvinceCity
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ProvincesInfo info = GetInfo();
            if (info != null)
            {
                TxbTitle.Text = info.Name;
                CbEnable.Checked = !info.IsClosed;
            }
            else
            {
                LtlAction.Text = "添加";
            }
        }
    }

    private ProvincesInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (ProvincesInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        ProvincesInfo info = ProvincesBLL.Get(ID);
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
        ProvincesInfo info = GetInfo();
        if (info==null)
        {
            info = new ProvincesInfo();
        }
        info.Name = TxbTitle.Text.Trim();
        info.IsClosed = !CbEnable.Checked;
        bool result = false;
        if (ViewState["INFO"] == null)
        {
            result = (ProvincesBLL.Add(info) > 0);
        }
        else
        {
            result = ProvincesBLL.Edit(info);
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
