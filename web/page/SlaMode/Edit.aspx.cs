using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Text;

public partial class page_SlaMode_Edit : _BaseData_SLAModel
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           

            SlaModeInfo info = GetInfo();
            if (null == info)
            {
            }
            else
            {
                LabAction.Text = "编辑";
                TxbName.Text = info.Name;
            }
        }
    }

    private SlaModeInfo GetInfo()
    {
        SlaModeInfo info;
        if (ViewState["info"] != null)
        {
            info = (SlaModeInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            
            info = SlaModeBLL.Get(ID);
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
        SlaModeInfo info = GetInfo();
        if (null == info)
        {
            
            info = new SlaModeInfo();
        }
        string modeName = TxbName.Text.Trim();

        if (modeName.Length != Encoding.Default.GetByteCount(modeName))
        {
            Function.AlertMsg("输入的符号必须为*-:(),即输入的符号为非中文符号");
            return;
        }

        info.Name = TxbName.Text.Trim();
        if (info.Name.Length > 50)
        {
            Function.AlertMsg("店铺名称过长"); return;
        }

        if (GetInfo() == null)
        {
            if (SlaModeBLL.Add(info) > 0)
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
            if (SlaModeBLL.Edit(info))
            {
                Function.AlertRefresh("修改成功","main");
            }
            else
            {
                Function.AlertMsg("修改失败");
            }
        }

    }
}
