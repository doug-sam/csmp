using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Collections;


public partial class page_WorkGroup_edit : _User_WorkGroup
{
    private static ListItem defaultitem = new ListItem("请选择", "0");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlProvince.DataSource = ProvincesBLL.GetList();
            ddlProvince.DataBind();
            DdlType.DataSource =SysEnum.ToDictionary(typeof(SysEnum.WorkGroupType));
            DdlType.DataBind();

            int ID = Function.GetRequestInt("ID");
            if (ID <= 0)
            {
                LtlAction.Text = "添加";
                ddlProvince.Items.Insert(0, defaultitem);
            }
            else
            {
                LtlAction.Text = "编辑";
                WorkGroupInfo info = WorkGroupBLL.Get(ID);
                if (null == info)
                {
                    Function.AlertMsg("参数有误"); return;
                }
                ViewState["info"] = info;
                TxbName.Text = info.Name;
                DdlType.SelectedValue = info.Type.ToString();
                ddlProvince.SelectedValue = info.ProvinceID.ToString();
                // CbEnable.Checked = !info.Enable;
            }

        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        WorkGroupInfo info;
        if (null == ViewState["info"])
        {
            info = new WorkGroupInfo();
            info.CityID = 0;
            info.CityName = "";
            info.Enable = true;
            
        }
        else
        {
            info = (WorkGroupInfo)ViewState["info"];
           
        }
        info.Name = TxbName.Text.Trim();
        info.ProvinceID = Function.ConverToInt(ddlProvince.SelectedValue);
        info.Type = Function.ConverToInt(DdlType.SelectedValue);
        ProvincesInfo pinfo = ProvincesBLL.Get(info.ProvinceID);
        if (null==pinfo)
        {
            Function.AlertMsg("请选择省份"); return;
        }
        info.ProvinceName = pinfo.Name;
        if (info.Name.Length > 50)
        {
            Function.AlertMsg("组名过长"); return;
        }
        if (string.IsNullOrEmpty(info.Name))
        {
            Function.AlertMsg("组名不能为空"); return;
        }

        if (null == ViewState["info"])
        {
            if (WorkGroupBLL.Add(info) > 0)
            {
                Function.AlertRefresh("添加成功", "main"); return;
            }
            else
            {
                Function.AlertMsg("添加失败，请重试或联系管理员"); return;
            }
        }
        else
        {
            if (WorkGroupBLL.Edit(info))
            {
                Function.AlertRefresh("编辑成功", "main"); return;
            }
            else
            {
                Function.AlertMsg("编辑失败，请重试或联系管理员"); return;
            }
        }
    }
}
