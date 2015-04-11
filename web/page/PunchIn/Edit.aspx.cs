using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_PunchIn_Edit : _Report_PunchIn
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            PunchInInfo info = GetInfo();
            if (info != null)
            {
                TxbDateRegisterAbs.Text = info.DateRegisterAbs.ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                LtlAction.Text = "添加";
                Function.AlertBack("参数有误");
            }
        }
    }

    public PunchInInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (PunchInInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        PunchInInfo info = PunchInBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
       
        PunchInInfo info = GetInfo();
        if (info==null)
        {
            return;
        }

        info.DateRegisterAbs = Function.ConverToDateTime(TxbDateRegisterAbs.Text.Trim());
        info.AddByUserID = CurrentUser.ID;
        info.AddByUserName = CurrentUser.Name;


        bool result = false;
        if (ViewState["INFO"] == null)
        {
            result = (PunchInBLL.Add(info) > 0);
        }
        else
        {
            result = PunchInBLL.Edit(info);
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
