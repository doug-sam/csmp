using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_TrunkNO_TrunkNOEdit : _BaseData_TrunkNO
{
    private static readonly ListItem DdlItemDefault = new ListItem("请选择", "0");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TrunkNOInfo info = GetInfo();
            if (null != info)
            {
                LabAction.Text = "编辑";
                TxbPNO.Text = info.PhysicalNo;
                TxbPNO.Enabled = false;
                TxbVNO.Text = info.VirtualNo;
                TxbDesc.Text = info.Description;
               
            }
        }
    }

    private TrunkNOInfo GetInfo()
    {
        TrunkNOInfo info;
        if (ViewState["info"] != null)
        {
            info = (TrunkNOInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = TrunkNO.Get(ID);
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
        if (string.IsNullOrEmpty(TxbPNO.Text.Trim()))
        {
            Function.AlertMsg("物理号码不能为空"); return;
        }
        if (string.IsNullOrEmpty(TxbVNO.Text.Trim()))
        {
            Function.AlertMsg("400号码不能为空"); return;
        }
        TrunkNOInfo info = GetInfo();
        if (null == info)
        {
            info = new TrunkNOInfo();
            TrunkNOInfo infotemp = TrunkNO.Get(TxbPNO.Text.Trim());
            if (null!=infotemp)
            {
                Function.AlertMsg("号码" + TxbPNO.Text.Trim() + "已存在"); return;
            }
        }

        info.PhysicalNo = TxbPNO.Text.Trim();
        info.VirtualNo = TxbVNO.Text.Trim();
        info.Description = TxbDesc.Text.Trim();
        if (GetInfo() == null)
        {
            if (TrunkNO.Add(info) > 0)
            {
                Function.AlertRefresh("添加成功","main");
            }
            else
            {
                Function.AlertMsg("添加失败");
            }
        }
        else
        {
            if (TrunkNO.Edit(info))
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
