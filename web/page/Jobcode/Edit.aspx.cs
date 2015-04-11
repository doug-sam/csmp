using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_JobCode_Edit : _BaseData_Jobcode
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlWorkGroup.DataSource = WorkGroupBLL.GetList();
            DdlWorkGroup.DataBind();
            DdlWorkGroup.Items.Insert(0, new ListItem("请选择", "0"));
            

            JobcodeInfo info = GetInfo();
            if (null == info)
            {
                DdlWorkGroup.SelectedValue = CurrentUser.WorkGroupID.ToString();
            }
            else
            {

                LabAction.Text = "编辑";
                DdlWorkGroup.SelectedValue = info.WorkGroupID.ToString();
                TxbCodeNo.Text = info.CodeNo;
                TxbMoney.Text = info.Money.ToString("G0");
                TxbTimeAction.Text = info.TimeAction;
                TxbTimeArrive.Text = info.TimeArrive;
            }
        }
    }

    private JobcodeInfo GetInfo()
    {
        JobcodeInfo info;
        if (ViewState["info"] != null)
        {
            info = (JobcodeInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = JobcodeBLL.Get(ID);
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
        JobcodeInfo info = GetInfo();
        if (null == info)
        {
            info = new JobcodeInfo();
            info.ID = 0;
        }

        info.WorkGroupID = Function.ConverToInt(DdlWorkGroup.SelectedValue);
        info.CodeNo = TxbCodeNo.Text.Trim();
        info.Money = Function.ConverToDecimal(TxbMoney.Text.Trim());
        info.TimeAction = TxbTimeAction.Text.Trim();
        info.TimeArrive = TxbTimeArrive.Text.Trim();


        if (info.WorkGroupID<=0)
        {
            Function.AlertMsg("请选择工作组");
            return;
        }
        if (info.Money<=0)
        {
            Function.AlertMsg("请认真填写实际金额");
            return;
        }

        JobcodeInfo dbinfo = JobcodeBLL.Get(info.CodeNo, info.WorkGroupID);
        if (null!=dbinfo&&dbinfo.ID!=info.ID)
        {
            Function.AlertMsg("该工作组下已经存在这个Jobcode了");
            return;
        }
        

        if (GetInfo() == null)
        {
            if (JobcodeBLL.Add(info) > 0)
            {
                Function.AlertMsg("添加成功");
            }
            else
            {
                Function.AlertMsg("添加失败");
            }
        }
        else
        {
            if (JobcodeBLL.Edit(info))
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
