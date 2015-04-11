using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_City_Edit : _BaseData_ProvinceCity
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0,new ListItem("请选择","0"));

            CityInfo info = GetInfo();
            if (info != null)
            {
                TxbTitle.Text = info.Name;
                DdlProvince.SelectedValue = info.ProvinceID.ToString();
                CbEnable.Checked = !info.IsClosed;
            }
            else
            {
                LtlAction.Text = "添加";
            }
        }
    }

    private CityInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (CityInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        CityInfo info = CityBLL.Get(ID);
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
        CityInfo info = GetInfo();
        if (info==null)
        {
            info = new CityInfo();
        }
        info.Name = TxbTitle.Text.Trim();
        info.ProvinceID = Function.ConverToInt(DdlProvince.SelectedValue);
        info.IsClosed = !CbEnable.Checked;
        if (info.ProvinceID<=0)
        {
            Function.AlertBack("请选择省份");
        }
        bool result = false;
        if (ViewState["INFO"] == null)
        {
            result = (CityBLL.Add(info) > 0);
        }
        else
        {
            result = CityBLL.Edit(info);
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
